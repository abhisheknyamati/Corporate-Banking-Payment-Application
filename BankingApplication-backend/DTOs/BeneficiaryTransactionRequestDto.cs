namespace BankingApplication_backend.DTOs
{
    public class BeneficiaryTransactionRequestDto
    {
        public int InitiatorOrgId { get; set; }
        public int? InboundId { get; set; }
        public int? OutboundId { get; set; }
        public int Amount { get; set; }
    }
}
