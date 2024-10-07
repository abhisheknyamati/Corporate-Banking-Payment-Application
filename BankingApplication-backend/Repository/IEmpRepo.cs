using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IEmpRepo
    {
        Task UpdateEmployeeAsync(Employee employee);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<Employee>> GetEmployeesByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize);
    }
}
