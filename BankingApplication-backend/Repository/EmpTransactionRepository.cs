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

        public async Task<int> GetTotalCountByOrgId(int orgId)
        {
            return await _context.EmpTransactions
                .CountAsync(e => e.OrgID == orgId);
        }



        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm)
        {
            var query = _context.EmpTransactions.AsQueryable();

            // Filter by OrgID
            query = query.Where(t => t.OrgID == orgId);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.EmployeeName.Contains(searchTerm) || t.EmployeeEmail.Contains(searchTerm)); // Adjust as needed
            }

            // Return total count
            return await query.CountAsync();
        }


        public async Task<List<EmpTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            var query = _context.EmpTransactions.AsQueryable();

            // Filter by OrgID
            query = query.Where(t => t.OrgID == orgId);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.EmployeeName.Contains(searchTerm) || t.EmployeeEmail.Contains(searchTerm)); // Adjust as needed
            }

            // Filter by date range if provided
            if (startDate.HasValue)
            {
                query = query.Where(t => t.EmployeeTransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.EmployeeTransactionDate <= endDate.Value);
            }

            // Apply pagination
            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.EmpTransactions.AsQueryable();

            // Filter by OrgID
            query = query.Where(t => t.OrgID == orgId);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.EmployeeName.Contains(searchTerm) || t.EmployeeEmail.Contains(searchTerm)); // Adjust as needed
            }

            // Filter by date range if provided
            if (startDate.HasValue)
            {
                query = query.Where(t => t.EmployeeTransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.EmployeeTransactionDate <= endDate.Value);
            }

            // Return total count
            return await query.CountAsync();
        }
    }
}