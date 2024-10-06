using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _adminRepo; 
        private readonly IOrgRepo _orgRepo;
        private readonly IBankRepo _bankRepo;

        public AdminService(IAdminRepo adminRepo, IOrgRepo orgRepo, IBankRepo bankRepo)
        {
            _adminRepo = adminRepo;
            _orgRepo = orgRepo;
            _bankRepo = bankRepo;
        }

        public async Task<IEnumerable<Organisation>> GetPendingOrganisation()
        {
            return await _orgRepo.GetByApprovalStatus("pending");
        }
        public async Task<IEnumerable<Bank>> GetPendingBank()
        {
            return await _bankRepo.GetByApprovalStatus("pending");
        }
        public async Task<Admin> CreateAdmin(AdminDto adminDto)
        {
            if (adminDto == null)
            {
                throw new ArgumentNullException(nameof(adminDto), "Invalid data.");
            }
            return await _adminRepo.AddAdmin(adminDto);
        }
    }
}



