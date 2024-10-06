using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IBankService _bankService;
        private readonly IOrgService _orgService;
        private readonly IDocumentService _documentService;

        public MainController(IAdminService adminService, IBankService bankService, IOrgService orgService, IDocumentService documentService)
        {
            _adminService = adminService;
            _bankService = bankService;
            _orgService = orgService;
            _documentService = documentService;
        }

        [HttpPost("Admin")]
        public async Task<IActionResult> PostAdmin([FromBody] AdminDto adminDto)
        {
            try
            {
                var admin = await _adminService.CreateAdmin(adminDto);
                return Ok(admin);
            }
            catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
            catch (Exception ex)
            {            
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Organization")]
        public async Task<IActionResult> PostOrganisation([FromForm] OrganisationDto organisationDto, IFormFile file)
        {
            if (organisationDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var organisation = new Organisation
            {
                OrganisationName = organisationDto.OrganisationName,
                OrganisationRegNumber = organisationDto.OrganisationRegNumber,
                FounderName = organisationDto.FounderName,
                OrganisationEmail = organisationDto.OrganisationEmail,
                OrganisationPassword = organisationDto.OrganisationPassword,
                BankName = organisationDto.BankName,
                Account = new Account
                {
                    AccountNumber = organisationDto.Account.AccountNumber,
                    IFSC = organisationDto.Account.IFSC,
                    AccountBalance = organisationDto.Account.AccountBalance
                },
            };

            //var beneficiary = new Beneficiary
            //{
            //    BeneficiaryId = organisationDto.OrganisationId,
            //    BeneficiaryName = organisationDto.OrganisationName,
            //    BeneficiaryEmail = organisationDto.OrganisationEmail,
            //    OrganisationId = null,
            //    Account = new Account
            //    {
            //        AccountNumber = organisationDto.Account.AccountNumber,
            //        IFSC = organisationDto.Account.IFSC,
            //        AccountBalance = organisationDto.Account.AccountBalance
            //    },

            //};

            //await _benificiaryService.AddBeneficiary(beneficiary);

            var createdOrganisation = await _orgService.AddOrganisation(organisation);

            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = new Document
                {
                    FileName = fileName,
                    FilePath = fullPath,
                    FileType = file.ContentType,
                    OrganisationId = createdOrganisation.OrganisationId
                };


                await _documentService.AddDocumentAsync(document);
            }

            return CreatedAtAction(nameof(GetOrganization), new { id = createdOrganisation.OrganisationId }, createdOrganisation);
        }

        [HttpPost("Bank")]
        public async Task<IActionResult> PostBank([FromForm] BankDto bankDto, IFormFile file)
        {
            if (bankDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var bank = new Bank
            {
                BankEmail = bankDto.BankEmail,
                BankName = bankDto.BankName,
                BankPassword = bankDto.BankPassword
            };

            var createdBank = await _bankService.AddBank(bank);
            if (createdBank == null)
            {
                return StatusCode(500, "An error occurred while creating the bank.");
            }

            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = new Document
                {
                    FileName = fileName,
                    FilePath = fullPath,
                    FileType = file.ContentType,
                    BankId = createdBank.BankId,
                };

                if (string.IsNullOrEmpty(document.FileName) || string.IsNullOrEmpty(document.FilePath) || string.IsNullOrEmpty(document.FileType))
                {
                    return BadRequest("Document details are incomplete.");
                }

                await _documentService.AddDocumentAsync(document);
            }

            return CreatedAtAction(nameof(GetBank), new { id = createdBank.BankId }, createdBank);
        }

        [HttpGet("Bank/{id}")]
        public async Task<IActionResult> GetBank(int id)
        {
            var bank = await _bankService.GetBankById(id);
            if (bank == null)
            {
                return NotFound();
            }
            return Ok(bank);
        }

        [HttpGet("Organization/{id}")]
        public async Task<IActionResult> GetOrganization(int id)
        {

            var organization = await _orgService.GetOrganisationById(id);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

    }
}
