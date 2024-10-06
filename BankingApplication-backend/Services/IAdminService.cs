using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<Organisation>> GetPendingOrganisation();
        Task<Admin> CreateAdmin(AdminDto adminDto);
        Task<IEnumerable<Bank>> GetPendingBank();
        Task<bool> UserExists(string userId);
    }
}
