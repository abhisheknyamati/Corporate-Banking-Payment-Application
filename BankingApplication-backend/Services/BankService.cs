using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepo _bankRepo;
        public BankService(IBankRepo bankRepo)
        {
            _bankRepo = bankRepo;
        }
        public async Task<List<BankOrganizationCount>> GetOrganizationCountsByBankAsync()
        {
            return await _bankRepo.GetOrganizationCountsByBankAsync();
        }

        public async Task<Bank> AddBank(Bank bank)
        {
            return await _bankRepo.AddBank(bank);
        }
        public async Task<Bank> GetBankById(int id)
        {
            return await _bankRepo.GetBankById(id);
        }

        public async Task UpdateBank(Bank bank) // New method to update a bank
        {
            await _bankRepo.UpdateBank(bank);
        }

        public int userIdToBankId(int userId)
        {
            return _bankRepo.userIdToBankId(userId);
        }

        public async Task<IEnumerable<Bank>> GetApprovedBanks()
        {
            return await _bankRepo.GetApprovedBanks();
        }

    }
}
