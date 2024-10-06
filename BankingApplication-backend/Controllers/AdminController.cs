using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IBankService _bankService;
        private readonly IOrgService _organisationService;
        private readonly IEmpTransactionService _service;
        private readonly IMailService _mailService;
        private readonly IEmpService _empService;
        private readonly IEmpTransactionService _empTransactionService;
        private readonly ISalaryService _salaryService;
        private readonly IClientTransactionService _clientTransactionService;
        public AdminController(IAdminService adminService, IBankService bankService, IOrgService organisationService,
            IEmpTransactionService service, IMailService mailService, IEmpService empService, IEmpTransactionService empTransactionService,
            ISalaryService salaryService, IClientTransactionService clientTransactionService)
        {
            _adminService = adminService;
            _bankService = bankService;
            _organisationService = organisationService;
            _service = service;
            _mailService = mailService;
            _empService = empService;
            _empTransactionService = empTransactionService;
            _salaryService = salaryService;
            _clientTransactionService = clientTransactionService;
        }

        [HttpGet("pending-org")]
        public async Task<IActionResult> GetPendingOrganisation()
        {
            var pendingBeneficiaries = await _adminService.GetPendingOrganisation();
            return Ok(pendingBeneficiaries);
        }

        [HttpGet("pending-bank")]
        public async Task<IActionResult> GetPendingBank()
        {
            var pendingBeneficiaries = await _adminService.GetPendingBank();
            return Ok(pendingBeneficiaries);
        }

        [HttpGet("salary-requests")]
        public async Task<IActionResult> GetSalaryRequests()
        {
            var requests = await _salaryService.GetSalaryRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("approve-salary-request/{id}")]
        public async Task<IActionResult> ApproveSalaryRequest(int id)
        {
            var result = await _salaryService.ApproveSalaryRequestAsync(id);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }
            return Ok(new { message = result });
        }

        [HttpPost("reject-salary-request/{id}")]
        public async Task<IActionResult> RejectSalaryRequest(int id)
        {
            var result = await _salaryService.RejectSalaryRequestAsync(id);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }
            return Ok(new { message = result });
        }
        [HttpPut("approve-org/{id}")]
        public async Task<IActionResult> ApproveOrganisation(int id)
        {
            var organisation = await _organisationService.GetOrganisationById(id);
            if (organisation == null)
            {
                return NotFound();
            }

            organisation.IsApproved = "approved";
            await _organisationService.UpdateOrganisation(organisation);
            var emailSubject = $"Orgnaisation Request ";
            var emailBody = $"The Orgnaisation request has been {organisation.IsApproved} for your Orgnaisation {organisation.OrganisationName} .\n\n";
            var mailData = new MailData
            {
                EmailTo = organisation.OrganisationEmail,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            _mailService.SendMail(mailData);
            return NoContent();
        }

        [HttpPut("reject-org/{id}")]
        public async Task<IActionResult> RejectOrganisation(int id)
        {
            var organisation = await _organisationService.GetOrganisationById(id);
            if (organisation == null)
            {
                return NotFound();
            }

            organisation.IsApproved = "rejected";
            await _organisationService.UpdateOrganisation(organisation);
            var emailSubject = $"Orgnaisation Request ";
            var emailBody = $"The Orgnaisation request has been {organisation.IsApproved} for your Orgnaisation {organisation.OrganisationName} .\n\n";
            var mailData = new MailData
            {
                EmailTo = organisation.OrganisationEmail,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            _mailService.SendMail(mailData);
            return NoContent();
        }

        [HttpPut("approve-bank/{id}")]
        public async Task<IActionResult> ApproveBank(int id)
        {
            var bank = await _bankService.GetBankById(id);
            if (bank == null)
            {
                return NotFound();
            }
            bank.IsApproved = "approved";
            await _bankService.UpdateBank(bank);
            var emailSubject = $"Bank Request ";
            var emailBody = $"The bank request has been {bank.IsApproved} for your bank {bank.BankName} .\n\n";
            var mailData = new MailData
            {
                EmailTo = bank.BankEmail,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            _mailService.SendMail(mailData);



            return NoContent();
        }

        [HttpPut("reject-bank/{id}")]
        public async Task<IActionResult> RejectBank(int id)
        {
            var bank = await _bankService.GetBankById(id);
            if (bank == null)
            {
                return NotFound();
            }

            bank.IsApproved = "rejected";
            await _bankService.UpdateBank(bank);
            var emailSubject = $"Bank Request ";
            var emailBody = $"The bank request has been {bank.IsApproved} for your bank {bank.BankName} .\n\n";
            var mailData = new MailData
            {
                EmailTo = bank.BankEmail,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            _mailService.SendMail(mailData);
            return NoContent();
        }

        [HttpGet("pending-transactions")]
        public async Task<IActionResult> GetPendingTransactions()
        {
            var pendingTransactions = await _clientTransactionService.GetPendingBeneficiaryTransactions();
            return Ok(pendingTransactions);
        }

        [HttpPost("approve-transaction/{transactionId}")]
        public async Task<IActionResult> ApproveTransaction(int transactionId)
        {
            var result = await _clientTransactionService.ApproveBeneficiaryTransaction(transactionId);
            if (result) return Ok("Transaction approved successfully.");

            return NotFound("Transaction not found or already processed.");
        }

        [HttpPost("reject-transaction/{transactionId}")]
        public async Task<IActionResult> RejectTransaction(int transactionId)
        {
            var result = await _clientTransactionService.RejectBeneficiaryTransaction(transactionId);
            if (result)
                return Ok("Transaction rejected successfully.");

            return NotFound("Transaction not found or already processed.");
        }

        [HttpGet("ViewOrganisation/{id}")]
        public async Task<IActionResult> ViewOrganisation(int id)
        {
            // dont change this, being used by view in admin

            var organization = await _organisationService.GetOrganisationById(id);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

        [HttpGet("ViewBank/{id}")]
        public async Task<IActionResult> ViewBank(int id)
        {
            // dont change this, being used by view in admin

            var bank = await _bankService.GetBankById(id);
            if (bank == null)
            {
                return NotFound();
            }
            return Ok(bank);
        }

        //[HttpGet("full-report")]
        //public async Task<IActionResult> GetFullReport()
        //{

        //    var totalOrganisations = await _organisationService.GetAllOrganisationsAsync();
        //    var totalBeneficiaries = await _clientTransactionService.GetAllBeneficiariesAsync();
        //    var totalEmployeeTransactions = await _empTransactionService.GetAllSalaryRequestsAsync();
        //    var totalBeneficiaryTransactions = await GetAllBeneficiaryTransactionsAsync();


        //    var pieChartData = new
        //    {
        //        TotalOrganisations = totalOrganisations.Count,
        //        TotalBeneficiaries = totalBeneficiaries.Count,
        //        TotalEmployeeTransactions = totalEmployeeTransactions.Count,
        //        TotalBeneficiaryTransactions = totalBeneficiaryTransactions.Count
        //    };


        //    var employeeCountByOrg = await GetEmployeeCountByOrganisationAsync();
        //    var orgCountByBank = await GetOrganisationCountByBankAsync();


        //    var barChartData = new
        //    {
        //        EmployeeCountByOrg = employeeCountByOrg,
        //        OrgCountByBank = orgCountByBank
        //    };

        //    return Ok(new { PieChartData = pieChartData, BarChartData = barChartData });
        //}
    }
}
