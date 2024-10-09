using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Services
{
    public class ClientTransactionService : IClientTransactionService
    {
        private readonly IClientTransactionRepo _clientTransactionRepo;
        private readonly IOrgRepo _orgRepo;
        private readonly BankingAppDbContext _context;

        public ClientTransactionService(IClientTransactionRepo clientTransactionRepo, IOrgRepo orgRepo, BankingAppDbContext context)
        {
            _clientTransactionRepo = clientTransactionRepo;
            _orgRepo = orgRepo;
            _context = context;
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

        public async Task<int> GetTotalCountByOrgId(int orgId)
        {
            return await _clientTransactionRepo.GetTotalCountByOrgId(orgId);
        }

        public async Task<List<BeneficiaryTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.BeneficiaryTransactions.AsQueryable();

            // Filter by InitiatorOrgId
            query = query.Where(t => t.InitiatorOrgId == orgId);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.IsApproved.Contains(searchTerm)); // Adjust this condition as needed
            }

            // Apply pagination
            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm)
        {
            var query = _context.BeneficiaryTransactions.AsQueryable();

            // Filter by InitiatorOrgId
            query = query.Where(t => t.InitiatorOrgId == orgId);

            // Optionally filter by search term (if applicable)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.IsApproved.Contains(searchTerm)); // Adjust this condition as needed
            }

            // Return total count
            return await query.CountAsync();
        }


        public async Task<List<BeneficiaryTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            return await _clientTransactionRepo.GetTransactionsByOrgId(orgId, searchTerm, startDate, endDate, pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate)
        {
            return await _clientTransactionRepo.GetTotalCountByOrgId(orgId, searchTerm, startDate, endDate);
        }
    }
}

