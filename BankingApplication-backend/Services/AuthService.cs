using BankingApplication_backend.Data;
using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingApplication_backend.Services
{

    public class AuthService
    {
        private readonly AuthRepo _repo;
        private readonly IMailService _mailService;
        public AuthService(AuthRepo repo, IMailService mailService)
        {
            _repo = repo;
            _mailService = mailService;
        }

        public async Task<string> Login(CredDto value)
        {
            if (string.IsNullOrEmpty(value.UserName) || string.IsNullOrEmpty(value.Password))
                throw new ArgumentException("Credentials cannot be empty");

            var roleName = _repo.GetRoleName(value);

            if (roleName == null)
            {
                throw new ArgumentException("Role not found for user");
            }
            dynamic user = null;
            switch (roleName.ToLower())
            {
                case "admin":
                    user = await _repo.ValidateAdmin(value.UserName, value.Password);
                    break;
                case "bank":
                    user = await _repo.ValidateBank(value.UserName, value.Password);
                    break;
                case "org":
                    user = await _repo.ValidateOrganisation(value.UserName, value.Password);
                    break;
                default:
                    throw new ArgumentException("Invalid user type");
            }

            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password");

            var claims = await GenerateClaims(user);
            var token = GenerateToken(claims);
            return token;
        }

        private async Task<Claim[]> GenerateClaims(IUser user)
        {
            var role = await fetchRole(user);
            var status = string.Empty;

            if (role == "bank")
            {
                status = await fetchBankStatus(user.UserId);
            }
            else if (role == "org")
            {
                status = await fetchOrgStatus(user.UserId);
            }
            else if (role == "admin")
            {
                status = "High Status";
            }

            return new[]
            {
            new Claim("UserID", user.UserId.ToString()),
            new Claim("Name", user.Name),
            new Claim("Email", user.Email),
            new Claim("role", role),
            new Claim("status", status),
        };
        }

        private string GenerateToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_repo.GetKey()));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _repo.GetIssuer(),
                audience: _repo.GetAudience(),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> fetchRole(IUser user)
        {
            var role = await _repo.GetUserRole(user.UserId);
            return role;
        }

        private async Task<string> fetchBankStatus(int userId)
        {
            var bankStatus = await _repo.GetBankStatus(userId);
            return bankStatus;
        }

        private async Task<string> fetchOrgStatus(int userId)
        {
            var orgStatus = await _repo.GetOrganisationStatus(userId);
            return orgStatus;
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            var user = await _repo.FindByEmail(email);
            if (user == null)
                throw new Exception("User not found");

            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                Token = Guid.NewGuid().ToString(), // Generate unique token
                ExpiryDate = DateTime.UtcNow.AddHours(1) // Token expiration time
            };

            await _repo.AddPasswordResetToken(resetToken);

            // Send email with reset token link (Email service should handle this)
            var emailSubjet = $"Password Reset Request";
            var resetLink = $"http://localhost:4200/login/reset-password?token={resetToken.Token}";
            var emailBody = $"Click on the following link to reset your password: {resetLink}\n" +
                $"This token is valid for ${resetToken.ExpiryDate.ToLocalTime()}";


            MailData mailData = new MailData()
            {
                EmailTo = email,
                EmailSubject = emailSubjet,
                EmailBody = emailBody,
            };

            _mailService.SendMail(mailData);

            return resetToken.Token;
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {         
            PasswordResetToken resetToken = await _repo.FindByTokenAndUserId(token);
            if (resetToken == null || resetToken.ExpiryDate < DateTime.UtcNow)
                return false; 

            var userCreds = await _repo.FindByUserId(resetToken.UserId);
            
            if (userCreds == null)
                throw new Exception("User not found");
            
            userCreds.Password = newPassword;
            await _repo.UpdateCredentials(userCreds);

            var role = userCreds.User.Role.RoleName;

            if(role == "org")
            {
                await _repo.UpdateOrgCred(userCreds.UserId, newPassword);   
            }else if(role.ToString() == "bank")
            {
                await _repo.UpdateBankCred(userCreds.UserId, newPassword);
            }

            await _repo.RemoveToken(resetToken);

            return true;
        }

        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

}
