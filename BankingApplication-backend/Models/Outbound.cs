using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Outbound
    {
        [Key]
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string FounderName { get; set; }
        public string OrganisationEmail { get; set; }
        public string IsApproved { get; set; }
        public int AccountNumber { get; set; }
        public string IFSC { get; set; }
        [ForeignKey("Organisation")]
        public int AddedBy { get; set; }
        [ValidateNever]
        [
            JsonIgnore
        ]
        public BeneficiaryTransaction BeneficiaryTransaction { get; set; }
    }
}
