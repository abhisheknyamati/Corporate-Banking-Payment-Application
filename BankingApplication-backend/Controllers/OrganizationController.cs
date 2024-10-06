using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
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
        private readonly IInboundService _inboundService;

        public OrganizationController(ISalaryService salaryService, IOrgService organizationService, IEmpService empService, IInboundService inboundService)
        {
            _salaryService = salaryService;
            _organizationService = organizationService;
            _empService = empService;
            _inboundService = inboundService;
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
            int orgId = _organizationService.UserIdToOrganisationId(addedById);
            var outbounds = await _organizationService.GetOutboundsByAddedBy(orgId); if (outbounds == null || !outbounds.Any())
            {
                return NotFound("No outbounds found for the given user.");
            }
            return Ok(outbounds);
        }


        [HttpPost("user/outbounds")]
        public async Task<IActionResult> AddOutbound([FromBody] OutboundDto outboundDto)
        {
            if (outboundDto == null)
            {
                return BadRequest("Invalid data.");
            }
            int orgId = _organizationService.UserIdToOrganisationId(outboundDto.AddedBy);
            // Map DTO to Outbound model
            var outbound = new Outbound
            {
                OrganisationName = outboundDto.OrganisationName,
                FounderName = outboundDto.FounderName,
                OrganisationEmail = outboundDto.OrganisationEmail,
                IsApproved = "pending",
                AccountNumber = outboundDto.AccountNumber,
                IFSC = outboundDto.IFSC,
                AddedBy = orgId
            };

            await _organizationService.AddOutbound(outbound);

            return CreatedAtAction(nameof(GetOutboundsByAddedBy), new { addedById = outbound.AddedBy }, outbound);
        }

        [HttpGet("CanExecuteTransaction")]
        private async Task<bool> CanExecuteTransaction(int orgId, decimal transactionAmount)
        {
            return await _organizationService.CanExecuteTransaction(orgId, transactionAmount);
        }

        [HttpPost("beneficiary-transaction")]
        public async Task<IActionResult> CreateBeneficiaryTransaction([FromBody] BeneficiaryTransactionRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("Invalid data.");
            }

            // Validate that at least one of InboundId or OutboundId is provided
            if (requestDto.InboundId == null && requestDto.OutboundId == null)
            {
                return BadRequest("Either InboundId or OutboundId must be provided.");
            }

            // Call the service to add the transaction
            await _organizationService.AddBeneficiaryTransaction(requestDto);

            // Respond with a simple success message
            return Ok(new { message = "Request sent successfully." });
        }
        [HttpGet("employee-transactions")]
        public async Task<IActionResult> GetEmployeeTransactions([FromQuery] EmployeeTransactionFilterDto filter)
        {
            var transactions = await _organizationService.GetEmployeeTransactionsByOrgIdAsync(filter);

            if (transactions == null || !transactions.Any())
            {
                return NotFound("No transactions found for the specified criteria.");
            }

            return Ok(transactions);
        }

    }
}
