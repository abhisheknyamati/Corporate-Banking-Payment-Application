using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IEmpTransactionRepository
    {
        Task<List<EmpTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
        Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate);
        Task<int> GetTotalCountByOrgId(int orgId);
        Task<List<TransactionStatusCount>> GetEmployeeTransactionStatusCountsAsync();
        Task<IEnumerable<EmpTransaction>> GetEmployeeSalaryDisbursements(int organizationId, string status);
    }
}
