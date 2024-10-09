using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IOrgService
    {
        Task SetOrganisationApprovalPendingAsync(int organisationId);
        Task<IEnumerable<BeneficiaryTransaction>> GetBeneficiaryTransactionsAsync(BeneficiaryTransactionFilterDto filter);
        Task<List<Inbound>> GetInboundsExcludingCurrentAsync(int id);
        Task<IEnumerable<Outbound>> GetActiveOutboundsByAddedBy(int addedById);
        Task<List<OrganizationEmployeeCount>> GetEmployeeCountsByOrganizationAsync();
        Task<bool> ApproveOutboundAsync(int outBoundId);
        Task<bool> RejectOutboundAsync(int outBoundId);
        Task<List<Outbound>> GetPendingOutboundOrganisationsAsync();
        Task<Employee> GetEmployeeByIdAsync(int employeeId);

        Task<IEnumerable<EmpTransaction>> GetEmployeeTransactionsByOrgIdAsync(EmployeeTransactionFilterDto filter);
        Task<IEnumerable<Outbound>> GetOutboundsByAddedBy(int addedById);
        Task<Organisation> AddOrganisation(Organisation organisation);
        Task<Organisation> GetOrganisationById(int id);
        Task UpdateOrganisation(Organisation organisation);
        int UserIdToOrganisationId(int userId);
        Task AddOutbound(Outbound outbound);
        Task<bool> CanExecuteTransaction(int orgId, int transactionAmount);
        Task AddBeneficiaryTransaction(BeneficiaryTransactionRequestDto requestDto);
        Task<List<Organisation>> GetOrganisationsByBankId(int bankId);
    }
}
