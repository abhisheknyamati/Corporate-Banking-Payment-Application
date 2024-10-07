using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IBankService
    {
        Task<List<BankOrganizationCount>> GetOrganizationCountsByBankAsync();
        Task<Bank> AddBank(Bank bank);
        Task UpdateBank(Bank bank);
        Task<Bank> GetBankById(int id);
        int userIdToBankId(int userId);
    }
}
