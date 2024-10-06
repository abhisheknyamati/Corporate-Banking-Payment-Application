using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Inbound
    {
        [Key]
        public int InboundId { get; set; }
        public string InboundName { get; set; }
        public string InboundFounderName { get; set; }
        public string InboundEmail { get; set; }
        public string IsApproved { get; set; }
        [ForeignKey("Account")]
        [JsonIgnore]
        public int AccountId { get; set; }
        [ValidateNever]
        public virtual Account Account { get; set; }
        [ForeignKey("Bank")]
        [JsonIgnore]
        public int? InboundBankId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual Bank Bank { get; set; }
        [ValidateNever]
        public BeneficiaryTransaction BeneficiaryTransaction { get; set; }



    }
}
