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

        public async Task<IEnumerable<BeneficiaryTransaction>> GetPendingTransactions()
        {
            return await _context.BeneficiaryTransactions
                .Where(t => string.IsNullOrEmpty(t.IsApproved)) // Assuming null or empty means pending
                .ToListAsync();
        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetApprovedTransactions()
        {
            return await _context.BeneficiaryTransactions
                .Where(t => t.IsApproved == "Approved") // Adjust based on your approval logic
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
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactions(int organizationId, string status)
        {
            return await _context.BeneficiaryTransactions
                .Where(t => t.InitiatorOrgId == organizationId && t.IsApproved == status)
                .ToListAsync();
        }
    }
}
