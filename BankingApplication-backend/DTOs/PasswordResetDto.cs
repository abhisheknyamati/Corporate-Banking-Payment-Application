namespace BankingApplication_backend.DTOs
{
    public class PasswordResetDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

}
