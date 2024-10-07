namespace BankingApplication_backend.Models
{
    public class ReportResponse
    {
        public TransactionStatusCount BeneficiaryTransactions { get; set; }
        public TransactionStatusCount EmployeeTransactions { get; set; }
        public IEnumerable<OrganizationEmployeeCount> OrganizationsByBank { get; set; }
        public IEnumerable<OrganizationEmployeeCount> EmployeesByOrganization { get; set; }
    }
}
