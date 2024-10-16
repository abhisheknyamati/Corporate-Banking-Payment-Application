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
            var credential = (from cred in _context.Credentials
                              join User in _context.Users on cred.UserId equals User.UserId
                              where cred.Username == value.UserName
                              select new { cred.Password, User.RoleId }).FirstOrDefault();

            if (credential == null || !PasswordHelper.VerifyPassword(value.Password, credential.Password))
            {
                return null; 
            }

            string roleName = (from role in _context.Roles
                               where role.RoleId == credential.RoleId
                               select role.RoleName).FirstOrDefault();

            return roleName;
        }


        public async Task<Admin> ValidateAdmin(string username, string password)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(u => u.AdminEmail == username);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.AdminPassword))
            {
                return null;
            }

            return user;
        }

        public async Task<Bank> ValidateBank(string username, string password)
        {
            var user = await _context.Banks.FirstOrDefaultAsync(u => u.BankEmail == username);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.BankPassword))
            {
                return null;
            }

            return user;
        }

        public async Task<Organisation> ValidateOrganisation(string username, string password)
        {
            var user = await _context.Organisations.FirstOrDefaultAsync(u => u.OrganisationEmail == username);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.OrganisationPassword))
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

        public async Task<Creds> FindByUserId(int userId)
        {
            var userCreds = await _context.Credentials
                                          .Include(c => c.User)
                                          .ThenInclude(u => u.Role) // Ensure Role is included
                                          .FirstOrDefaultAsync(c => c.UserId == userId);
            return userCreds;
        }


        public async Task<Creds> FindByEmail(string email) // not needed while resetting password
        {
            var userCreds = _context.Credentials.Where(c => c.Username == email).FirstOrDefault();
            return userCreds;
        }

        public async Task AddPasswordResetToken(PasswordResetToken resetToken)
        {
            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetToken> FindByTokenAndUserId(string token)
        {
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task UpdateCredentials(Creds creds)
        {
            _context.Credentials.Update(creds);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveToken(PasswordResetToken resetToken)
        {
            _context.PasswordResetTokens.Remove(resetToken);
            await _context.SaveChangesAsync();
        }

        //public async Task<string> GetRoleByUserId(int userId)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(r => r.UserId == userId);
        //    var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == user.RoleId);

        //    return role.RoleName;
        //}

        public async Task UpdateOrgCred(int userId, string password)
        {
            Organisation organisation = await _context.Organisations.FirstOrDefaultAsync(r => r.UserId == userId);
            if (organisation != null)
            {
                organisation.OrganisationPassword = password;
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task UpdateBankCred(int userId, string password)
        {
            Bank bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == userId);
            if (bank != null)
            {
                bank.BankPassword = password;
                await _context.SaveChangesAsync(); 
            }
        }

    }
}
