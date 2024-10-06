using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication_backend.Models
{
    public class Admin : IUser
    {
        [Key]
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ValidateNever]
        public virtual User User { get; set; }
        string IUser.Name => AdminName;
        string IUser.Email => AdminEmail;
    }
}
