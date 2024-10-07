using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class EmpTransactionRepository : IEmpTransactionRepository
    {
        private readonly BankingAppDbContext _context;
        public EmpTransactionRepository(BankingAppDbContext context) { _context = context; }

        public async Task<List<TransactionStatusCount>> GetEmployeeTransactionStatusCountsAsync()
        {
            return await _context.EmpTransactions
                .GroupBy(t => t.IsApproved)
                .Select(g => new TransactionStatusCount
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToListAsync();
        }


        public async Task<IEnumerable<EmpTransaction>> GetEmployeeSalaryDisbursements(int organizationId, string status)
        {
            return await _context.EmpTransactions
                .Where(s => s.OrgID == organizationId && s.IsApproved == status).Include(o=>o.Organisation)
                .ToListAsync();
        }
    }
}
