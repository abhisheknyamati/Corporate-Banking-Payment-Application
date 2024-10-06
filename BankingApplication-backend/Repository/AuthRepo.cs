using BankingApplication_backend.Data;
using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace BankingApplication_backend.Repository
{
    public class AuthRepo
    {
        private readonly BankingAppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepo(BankingAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GetRoleName(CredDto value)
        {
            string roleName = (from cred in _context.Credentials
                               join User in _context.Users on cred.UserId equals User.UserId
                               join role in _context.Roles on User.RoleId equals role.RoleId
                               where cred.Username == value.UserName && cred.Password == value.Password
                               select role.RoleName).FirstOrDefault();

            return roleName;
        }

        public async Task<Admin> ValidateAdmin(string username, string password)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(u => u.AdminEmail == username);
            if (user == null || user.AdminPassword != password)
            {
                return null;
            }

            return user;
        }

        public async Task<Bank> ValidateBank(string username, string password)
        {
            var user = await _context.Banks.FirstOrDefaultAsync(u => u.BankEmail == username);
            if (user == null || user.BankPassword != password)
            {
                return null;
            }

            return user;
        }

        public async Task<Organisation> ValidateOrganisation(string username, string password)
        {
            var user = await _context.Organisations.FirstOrDefaultAsync(u => u.OrganisationEmail == username);
            if (user == null || user.OrganisationPassword != password)
            {
                return null;
            }

            return user;
        }

        public string GetIssuer()
        {
            return _configuration["Jwt:Issuer"];
        }
        public string GetKey()
        {
            return _configuration["Jwt:Key"];
        }
        public string GetAudience()
        {
            return _configuration["Jwt:Audience"];
        }

        public async Task<string> GetUserRole(int userId)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);
            return user?.Role?.RoleName ?? "User";
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {

            //using (var sha256 = SHA256.Create())
            //{
            //    var inputBytes = Encoding.UTF8.GetBytes(inputPassword);
            //    var hashBytes = sha256.ComputeHash(inputBytes);
            //    var hashString = Convert.ToBase64String(hashBytes);

            //    return hashString == storedPassword; // Compare hashed passwords
            //}
            return inputPassword.Equals(storedPassword);
        }

        public async Task<string> GetBankStatus(int userId)
        {
            var bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == userId);
            return bank?.IsApproved;
        }

        public async Task<string> GetOrganisationStatus(int userId)
        {
            var organisation = await _context.Organisations.FirstOrDefaultAsync(o => o.UserId == userId);
            return organisation?.IsApproved;
        }

    }

 
}
