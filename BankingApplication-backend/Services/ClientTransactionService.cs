using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class ClientTransactionService : IClientTransactionService
    {
        private readonly IClientTransactionRepo _clientTransactionRepo;

        public ClientTransactionService(IClientTransactionRepo clientTransactionRepo)
        {
            _clientTransactionRepo = clientTransactionRepo;
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
            var transaction = await _clientTransactionRepo.GetTransactionById(transactionId);
            //write logic here to - transaction amount from intiator
                transaction.IsApproved = "approved"; 
                return await _clientTransactionRepo.UpdateTransaction(transaction);


    
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
