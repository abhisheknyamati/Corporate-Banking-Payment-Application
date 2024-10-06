using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Organisation : IUser
    {
        [Key]
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationRegNumber { get; set; }
        public string FounderName { get; set; }
        public string OrganisationEmail { get; set; }
        public string IsApproved { get; set; }
        public bool IsActive { get; set; }
        public string OrganisationPassword { get; set; }
        [ForeignKey("Account")]
        [JsonIgnore]
        public int AccountId { get; set; }
        [ValidateNever]
        //   [JsonIgnore]
        public virtual Account Account { get; set; }

        public string BankName { get; set; }
        [ForeignKey("Bank")]
        [JsonIgnore]
        public int? BankId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual Bank Bank { get; set; }
        [ForeignKey("User")]
        [JsonIgnore]
        public int UserId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual User User { get; set; }
        [ValidateNever]
        public virtual Document Document { get; set; }
        string IUser.Name => OrganisationName;
        string IUser.Email => OrganisationEmail;
    }
}
