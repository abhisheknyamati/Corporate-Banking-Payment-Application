using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ISalaryService _salaryService;
        private readonly IOrgService _organizationService;
        private readonly IEmpService _empService;

        public OrganizationController(ISalaryService salaryService, IOrgService organizationService, IEmpService empService)
        {
            _salaryService = salaryService;
            _organizationService = organizationService;
            _empService = empService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganization(int id)
        {
            var x = _organizationService.UserIdToOrganisationId(id);
            var organization = await _organizationService.GetOrganisationById(x);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

        [HttpPost("send-salary-request-userId")]
        public async Task<IActionResult> SendSalaryRequestByUserId([FromBody] SalaryRequestDto requestDto)
        {   // this is being converted at last
            var result = await _salaryService.SendSalaryRequestByUserId(requestDto);
            return result;
        }

        [HttpPost("add-employee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            var result = await _salaryService.AddEmployee(employee);
            return result;
        }

        [HttpPost("addOrgEmployee")]
        public async Task<IActionResult> AddOrgEmployeeByUserId([FromBody] Employee employee)
        {
            var result = await _salaryService.AddOrgEmployeeByUserId(employee);
            return result;
        }

        [HttpPost("send-salary-request")]
        public async Task<IActionResult> SendSalaryRequest([FromBody] SalaryRequestDto requestDto)
        {
            var result = await _salaryService.SendSalaryRequest(requestDto);
            return result;
        }


        [HttpGet("user/Oemp/{userId}")]
        public async Task<IActionResult> GetOrganizationEmployeesByUserId(int userId, string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var x = _organizationService.UserIdToOrganisationId(userId);

            try
            {
                var employees = await _empService.GetEmployeesByOrgId(x, searchTerm, pageNumber, pageSize);
                return Ok(employees);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("user/outbounds/{addedById}")]
        public async Task<IActionResult> GetOutboundsByAddedBy(int addedById)
        {
            var outbounds = await _organizationService.GetOutboundsByAddedBy(addedById); if (outbounds == null || !outbounds.Any())
            {
                return NotFound("No outbounds found for the given user.");
            }
            return Ok(outbounds);
        }


        [HttpPost("user/outbounds")]
        public async Task<IActionResult> AddOutbound([FromBody] Outbound outbound)
        {
            if (outbound == null)
            {
                return BadRequest("Invalid data.");
            }
            await _organizationService.AddOutbound(outbound);
            return CreatedAtAction(nameof(GetOutboundsByAddedBy), new { addedById = outbound.AddedBy }, outbound);
        }

        [HttpGet("CanExecuteTransaction")]
        private async Task<bool> CanExecuteTransaction(int orgId, decimal transactionAmount)
        {
            return await _organizationService.CanExecuteTransaction(orgId, transactionAmount);
        }

    }
}
