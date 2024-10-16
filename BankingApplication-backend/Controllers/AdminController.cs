using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
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
        private readonly IInboundService _inboundService;
        private readonly IDownloadService _downloadService;

        public AdminController(IAdminService adminService, IBankService bankService, IOrgService organisationService,
            IEmpTransactionService service, IMailService mailService, IEmpService empService, IEmpTransactionService empTransactionService,
            ISalaryService salaryService, IClientTransactionService clientTransactionService,IInboundService inboundService, IDownloadService ownloadService)
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
            _inboundService = inboundService;
            _downloadService = ownloadService;
        }
        //new
        [HttpGet("pending-outbounds")]
        public async Task<IActionResult> GetPendingOutboundOrganisations()
        {
            List<Outbound> pendingOrganisations = await _organisationService.GetPendingOutboundOrganisationsAsync();

            if (pendingOrganisations == null || pendingOrganisations.Count == 0)
            {
                return NotFound("No pending outbound organisations found.");
            }

            return Ok(pendingOrganisations);
        }
        [HttpPost("approve-outbound/{outBoundId}")]
        public async Task<IActionResult> ApproveOutbound(int outBoundId)
        {
            bool result = await _organisationService.ApproveOutboundAsync(outBoundId);

            if (!result)
            {
                return NotFound("Organisation not found.");
            }

            return Ok(new { Message = "Organisation approved successfully." });
        }

        [HttpPost("reject-outbound/{outBoundId}")]
        public async Task<IActionResult> RejectOutbound(int outBoundId)
        {
            bool result = await _organisationService.RejectOutboundAsync(outBoundId);

            if (!result)
            {
                return NotFound("Organisation not found.");
            }

            return Ok(new { Message = "Organisation rejected successfully." });
        }






        [HttpGet("CanExecuteTransaction/{orgId}/{transactionAmount}")]
        public async Task<int> CanExecuteTransaction(int orgId, int transactionAmount)
        {
            var result = await _organisationService.CanExecuteTransaction(orgId, transactionAmount);
            if (!result)
            {
                return 0;
            }
            return 1;
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

        //added email here------------
        [HttpPost("approve-salary-request/{id}")]             //emp id
        public async Task<IActionResult> ApproveSalaryRequest(int id)
        {
            var emp = await _organisationService.GetEmployeeByIdAsync(id);
            var result = await _salaryService.ApproveSalaryRequestAsync(id);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }
            var emailSubject = $"Salary Request Status ";
            var emailBody = $"The Salary request has been rejected by admin .\n\n";
            var mailData = new MailData
            {
                EmailTo = emp.EmployeeEmail,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            _mailService.SendMail(mailData);
            return Ok(new { message = result });
        }

        [HttpPost("reject-salary-request/{id}")]
        public async Task<IActionResult> RejectSalaryRequest(int id)
        {
            var emp = await _organisationService.GetEmployeeByIdAsync(id);
            var result = await _salaryService.RejectSalaryRequestAsync(id);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }
            var emailSubject = $"Salary Request Status ";
            var emailBody = $"The Salary request has been rejected by admin .\n\n";
            var mailData = new MailData
            {
                EmailTo = emp.EmployeeEmail,
                EmailSubject = emailSubject,
                EmailBody = emailBody
            };

            _mailService.SendMail(mailData);
            return Ok(new { message = result });
        }
        [HttpPut("approve-org/{id}")]
        public async Task<IActionResult> ApproveOrganisation(int id)
        {
            // Retrieve the organization by ID
            var organisation = await _organisationService.GetOrganisationById(id);
            if (organisation == null)
            {
                return NotFound();
            }

            // Update the organization status to approved
            organisation.IsApproved = "approved";
            await _organisationService.UpdateOrganisation(organisation);

            // Create a new Inbound object based on the approved organization
            var inbound = new Inbound
            {
                InboundName = organisation.OrganisationName,
                InboundFounderName = organisation.FounderName,
                InboundEmail = organisation.OrganisationEmail,
                IsApproved = "approved",
                AccountId = organisation.AccountId, 
                InboundBankId = organisation.BankId 
            };

           
            await _inboundService.AddInbound(inbound); 

            
            var emailSubject = $"Organisation Request Approved";
            var emailBody = $"The Organisation request has been {organisation.IsApproved} for your Organisation {organisation.OrganisationName}.\n\n";
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
            if (result) return Ok(result);

            return NotFound("Transaction not found or already processed.");
        }

        [HttpPost("reject-transaction/{transactionId}")]
        public async Task<IActionResult> RejectTransaction(int transactionId)
        {
            var result = await _clientTransactionService.RejectBeneficiaryTransaction(transactionId);
            if (result)
                return Ok(result);

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

        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport()
        {
            var transactionCounts = await _clientTransactionService.GetTransactionStatusCountsAsync();
            var employeeTransactionCounts = await _empTransactionService.GetEmployeeTransactionStatusCountsAsync();

            var organizationCountsByBank = await _bankService.GetOrganizationCountsByBankAsync();
            var employeeCountsByOrganization = await _organisationService.GetEmployeeCountsByOrganizationAsync();

            var report = new 
            {
                BeneficiaryTransactions = new
                {
                    Pending = transactionCounts.FirstOrDefault(x => x.Status == "pending")?.Count ?? 0,
                    Approved = transactionCounts.FirstOrDefault(x => x.Status == "approved")?.Count ?? 0,
                    Rejected = transactionCounts.FirstOrDefault(x => x.Status == "rejected")?.Count ?? 0,
                },
                EmployeeTransactions = new
                {
                    Approved = employeeTransactionCounts.FirstOrDefault(x => x.Status == "approved")?.Count ?? 0,
                    Rejected = employeeTransactionCounts.FirstOrDefault(x => x.Status == "rejected")?.Count ?? 0,
                },
                OrganizationsByBank = organizationCountsByBank,
                EmployeesByOrganization = employeeCountsByOrganization
            };

            return Ok(report);
        }
        [HttpGet("download-report")]
        public async Task<IActionResult> DownloadReport()
        {
            var reportDataResult = await GetReport() as OkObjectResult;

            if (reportDataResult == null || reportDataResult.Value == null)
            {
                return NotFound();
            }

            var reportData = reportDataResult.Value;

            // Create CSV content from the report data
            var csvContent = GenerateCsv(reportData);

            // Return the CSV file
            var fileName = "report.csv";
            return File(Encoding.UTF8.GetBytes(csvContent), "text/csv", fileName);
        }

        private string GenerateCsv(object reportData)
        {
            var sb = new StringBuilder();

            // Header
            sb.AppendLine("Category,Pending,Approved,Rejected");

            // Beneficiary Transactions
            sb.AppendLine($"Beneficiary Transactions,{((dynamic)reportData).BeneficiaryTransactions.Pending},{((dynamic)reportData).BeneficiaryTransactions.Approved},{((dynamic)reportData).BeneficiaryTransactions.Rejected}");

            // Employee Transactions
            sb.AppendLine($"Employee Transactions,{((dynamic)reportData).EmployeeTransactions.Pending},{((dynamic)reportData).EmployeeTransactions.Approved},{((dynamic)reportData).EmployeeTransactions.Rejected}");

            // Organizations by Bank
            sb.AppendLine("\nOrganizations by Bank");
            sb.AppendLine("Bank Name,Organization Count");

            foreach (var org in ((dynamic)reportData).OrganizationsByBank)
            {
                sb.AppendLine($"{org.BankName},{org.OrganizationCount}");
            }

            // Employees by Organization
            sb.AppendLine("\nEmployees by Organization");
            sb.AppendLine("Organization Name,Employee Count");

            foreach (var emp in ((dynamic)reportData).EmployeesByOrganization)
            {
                sb.AppendLine($"{emp.OrganizationName},{emp.EmployeeCount}");
            }

            return sb.ToString();
        }


        [HttpPost("approve-outbound-array")]
        public async Task<IActionResult> ApproveOutbounds([FromBody] int[] outBoundIds)
        {
            if (outBoundIds == null || outBoundIds.Length == 0)
            {
                return BadRequest("No outbound IDs provided.");
            }

            var approvedOutbounds = new List<int>();

            foreach (var outBoundId in outBoundIds)
            {
                bool result = await _organisationService.ApproveOutboundAsync(outBoundId);
                if (result)
                {
                    approvedOutbounds.Add(outBoundId);
                }
                else
                {
                    return NotFound($"Organisation with ID {outBoundId} not found.");
                }
            }

            return Ok(new { Message = "Organisations approved successfully.", ApprovedIds = approvedOutbounds });
        }

        [HttpPost("reject-outbound-array")]
        public async Task<IActionResult> RejectOutbounds([FromBody] int[] outBoundIds)
        {
            if (outBoundIds == null || outBoundIds.Length == 0)
            {
                return BadRequest("No outbound IDs provided.");
            }

            var rejectedOutbounds = new List<int>();

            foreach (var outBoundId in outBoundIds)
            {
                bool result = await _organisationService.RejectOutboundAsync(outBoundId);
                if (result)
                {
                    rejectedOutbounds.Add(outBoundId);
                }
                else
                {
                    return NotFound($"Organisation with ID {outBoundId} not found.");
                }
            }

            return Ok(new { Message = "Organisations rejected successfully.", RejectedIds = rejectedOutbounds });
        }

        [HttpPut("reject-bank-array")]
        public async Task<IActionResult> RejectBanks([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No bank IDs provided.");
            }

            var rejectedBanks = new List<Bank>();
            var emailDataList = new List<MailData>();

            foreach (var id in ids)
            {
                // Retrieve the bank by ID
                var bank = await _bankService.GetBankById(id);
                if (bank == null)
                {
                    return NotFound($"Bank with ID {id} not found.");
                }

                // Update the bank status to rejected
                bank.IsApproved = "rejected";
                await _bankService.UpdateBank(bank);
                rejectedBanks.Add(bank);

                // Prepare email notification data
                var emailSubject = $"Bank Request ";
                var emailBody = $"The bank request has been {bank.IsApproved} for your bank {bank.BankName}.\n\n";

                emailDataList.Add(new MailData
                {
                    EmailTo = bank.BankEmail,
                    EmailSubject = emailSubject,
                    EmailBody = emailBody
                });
            }

            foreach (var mailData in emailDataList)
            {
                _mailService.SendMail(mailData);
            }

            return NoContent(); 
        }

        [HttpPut("approve-bank-array")]
        public async Task<IActionResult> ApproveBanks([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No bank IDs provided.");
            }

            var approvedBanks = new List<Bank>();
            var emailDataList = new List<MailData>();

            foreach (var id in ids)
            {
                var bank = await _bankService.GetBankById(id);
                if (bank == null)
                {
                    return NotFound($"Bank with ID {id} not found.");
                }

                bank.IsApproved = "approved";
                await _bankService.UpdateBank(bank);
                approvedBanks.Add(bank);

                var emailSubject = $"Bank Request ";
                var emailBody = $"The bank request has been {bank.IsApproved} for your bank {bank.BankName}.\n\n";

                emailDataList.Add(new MailData
                {
                    EmailTo = bank.BankEmail,
                    EmailSubject = emailSubject,
                    EmailBody = emailBody
                });
            }

            foreach (var mailData in emailDataList)
            {
                _mailService.SendMail(mailData);
            }

            return NoContent(); 
        }

        [HttpPut("approve-org-array")]
        public async Task<IActionResult> ApproveOrganisations([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No organization IDs provided.");
            }

            var approvedOrganisations = new List<Organisation>();
            var emailDataList = new List<MailData>();

            foreach (var id in ids)
            {
                var organisation = await _organisationService.GetOrganisationById(id);
                if (organisation == null)
                {
                    return NotFound($"Organisation with ID {id} not found.");
                }

                organisation.IsApproved = "approved";
                await _organisationService.UpdateOrganisation(organisation);
                approvedOrganisations.Add(organisation);

                var inbound = new Inbound
                {
                    InboundName = organisation.OrganisationName,
                    InboundFounderName = organisation.FounderName,
                    InboundEmail = organisation.OrganisationEmail,
                    IsApproved = "approved",
                    AccountId = organisation.AccountId, 
                    InboundBankId = organisation.BankId 
                };

                await _inboundService.AddInbound(inbound);

                var emailSubject = $"Organisation Request Approved";
                var emailBody = $"The Organisation request has been {organisation.IsApproved} for your Organisation {organisation.OrganisationName}.\n\n";

                emailDataList.Add(new MailData
                {
                    EmailTo = organisation.OrganisationEmail,
                    EmailSubject = emailSubject,
                    EmailBody = emailBody
                });
            }

            foreach (var mailData in emailDataList)
            {
                _mailService.SendMail(mailData);
            }

            return NoContent(); 
        }
        [HttpPut("reject-org-array")]
        public async Task<IActionResult> RejectOrganisations([FromBody] int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No organization IDs provided.");
            }

            var rejectedOrganisations = new List<Organisation>();
            var emailDataList = new List<MailData>();

            foreach (var id in ids)
            {
                var organisation = await _organisationService.GetOrganisationById(id);
                if (organisation == null)
                {
                    return NotFound($"Organisation with ID {id} not found.");
                }

                organisation.IsApproved = "rejected";
                await _organisationService.UpdateOrganisation(organisation);
                rejectedOrganisations.Add(organisation);

                var emailSubject = $"Organisation Request ";
                var emailBody = $"The Organisation request has been {organisation.IsApproved} for your Organisation {organisation.OrganisationName}.\n\n";

                emailDataList.Add(new MailData
                {
                    EmailTo = organisation.OrganisationEmail,
                    EmailSubject = emailSubject,
                    EmailBody = emailBody
                });
            }

            foreach (var mailData in emailDataList)
            {
                _mailService.SendMail(mailData);
            }

            return NoContent();
        }

    }
}

