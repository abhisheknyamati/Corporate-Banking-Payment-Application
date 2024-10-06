using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication_backend.Repository
{
    public interface ISalaryRepository
    {
        Task<List<SalaryRequest>> GetSalaryRequestsAsync();
        Task<SalaryRequest> GetSalaryRequestByIdAsync(int id);
        Task<EmpTransaction> GetTransactionByEmployeeAndOrgAsync(int employeeId, int orgId);
        Task<Organisation> GetOrganisationByIdAsync(int orgId);
        Task SaveChangesAsync();
        void RemoveSalaryRequest(SalaryRequest request);
        Task<IActionResult> SendSalaryRequestByUserId(SalaryRequestDto requestDto);
        Task<IActionResult >AddEmployee(Employee employee);
        Task<IActionResult> AddOrgEmployeeByUserId(Employee employee);
        Task<IActionResult> SendSalaryRequest(SalaryRequestDto requestDto);
    }
}
