using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IOrgRepo
    {
        Task<IEnumerable<Outbound>> GetOutboundsByAddedBy(int addedById);
        Task<Organisation> GetOrganisationById(int id);
        Task<List<Organisation>> GetOrganisationsByBankId(int bankId);
        Task<IEnumerable<Organisation>> GetByApprovalStatus(string status);
        Task UpdateOrganisation(Organisation organisation);
        Organisation UserIdToOrganisationId(int userId);
        Task AddOutbound(Outbound outbound);
        Task<Organisation> GetOrganisationWithAccountAsync(int orgId);
        Task<Organisation> AddOrganisation(Organisation organisation);
    }
}
