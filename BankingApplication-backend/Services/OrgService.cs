using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class OrgService : IOrgService
    {
        public readonly IOrgRepo _orgRepo;
        public OrgService(IOrgRepo orgRepo)
        {
            _orgRepo = orgRepo;
        }
        public async Task<Organisation> AddOrganisation(Organisation organisation)
        {
            return await _orgRepo.AddOrganisation(organisation);
        }

        public async Task<List<Organisation>> GetOrganisationsByBankId(int bankId)
        {
            return await _orgRepo.GetOrganisationsByBankId(bankId);
        }

        public async Task<Organisation> GetOrganisationById(int id)
        {
            return await _orgRepo.GetOrganisationById(id);
        }
        public async Task UpdateOrganisation(Organisation organisation)
        {
            await _orgRepo.UpdateOrganisation(organisation);
        }

        public async Task<IEnumerable<Outbound>> GetOutboundsByAddedBy(int addedById)
        {
            return await _orgRepo.GetOutboundsByAddedBy(addedById);
        }

        public int UserIdToOrganisationId(int userId)
        {
            var organisation =  _orgRepo.UserIdToOrganisationId(userId);
            return organisation.OrganisationId;
        }

        public async Task AddOutbound(Outbound outbound)
        {
            await _orgRepo.AddOutbound(outbound);
        }

        public async Task<bool> CanExecuteTransaction(int orgId, decimal transactionAmount)
        {
            var organisation = await _orgRepo.GetOrganisationWithAccountAsync(orgId);
            if (organisation == null || organisation.Account == null) { return false; }
            return (organisation.Account.AccountBalance - transactionAmount) >= 100000;
        }

    }
}
