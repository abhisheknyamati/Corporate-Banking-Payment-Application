namespace BankingApplication_backend.Models
{
    public class TransactionStatusCount
    {

        public string Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public int Count { get; set; }      // Number of transactions for that status
    }
}
