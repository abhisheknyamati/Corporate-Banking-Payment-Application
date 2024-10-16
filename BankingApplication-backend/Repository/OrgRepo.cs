using BankingApplication_backend.Data;
using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace BankingApplication_backend.Repository
{
    public class OrgRepo : IOrgRepo
    {
        private readonly BankingAppDbContext _context;
        public OrgRepo(BankingAppDbContext context)
        {
            _context = context;
        }
        //new
        public async Task<List<Outbound>> GetPendingOutboundOrganisationsAsync()
        {
            return await _context.OutboundOrgs
                .Where(o => o.IsApproved == "pending") 
                .ToListAsync();
        }
        public async Task<Outbound> GetOutboundByIdAsync(int outBoundId)
        {
            return await _context.OutboundOrgs.FindAsync(outBoundId);
        }
        public async Task<List<OrganizationEmployeeCount>> GetEmployeeCountsByOrganizationAsync()
        {
            return await _context.Employees
                .GroupBy(emp => emp.Organisation.OrganisationName)
                .Select(g => new OrganizationEmployeeCount
                {
                    OrganizationName = g.Key,
                    EmployeeCount = g.Count()
                })
                .ToListAsync();
        }
        public async Task UpdateOutboundAsync(Outbound outbound)
        {
            _context.OutboundOrgs.Update(outbound);
            await _context.SaveChangesAsync();
        }



        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Inbound> GetByIdAsync(int id)
        {
            return await _context.InboundOrgs.FindAsync(id);
        }

        public async Task<List<Inbound>> GetAllExceptAsync(int id)
        {   
            var organisation = await _context.Organisations.FirstOrDefaultAsync( o => o.OrganisationId == id);
            return await _context.InboundOrgs
                .Where(i => i.InboundName != organisation.OrganisationName)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmpTransaction>> GetEmployeeTransactionsByOrgIdAsync(EmployeeTransactionFilterDto filter)
        {
            var query = _context.EmpTransactions.AsQueryable();

            query = query.Where(e => e.OrgID == filter.OrgId);

            if (filter.StartDate.HasValue)
            {
                query = query.Where(e => e.EmployeeTransactionDate.Date >= filter.StartDate.Value.Date);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(e => e.EmployeeTransactionDate.Date <= filter.EndDate.Value.Date);
            }

            return await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
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
        public async Task AddBeneficiaryTransaction(BeneficiaryTransaction transaction)
        {
            await _context.BeneficiaryTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Outbound>> GetActiveOutboundsByAddedBy(int addedById)
        {
            var outboundsOrg = await _context.OutboundOrgs
                .Where(o => o.AddedBy == addedById && o.IsApproved == "approved")
                .ToListAsync();

            return outboundsOrg;
        }


        public async Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactionsByOrgIdAsync(BeneficiaryTransactionFilterDto filter)
        {
            var query = _context.BeneficiaryTransactions.AsQueryable();

            // Filter by OrgId
            query = query.Where(b => b.InitiatorOrgId == filter.OrgId);

            if (filter.StartDate.HasValue)
            {
                query = query.Where(b => b.BeneficiaryTransactionDate.Date >= filter.StartDate.Value.Date);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(b => b.BeneficiaryTransactionDate.Date <= filter.EndDate.Value.Date);
            }

            // Apply pagination
            return await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }



    }
}
