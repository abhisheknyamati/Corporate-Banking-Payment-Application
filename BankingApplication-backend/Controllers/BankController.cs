using BankingApplication_backend.Models;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IClientTransactionService _clientTransactionService;
        private readonly IEmpTransactionService _empTransactionService;
        private readonly IOrgService _orgService;
        
        public BankController(IBankService bankService, IClientTransactionService clientTransactionService, IEmpTransactionService empTransactionService, IOrgService orgService)
        {
            _bankService = bankService;
            _clientTransactionService = clientTransactionService;
            _empTransactionService = empTransactionService;
            _orgService = orgService;
        }


        [HttpGet("ViewBank/{userId}")]
        public async Task<IActionResult> ViewBank(int userId)
        {

            int bankid = _bankService.userIdToBankId(userId);
            var bank = await _bankService.GetBankById(bankid);
            if (bank == null)
            {
                return NotFound();
            }
            return Ok(bank);
        }

        [HttpGet("beneficiary-transactions")]
        public async Task<IActionResult> GetBeneficiaryTransactions(int organizationId, string status)
        {
            if (organizationId <= 0 || string.IsNullOrEmpty(status))
            {
                return BadRequest("Invalid organization ID or status.");
            }
            var transactions = await _clientTransactionService.GetBeneficiaryTransactions(organizationId, status); if (transactions == null || !transactions.Any())
            {
                return NotFound("No beneficiary transactions found.");
            }
            return Ok(transactions);
        }
        [HttpGet("salary-disbursement")]
        public async Task<IActionResult> GetEmployeeSalaryDisbursement(int organizationId, string status)
        {
            if (organizationId <= 0 || string.IsNullOrEmpty(status))
            {
                return BadRequest("Invalid organization ID or status.");
            }
            var salaryDisbursements = await _empTransactionService.GetEmployeeSalaryDisbursements(organizationId, status); if (salaryDisbursements == null || !salaryDisbursements.Any())
            {
                return NotFound("No salary disbursement records found.");
            }
            return Ok(salaryDisbursements);
        }
        [HttpGet("organisationsbyUserId/{userId}")]
        public async Task<IActionResult> GetOrganisationsByBankId(int userId)
        {
            int bankId = _bankService.userIdToBankId(userId);
            var organisations = await _orgService.GetOrganisationsByBankId(bankId);

            if (organisations == null || !organisations.Any())
            {
                return NotFound($"No organizations found for Bank ID {bankId}.");
            }

            return Ok(organisations);
        }
    }
}
