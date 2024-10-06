using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication_backend.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryService(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        public async Task<List<SalaryRequest>> GetSalaryRequestsAsync()
        {
            return await _salaryRepository.GetSalaryRequestsAsync();
        }

        public async Task<string> ApproveSalaryRequestAsync(int id)
        {
            var request = await _salaryRepository.GetSalaryRequestByIdAsync(id);
            if (request == null) return $"Salary request for Employee ID {id} not found.";

            var transaction = await _salaryRepository.GetTransactionByEmployeeAndOrgAsync(id, request.OrgID);
            var requiredOrg = await _salaryRepository.GetOrganisationByIdAsync(request.OrgID);
            if (requiredOrg == null) return $"Organization with ID {request.OrgID} not found.";

            requiredOrg.Account.AccountBalance -= transaction.Amount;
            transaction.IsApproved = "approved";

            _salaryRepository.RemoveSalaryRequest(request);
            await _salaryRepository.SaveChangesAsync();

            return "Salary request approved and transactions updated.";
        }

        public async Task<string> RejectSalaryRequestAsync(int id)
        {
            var request = await _salaryRepository.GetSalaryRequestByIdAsync(id);
            if (request == null) return $"Salary request for Employee ID {id} not found.";

            var transaction = await _salaryRepository.GetTransactionByEmployeeAndOrgAsync(id, request.OrgID);
            transaction.IsApproved = "rejected";

            _salaryRepository.RemoveSalaryRequest(request);
            await _salaryRepository.SaveChangesAsync();

            return "Salary request rejected and transactions updated.";
        }


        public async Task<IActionResult> SendSalaryRequestByUserId(SalaryRequestDto requestDto)
        {
            return await _salaryRepository.SendSalaryRequestByUserId(requestDto);
        }

        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            return await _salaryRepository.AddEmployee(employee);
        }

        public async Task<IActionResult> AddOrgEmployeeByUserId(Employee employee)
        {
            return await _salaryRepository.AddOrgEmployeeByUserId(employee);
        }

        public async Task<IActionResult> SendSalaryRequest(SalaryRequestDto requestDto)
        {
            return await _salaryRepository.SendSalaryRequest(requestDto);
        }
    }
}
