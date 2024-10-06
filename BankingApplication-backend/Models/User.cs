using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
