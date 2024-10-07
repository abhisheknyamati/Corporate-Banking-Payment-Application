using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IClientTransactionService
    {
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
