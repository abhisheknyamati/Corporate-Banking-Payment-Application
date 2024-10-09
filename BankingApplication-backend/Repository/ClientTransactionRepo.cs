using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class ClientTransactionRepo : IClientTransactionRepo
    {
        private readonly BankingAppDbContext _context;

        public ClientTransactionRepo(BankingAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TransactionStatusCount>> GetTransactionStatusCountsAsync()
        {
            return await _context.BeneficiaryTransactions
                .GroupBy(t => t.IsApproved)
                .Select(g => new TransactionStatusCount
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToListAsync();
        }



        public async Task<IEnumerable<BeneficiaryTransaction>> GetPendingTransactions()
        {
            return await _context.BeneficiaryTransactions
                .Where(t=>t.IsApproved=="pending").Include(o=>o.Inbound).Include(o=>o.Outbound)
                .ToListAsync();
        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetApprovedTransactions()
        {
            return await _context.BeneficiaryTransactions
                .Where(t => t.IsApproved == "approved")
                .ToListAsync();
        }

        public async Task<(IEnumerable<BeneficiaryTransaction> Transactions, int TotalCount)> GetBeneficiaryTransactionsByStatusAndOrgId(
            string status, int initiatorOrgId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            var query = _context.BeneficiaryTransactions.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.IsApproved == status);
            }

            if (initiatorOrgId > 0)
            {
                query = query.Where(t => t.InitiatorOrgId == initiatorOrgId);
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.BeneficiaryTransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.BeneficiaryTransactionDate <= endDate.Value);
            }

            var totalCount = await query.CountAsync();
            var transactions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (transactions, totalCount);
        }

        public async Task<BeneficiaryTransaction> GetTransactionById(int transactionId)
        {
            return await _context.BeneficiaryTransactions.FindAsync(transactionId);
        }

        public async Task<bool> UpdateTransaction(BeneficiaryTransaction transaction)
        {
            _context.BeneficiaryTransactions.Update(transaction);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactions(int organizationId, string status)
        {
            return await _context.BeneficiaryTransactions
                .Where(t => t.InitiatorOrgId == organizationId && t.IsApproved == status).Include(i=>i.Inbound).Include(o=>o.Outbound)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountByOrgId(int orgId)
        {
            return await _context.BeneficiaryTransactions
                .CountAsync(b => b.InitiatorOrgId == orgId);
        }


        public async Task<List<BeneficiaryTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            var query = _context.BeneficiaryTransactions.AsQueryable();

            // Filter by OrgID
            query = query.Where(t => t.InitiatorOrgId == orgId).Include(c => c.Inbound).Include(c => c.Outbound);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.IsApproved.Contains(searchTerm)).Include(c => c.Inbound).Include(c => c.Outbound); // Adjust as needed
            }

            // Filter by date range if provided
            if (startDate.HasValue)
            {
                query = query.Where(t => t.BeneficiaryTransactionDate >= startDate.Value).Include(c => c.Inbound).Include(c => c.Outbound);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.BeneficiaryTransactionDate <= endDate.Value).Include(c => c.Inbound).Include(c => c.Outbound);
            }

            // Apply pagination
            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.BeneficiaryTransactions.AsQueryable();

            // Filter by OrgID
            query = query.Where(t => t.InitiatorOrgId == orgId).Include(c => c.Inbound).Include(c => c.Outbound);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.IsApproved.Contains(searchTerm)).Include(c => c.Inbound).Include(c => c.Outbound); // Adjust as needed
            }

            // Filter by date range if provided
            if (startDate.HasValue)
            {
                query = query.Where(t => t.BeneficiaryTransactionDate >= startDate.Value).Include(c => c.Inbound).Include(c => c.Outbound);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.BeneficiaryTransactionDate <= endDate.Value).Include(c => c.Inbound).Include(c => c.Outbound);
            }

            // Return total count
            return await query.CountAsync();
        }
    }
}

