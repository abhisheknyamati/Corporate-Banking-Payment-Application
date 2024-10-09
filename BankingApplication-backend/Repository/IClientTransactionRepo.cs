using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IClientTransactionRepo
    {

        Task<List<BeneficiaryTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
        Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate);
        Task<int> GetTotalCountByOrgId(int orgId);
        Task<List<TransactionStatusCount>> GetTransactionStatusCountsAsync();
        Task<IEnumerable<BeneficiaryTransaction>> GetPendingTransactions();
        Task<IEnumerable<BeneficiaryTransaction>> GetApprovedTransactions();
        Task<(IEnumerable<BeneficiaryTransaction> Transactions, int TotalCount)> GetBeneficiaryTransactionsByStatusAndOrgId(string status, int initiatorOrgId, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
        Task<BeneficiaryTransaction> GetTransactionById(int transactionId);
        Task<bool> UpdateTransaction(BeneficiaryTransaction transaction);
        Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactions(int organizationId, string status);
    }
}
