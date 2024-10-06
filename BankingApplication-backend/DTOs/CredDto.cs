using System.ComponentModel.DataAnnotations;

namespace BankingApplication_backend.DTOs
{
    public class CredDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
    }
}
