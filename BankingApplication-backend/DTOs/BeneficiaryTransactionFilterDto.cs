namespace BankingApplication_backend.DTOs
{
    public class BeneficiaryTransactionFilterDto
    {
        public int OrgId { get; set; }
        // public string OutboundName { get; set; } // Optional: For filtering by outbound name
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}


//[HttpGet("beneficiary-transactions")]
//public async Task<IActionResult> GetBeneficiaryTransactions([FromQuery] BeneficiaryTransactionFilterDto filter)
//{
//    int orgId = _organizationService.UserIdToOrganisationId(filter.OrgId);
//    filter.OrgId = orgId;
//    var transactions = await _organizationService.GetBeneficiaryTransactionsAsync(filter);
//    if (transactions == null || !transactions.Any())
//    {
//        return NotFound("No transactions found for the specified criteria.");
//    }

//    return Ok(transactions);
