using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IClientTransactionService
    {
        Task<List<BeneficiaryTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
        Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate);
        Task<List<BeneficiaryTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize);
        Task<int> GetTotalCountByOrgId(int orgId, string searchTerm);
        Task<int> GetTotalCountByOrgId(int orgId);
        Task<List<TransactionStatusCount>> GetTransactionStatusCountsAsync();
        Task<IEnumerable<BeneficiaryTransaction>> GetPendingBeneficiaryTransactions();
        Task<IEnumerable<BeneficiaryTransaction>> GetApprovedBeneficiaryTransactions();

        Task<(IEnumerable<BeneficiaryTransaction> Transactions, int TotalCount)>
            GetBeneficiaryTransactionsByStatusAndOrgId(
                string status,
                int initiatorOrgId,
                DateTime? startDate,
                DateTime? endDate,
                int pageNumber,
                int pageSize);

        Task<bool> ApproveBeneficiaryTransaction(int transactionId);

        Task<bool> RejectBeneficiaryTransaction(int transactionId);
        Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactions(int organizationId, string status);
    }
}
