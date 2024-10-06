using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IClientTransactionService _clientTransactionService;
        private readonly IEmpTransactionService _empTransactionService;
        public BankController(IBankService bankService, IClientTransactionService clientTransactionService, IEmpTransactionService empTransactionService)
        {
            _bankService = bankService;
            _clientTransactionService = clientTransactionService;
            _empTransactionService = empTransactionService;
        }


        [HttpGet("ViewBank/{id}")]
        public async Task<IActionResult> ViewBank(int id)
        {


            var bank = await _bankService.GetBankById(id);
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
        // getorg by user id pending
    }
}
