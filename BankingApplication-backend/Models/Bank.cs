using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Bank : IUser
    {
        [Key]
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankEmail { get; set; }
        public string BankPassword { get; set; }
        public string IsApproved { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("User")]
        [JsonIgnore]
        public int UserId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual User User { get; set; }
        [ValidateNever]

        public virtual Document Document { get; set; }
        string IUser.Name => BankName;
        string IUser.Email => BankEmail;

    }
}
