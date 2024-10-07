using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IBankRepo
    {
        Task<List<BankOrganizationCount>> GetOrganizationCountsByBankAsync();


        Task<Bank> GetBankById(int id);
        Task<Bank> AddBank(Bank bank);
        Task<IEnumerable<Bank>> GetByApprovalStatus(string status);
        Task UpdateBank(Bank bank);
        int userIdToBankId(int userId);
    }
}
