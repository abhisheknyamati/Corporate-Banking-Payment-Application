namespace BankingApplication_backend.Models
{
    public interface IUser
    {
        int UserId { get; }
        string Name { get; }
        string Email { get; }
    }
}
