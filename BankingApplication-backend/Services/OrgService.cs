using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Services
{
    public class OrgService : IOrgService
    {
        public readonly IOrgRepo _orgRepo;
        public OrgService(IOrgRepo orgRepo)
        {
            _orgRepo = orgRepo;
        }
        public async Task<IEnumerable<EmpTransaction>> GetEmployeeTransactionsByOrgIdAsync(EmployeeTransactionFilterDto filter)
        {
            return await _orgRepo.GetEmployeeTransactionsByOrgIdAsync(filter);
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
            Organisation organisation =  _orgRepo.UserIdToOrganisationId(userId);
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
        public Task AddBeneficiaryTransaction(BeneficiaryTransactionRequestDto requestDto)
        {
            int? inboundId = requestDto.InboundId == 0 ? (int?)null : requestDto.InboundId;
            int? outboundId = requestDto.OutboundId == 0 ? (int?)null : requestDto.OutboundId;

            int orgId = UserIdToOrganisationId(requestDto.InitiatorOrgId);

            var transaction = new BeneficiaryTransaction
            {
                InitiatorOrgId = orgId,
                InboundId = inboundId,
                OutboundId = outboundId,
                Amount = requestDto.Amount,
                BeneficiaryTransactionDate = DateTime.Now,
                IsApproved="pending"
            };
            

           return _orgRepo.AddBeneficiaryTransaction(transaction);
        }

    }
}
