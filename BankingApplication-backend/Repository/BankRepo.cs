using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class BankRepo : IBankRepo
    {
        private readonly BankingAppDbContext _context;
        public BankRepo(BankingAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BankOrganizationCount>> GetOrganizationCountsByBankAsync()
        {
            return await _context.Organisations
                .GroupBy(org => org.BankName)
                .Select(g => new BankOrganizationCount
                {
                    BankName = g.Key,
                    OrganizationCount = g.Count()
                })
                .ToListAsync();
        }

        public async Task<Bank> AddBank(Bank bank)
        {
            var roleId = await GetOrCreateRoleId("bank");
            var user = new User
            {
                RoleId = roleId
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            bank.UserId = user.UserId;

            bank.IsApproved = "pending";
            bank.IsActive = true;

            var creds = new Creds
            {
                Username = bank.BankEmail,
                Password = bank.BankPassword,
                UserId = user.UserId
            };

            await _context.Credentials.AddAsync(creds);
            await _context.SaveChangesAsync();

            await _context.Banks.AddAsync(bank);
            await _context.SaveChangesAsync();

            return bank;
        }


        public async Task<int> GetOrCreateRoleId(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role == null)
            {
                role = new Role { RoleName = roleName };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }

            return role.RoleId;
        }

        public async Task<Bank> GetBankById(int id)
        {
            return await _context.Banks
                  .Include(b => b.Document)
                  .FirstOrDefaultAsync(b => b.BankId == id);
        }

        public async Task<IEnumerable<Bank>> GetByApprovalStatus(string status)
        {
            return await _context.Banks.Where(b => b.IsApproved == status).Include(d => d.Document).ToListAsync();
        }

        public async Task UpdateBank(Bank bank)
        {
            _context.Banks.Update(bank);
            await _context.SaveChangesAsync();
        }

        public int userIdToBankId(int userId)
        {
            Bank bank = _context.Banks.FirstOrDefault(b => b.UserId == userId);
            return bank.BankId;
        }

        //view org list method pending
    }
}
