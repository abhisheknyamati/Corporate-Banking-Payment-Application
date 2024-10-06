using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication_backend.Services
{
    public interface ISalaryService 
    {
        Task<List<SalaryRequest>> GetSalaryRequestsAsync();
        Task<string> ApproveSalaryRequestAsync(int id);
        Task<string> RejectSalaryRequestAsync(int id);
        Task<IActionResult> SendSalaryRequestByUserId(SalaryRequestDto requestDto);
        Task<IActionResult> SendSalaryRequest(SalaryRequestDto requestDto);
        Task<IActionResult> AddEmployee(Employee employee);
        Task<IActionResult> AddOrgEmployeeByUserId(Employee employee);
    }
}
