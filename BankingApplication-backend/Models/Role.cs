using System.ComponentModel.DataAnnotations;

namespace BankingApplication_backend.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
