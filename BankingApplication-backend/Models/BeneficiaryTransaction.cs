using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace BankingApplication_backend.Models
{
    public class BeneficiaryTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("Organisation")]
        public int InitiatorOrgId { get; set; }

        [ForeignKey("Inbound")]
        public int? InboundId { get; set; }
        [ValidateNever]
        public virtual Inbound Inbound { get; set; }
        public int Amount { get; set; }
        [ForeignKey("Outbound")]
        public int? OutboundId { get; set; }
        [ValidateNever]
        public virtual Outbound Outbound { get; set; }
        public string IsApproved { get; set; }
        public DateTime BeneficiaryTransactionDate { get; set; }
    }
}
