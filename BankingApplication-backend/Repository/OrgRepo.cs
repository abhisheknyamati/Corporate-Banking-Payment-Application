using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class OrgRepo : IOrgRepo
    {
        private readonly BankingAppDbContext _context;
        public OrgRepo(BankingAppDbContext context)
        {
            _context = context;
        }

        public async Task<Organisation> AddOrganisation(Organisation organisation)
        {
            var bank = await _context.Banks.FirstOrDefaultAsync(b => b.BankName == organisation.BankName);

            var roleId = await GetOrCreateRoleIdAsync("org");

            var user = new User
            {
                RoleId = roleId
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            organisation.UserId = user.UserId;
            organisation.IsApproved = "pending";
            organisation.BankId = bank.BankId;
            organisation.IsActive = true;

            var creds = new Creds
            {
                Username = organisation.OrganisationEmail,
                Password = organisation.OrganisationPassword,
                UserId = user.UserId
            };

            await _context.Credentials.AddAsync(creds);
            await _context.SaveChangesAsync();

            await _context.Organisations.AddAsync(organisation);
            await _context.SaveChangesAsync();
            return organisation;
        }

        public async Task<int> GetOrCreateRoleIdAsync(string roleName)
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

        public async Task<List<Organisation>> GetOrganisationsByBankId(int bankId)
        {
            return await _context.Organisations
                .Where(org => org.BankId == bankId)
                .ToListAsync();
        }

        public async Task<Organisation> GetOrganisationById(int id)
        {
            return await _context.Organisations
                .Include(o => o.Document).Include(a => a.Account)
                .FirstOrDefaultAsync(o => o.OrganisationId == id);
        }

        public async Task<IEnumerable<Organisation>> GetByApprovalStatus(string status)
        {
            return await _context.Organisations
                 .Where(o => o.IsApproved == status)
                 .Include(o => o.Account)
                 .ToListAsync();
        }
        public async Task UpdateOrganisation(Organisation organisation)
        {
            _context.Organisations.Update(organisation); // Mark it as modified
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task<IEnumerable<Outbound>> GetOutboundsByAddedBy(int addedById)
        {
            return await _context.OutboundOrgs.Where(o => o.AddedBy == addedById).ToListAsync();
        }

        public Organisation UserIdToOrganisationId(int userId)
        {
            return  _context.Organisations.FirstOrDefault(x => x.UserId == userId);
        }

        public async Task AddOutbound(Outbound outbound)
        {
            await _context.OutboundOrgs.AddAsync(outbound);
            await _context.SaveChangesAsync();
        }

        public async Task<Organisation> GetOrganisationWithAccountAsync(int orgId)
        {
            return await _context.Organisations.Include(o => o.Account).FirstOrDefaultAsync(o => o.OrganisationId == orgId);
        }
    }
}
