using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Net;

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
        private readonly Cloudinary _cloudinary;

        public MainController(IAdminService adminService, IBankService bankService, IOrgService orgService, IDocumentService documentService,Cloudinary cloudinary)
        {
            _adminService = adminService;
            _bankService = bankService;
            _orgService = orgService;
            _documentService = documentService;
            _cloudinary = cloudinary;
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
                Account = new Models.Account
                {
                    AccountNumber = organisationDto.Account.AccountNumber,
                    IFSC = organisationDto.Account.IFSC,
                    AccountBalance = organisationDto.Account.AccountBalance
                },
            };

            var createdOrganisation = await _orgService.AddOrganisation(organisation);

            if (file != null && file.Length > 0)
            {
                // Validate file type and size if necessary
                var validExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".gif", ".pdf" };
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!validExtensions.Contains(extension))
                {
                    return BadRequest("Invalid file extension.");
                }

                // Validate file size (max 5MB)
                long size = file.Length;
                if (size > (5 * 1024 * 1024))
                {
                    return BadRequest("File size exceeds the 5MB limit.");
                }

                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = $"organization/{createdOrganisation.OrganisationId}/{file.FileName}", // Specify folder structure
                        Overwrite = true // Optional: Overwrite if exists
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        var document = new Document
                        {
                            FileName = file.FileName,
                            FilePath = uploadResult.SecureUrl.ToString(), // Use the secure URL from Cloudinary
                            FileType = file.ContentType,
                            OrganisationId = createdOrganisation.OrganisationId
                        };

                        await _documentService.AddDocumentAsync(document);
                    }
                    else
                    {
                        return StatusCode((int)uploadResult.StatusCode, "Error uploading file to Cloudinary.");
                    }
                }
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
                var validExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".gif", ".pdf" };
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!validExtensions.Contains(extension))
                {
                    return BadRequest("Invalid file extension.");
                }

                if (file.Length > (5 * 1024 * 1024))
                {
                    return BadRequest("File size exceeds the 5MB limit.");
                }

                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var publicId = $"bank/{createdBank.BankId}/{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}";
                        var uploadParams = extension == ".pdf"
                            ? new RawUploadParams
                            {
                                File = new FileDescription(file.FileName, stream),
                                PublicId = publicId,
                                Overwrite = true
                            }
                            : new ImageUploadParams
                            {
                                File = new FileDescription(file.FileName, stream),
                                PublicId = publicId,
                                Overwrite = true
                            };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        if (uploadResult.StatusCode == HttpStatusCode.OK)
                        {
                            var document = new Document
                            {
                                FileName = file.FileName,
                                FilePath = uploadResult.SecureUrl.ToString(),
                                FileType = file.ContentType,
                                BankId = createdBank.BankId,
                            };

                            await _documentService.AddDocumentAsync(document);
                        }
                        else
                        {
                            return StatusCode((int)uploadResult.StatusCode, "Error uploading file to Cloudinary.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    return StatusCode(500, $"An error occurred while uploading the file: {ex.Message}");
                }
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
        [HttpGet("check-username/{email}")]
        public async Task<IActionResult> CheckUsername(string email)
        {
            var exists = await _adminService.UserExists(email); // Implement this method in your service
            return Ok(new { exists });
        }

    }
}
