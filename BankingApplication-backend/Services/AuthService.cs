using BankingApplication_backend.Data;
using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingApplication_backend.Services
{

    public class AuthService
    {
        private readonly AuthRepo _repo;
        public AuthService(AuthRepo repo)
        {
            _repo = repo;
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

    }

}
