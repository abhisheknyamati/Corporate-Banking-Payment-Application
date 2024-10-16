namespace BankingApplication_backend.DTOs
{
    public class SalaryRequestDto
    {
        public int OrgID { get; set; }
        public List<int> EmployeeIds { get; set; }

    }
}
