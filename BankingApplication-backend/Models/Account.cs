using System.ComponentModel.DataAnnotations;

namespace BankingApplication_backend.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public int AccountNumber { get; set; }
        public string IFSC { get; set; }
        public int AccountBalance { get; set; }

    }
}
