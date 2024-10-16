using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using BankingApplication_backend.Services;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "org")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ISalaryService _salaryService;
        private readonly IOrgService _organizationService;
        private readonly IEmpService _empService;
        private readonly IInboundService _inboundService;
        private readonly Cloudinary _cloudinary;
        private readonly IDocumentService _documentService;
        private readonly IEmpTransactionService _empTransactionService;
        private readonly IClientTransactionService _clientTransactionService;

        public OrganizationController(ISalaryService salaryService, IOrgService organizationService, IEmpService empService, IInboundService inboundService, Cloudinary cloudinary, IDocumentService documentService, IEmpTransactionService empTransactionService, IClientTransactionService clientTransactionService)
        {
            _salaryService = salaryService;
            _organizationService = organizationService;
            _empService = empService;
            _inboundService = inboundService;
            _cloudinary = cloudinary;
            _documentService = documentService;
            _empTransactionService = empTransactionService;
            _clientTransactionService = clientTransactionService;
        }
        [HttpPut("Organization/{id}/UpdateDocument")]
        public async Task<IActionResult> UpdateDocument(int id, IFormFile file)
        {
            int orgId = _organizationService.UserIdToOrganisationId(id);
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Fetch the existing organization
            var existingOrganisation = await _organizationService.GetOrganisationById(orgId);
            if (existingOrganisation == null)
            {
                return NotFound("Organisation not found.");
            }

            // Validate file type and size
            var validExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".gif", ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!validExtensions.Contains(extension))
            {
                return BadRequest("Invalid file extension.");
            }

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
                    PublicId = $"organization/{existingOrganisation.OrganisationId}/{file.FileName}", // Specify folder structure
                    Overwrite = true // Optional: Overwrite if exists
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                    // Create or update document record
                    var document = new Document
                    {
                        FileName = file.FileName,
                        FilePath = uploadResult.SecureUrl.ToString(), // Use the secure URL from Cloudinary
                        FileType = file.ContentType,
                        OrganisationId = existingOrganisation.OrganisationId
                    };

                    await _documentService.UpdateOrAddDocumentAsync(document);
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, "Error uploading file to Cloudinary.");
                }
            }

            return NoContent(); // 204 No Content
        }

        [HttpPut("Organization/{id}/UpdateBalance/{newBalance}")]
        public async Task<IActionResult> UpdateBalance(int id, int newBalance)
        {
            int orgId = _organizationService.UserIdToOrganisationId(id);
            if (newBalance <= 0)
            {
                return BadRequest("Invalid balance data.");
            }

            // Fetch the existing organization
            var existingOrganisation = await _organizationService.GetOrganisationById(orgId);
            if (existingOrganisation == null)
            {
                return NotFound("Organisation not found.");
            }

            // Update Account Balance
            existingOrganisation.Account.AccountBalance += newBalance;

            // Save changes to the organization
            await _organizationService.UpdateOrganisation(existingOrganisation);

            return NoContent(); // 204 No Content
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


        [HttpGet("user/activeOutbounds/{addedById}")]
        public async Task<IActionResult> GetActiveOutboundsByAddedBy(int addedById)
        {
            int orgId = _organizationService.UserIdToOrganisationId(addedById);
            var outbounds = await _organizationService.GetActiveOutboundsByAddedBy(orgId); if (outbounds == null || !outbounds.Any())
            {
                return NotFound("No outbounds found for the given user.");
            }
            return Ok(outbounds);
        }

        [HttpDelete("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteEmployee(int id)
        {
            var result = await _empService.SoftDeleteEmployeeAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(new { message = "Employee marked as inactive" });
        }

        [HttpGet("GetInboundCompaniesExcludingCurrent/{id}")]
        public async Task<ActionResult<List<Inbound>>> GetInboundCompanies(int id)
        {
            var currentInbound = await _organizationService.GetInboundsExcludingCurrentAsync(id);
            if (currentInbound == null || !currentInbound.Any())
            {
                return NotFound();
            }

            return Ok(currentInbound);
        }

        [HttpGet("GetInboundCompaniesExcludingCurrentByUserId/{id}")]
        public async Task<ActionResult<List<Inbound>>> GetInboundCompaniesByUserId(int id)
        {
            var orgId = _organizationService.UserIdToOrganisationId(id);
            var currentInbound = await _organizationService.GetInboundsExcludingCurrentAsync(orgId);
            if (currentInbound == null || !currentInbound.Any())
            {
                return NotFound();
            }

            return Ok(currentInbound);
        }


        [HttpGet("user/Oemp/{userId}")]
        public async Task<IActionResult> GetOrganizationEmployeesByUserId(int userId, string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            var orgId = _organizationService.UserIdToOrganisationId(userId);

            try
            {
                var employees = await _empService.GetEmployeesByOrgId(orgId, searchTerm, pageNumber, pageSize);

                // Get total count of employees
                var totalCount = await _empService.GetTotalCountByOrgId(orgId, searchTerm);

                // Return an anonymous object
                return Ok(new
                {
                    values = employees,
                    totalCount = totalCount
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet("employee-transactions/{userId}")]
        public async Task<IActionResult> GetEmpTransactionsByOrgId(int userId, string searchTerm = "", DateTime? startDate = null, DateTime? endDate = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var orgId = _organizationService.UserIdToOrganisationId(userId);

                // Fetch transactions based on OrgId and date range
                var transactions = await _empTransactionService.GetTransactionsByOrgId(orgId, searchTerm, startDate, endDate, pageNumber, pageSize);

                // Get total count of transactions
                var totalCount = await _empTransactionService.GetTotalCountByOrgId(orgId, searchTerm, startDate, endDate);

                // Return an anonymous object with values and total count
                return Ok(new
                {
                    values = transactions,
                    totalCount = totalCount
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpGet("beneficiary-transactions/{userId}")]
        public async Task<IActionResult> GetBeneficiaryTransactionsByOrgId(int userId, string searchTerm = "", DateTime? startDate = null, DateTime? endDate = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {

                var orgId = _organizationService.UserIdToOrganisationId(userId);
                // Fetch transactions based on OrgId and date range
                var transactions = await _clientTransactionService.GetTransactionsByOrgId(orgId, searchTerm, startDate, endDate, pageNumber, pageSize);

                // Get total count of transactions
                var totalCount = await _clientTransactionService.GetTotalCountByOrgId(orgId, searchTerm, startDate, endDate);

                // Return an anonymous object with values and total count
                return Ok(new
                {
                    values = transactions,
                    totalCount = totalCount
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPut("reapply/{userId}")]
        public async Task<IActionResult> SetApprovalPending(int userId)
        {
            try
            {
                var organisationId = _organizationService.UserIdToOrganisationId(userId);

                await _organizationService.SetOrganisationApprovalPendingAsync(organisationId);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // 500 Internal Server Error
            }
        }
    }

}

