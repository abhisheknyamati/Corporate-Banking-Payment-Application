using BankingApplication_backend.Models;

namespace BankingApplication_backend.Services
{
    public interface IEmpService
    {
        Task<int> GetTotalCountByOrgId(int orgId, string searchTerm);
        Task<bool> SoftDeleteEmployeeAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeesByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize);
    }
}
