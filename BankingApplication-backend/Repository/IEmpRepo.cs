using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IEmpRepo
    {
        Task<IEnumerable<Employee>> GetEmployeesByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize);
    }
}
