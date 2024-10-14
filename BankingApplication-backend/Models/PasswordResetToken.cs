namespace BankingApplication_backend.Models
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; } 
        public virtual User User { get; set; }
    }

}
