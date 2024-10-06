using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IAdminRepo
    {
        Task<Admin> AddAdmin(AdminDto adminDto);
        Task<bool> UserExists(string userId);
    }
}
