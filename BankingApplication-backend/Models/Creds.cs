using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Creds
    {
        [Key]
        public int CredId {  get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ForeignKey("User")]
        public int UserId {  get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
