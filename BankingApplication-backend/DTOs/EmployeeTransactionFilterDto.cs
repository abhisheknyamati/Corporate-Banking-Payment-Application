namespace BankingApplication_backend.DTOs
{
    public class EmployeeTransactionFilterDto
    {
        public int OrgId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
