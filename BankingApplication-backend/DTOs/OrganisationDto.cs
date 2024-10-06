using BankingApplication_backend.Models;

namespace BankingApplication_backend.DTOs
{
    public class OrganisationDto
    {
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationRegNumber { get; set; }
        public string FounderName { get; set; }
        public string OrganisationEmail { get; set; }
        public string OrganisationPassword { get; set; }
        public string BankName { get; set; }
        public Account Account { get; set; }


    }
}
