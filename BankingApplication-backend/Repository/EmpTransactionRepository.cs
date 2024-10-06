using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class EmpTransactionRepository : IEmpTransactionRepository
    {
        private readonly BankingAppDbContext _context;
        public EmpTransactionRepository(BankingAppDbContext context) { _context = context; }
        public async Task<IEnumerable<EmpTransaction>> GetEmployeeSalaryDisbursements(int organizationId, string status)
        {
            return await _context.EmpTransactions
                .Where(s => s.OrgID == organizationId && s.IsApproved == status)
                .ToListAsync();
        }
    }
}
