using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class ClientTransactionService : IClientTransactionService
    {
        private readonly IClientTransactionRepo _clientTransactionRepo;
        private readonly IOrgRepo _orgRepo;

        public ClientTransactionService(IClientTransactionRepo clientTransactionRepo, IOrgRepo orgRepo)
        {
            _clientTransactionRepo = clientTransactionRepo;
            _orgRepo = orgRepo;
        }
        public async Task<List<TransactionStatusCount>> GetTransactionStatusCountsAsync()
        {
            return await _clientTransactionRepo.GetTransactionStatusCountsAsync();


        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetPendingBeneficiaryTransactions()
        {
            return await _clientTransactionRepo.GetPendingTransactions();
        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetApprovedBeneficiaryTransactions()
        {
            return await _clientTransactionRepo.GetApprovedTransactions();
        }

        public async Task<(IEnumerable<BeneficiaryTransaction> Transactions, int TotalCount)>
            GetBeneficiaryTransactionsByStatusAndOrgId(
                string status,
                int initiatorOrgId,
                DateTime? startDate,
                DateTime? endDate,
                int pageNumber,
                int pageSize)
        {
            return await _clientTransactionRepo.GetBeneficiaryTransactionsByStatusAndOrgId(
                status,
                initiatorOrgId,
                startDate,
                endDate,
                pageNumber,
                pageSize);
        }

        public async Task<bool> ApproveBeneficiaryTransaction(int transactionId)
        {
            // Fetch the transaction by ID
            var transaction = await _clientTransactionRepo.GetTransactionById(transactionId);

            // Check if the transaction exists
            if (transaction == null)
            {
                throw new Exception("Transaction not found.");
            }

            // Fetch the initiator's organisation using its ID
            var initiatorOrg = await _orgRepo.GetOrganisationById(transaction.InitiatorOrgId);

            // Check if the initiator organisation exists
            if (initiatorOrg == null)
            {
                throw new Exception("Initiator organisation not found.");
            }

            // Verify if the initiator has sufficient balance
            if (initiatorOrg.Account.AccountBalance < transaction.Amount)
            {
                throw new Exception("Insufficient balance in initiator's account.");
            }

            // Deduct the transaction amount from the initiator's balance
            initiatorOrg.Account.AccountBalance -= transaction.Amount;

            // Update the transaction status to approved
            transaction.IsApproved = "approved";

            // Update both the organisation and transaction in the repository
            //var updateOrgResult = await _orgRepo.UpdateOrganisation(initiatorOrg);
            var updateTransResult = await _clientTransactionRepo.UpdateTransaction(transaction);

            // Return true if both updates were successful
            return updateTransResult;
        }

        public async Task<bool> RejectBeneficiaryTransaction(int transactionId)
        {
            var transaction = await _clientTransactionRepo.GetTransactionById(transactionId);
           
                transaction.IsApproved = "rejected"; 
                return await _clientTransactionRepo.UpdateTransaction(transaction);
        
        }

        public async Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactions(int organizationId, string status)
        {
            return await _clientTransactionRepo.GetBeneficiaryTransactions(organizationId, status);
        }
    }
}
