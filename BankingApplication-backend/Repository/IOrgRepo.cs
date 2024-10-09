using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IOrgRepo
    {
        Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactionsByOrgIdAsync(BeneficiaryTransactionFilterDto filter);
        Task<Inbound> GetByIdAsync(int id);
        Task<List<Inbound>> GetAllExceptAsync(int id);
        Task<IEnumerable<Outbound>> GetActiveOutboundsByAddedBy(int addedById);
        Task<List<OrganizationEmployeeCount>> GetEmployeeCountsByOrganizationAsync();
        Task UpdateOutboundAsync(Outbound outbound);
        Task<Outbound> GetOutboundByIdAsync(int outBoundId);
        Task<List<Outbound>> GetPendingOutboundOrganisationsAsync();
        Task<Employee> GetEmployeeByIdAsync(int employeeId);

        Task<IEnumerable<Outbound>> GetOutboundsByAddedBy(int addedById);
        Task<Organisation> GetOrganisationById(int id);
        Task<List<Organisation>> GetOrganisationsByBankId(int bankId);
        Task<IEnumerable<Organisation>> GetByApprovalStatus(string status);
        Task UpdateOrganisation(Organisation organisation);
        Organisation UserIdToOrganisationId(int userId);
        Task AddOutbound(Outbound outbound);
        Task<Organisation> GetOrganisationWithAccountAsync(int orgId);
        Task<Organisation> AddOrganisation(Organisation organisation);
        Task AddBeneficiaryTransaction(BeneficiaryTransaction transaction);
        Task<IEnumerable<EmpTransaction>> GetEmployeeTransactionsByOrgIdAsync(EmployeeTransactionFilterDto filter);
    

    }
}
