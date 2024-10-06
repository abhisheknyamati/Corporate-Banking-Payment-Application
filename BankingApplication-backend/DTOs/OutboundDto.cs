namespace BankingApplication_backend.DTOs
{
    public class OutboundDto
    {
        public string OrganisationName { get; set; }
        public string FounderName { get; set; }
        public string OrganisationEmail { get; set; }
        public string IsApproved { get; set; }
        public int AccountNumber { get; set; }
        public string IFSC { get; set; }
        public int AddedBy { get; set; }
    }
}
