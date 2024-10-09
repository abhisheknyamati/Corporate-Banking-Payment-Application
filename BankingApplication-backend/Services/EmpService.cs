using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Services
{
    public class EmpService : IEmpService
    {

        private readonly IEmpRepo _empRepo;
        private readonly BankingAppDbContext _context;
        public EmpService(IEmpRepo empRepo, BankingAppDbContext context)
        {
            _empRepo = empRepo;
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize)
        {
            var employees = await _empRepo.GetEmployeesByOrgId(orgId, searchTerm, pageNumber, pageSize);
            return employees;
        }

        public async Task<bool> SoftDeleteEmployeeAsync(int id)
        {
            var employee = await _empRepo.GetEmployeeByIdAsync(id);
            if (employee == null) return false;

            employee.IsActive = false;
            await _empRepo.UpdateEmployeeAsync(employee);
            return true;
        }

        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm)
        {
            var query = _context.Employees.AsQueryable();

            // Filter by organization ID
            query = query.Where(e => e.OrganisationId == orgId);

            // Search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.EmployeeName.Contains(searchTerm));
            }

            // Get total count
            return await query.CountAsync();
        }
    }
}
