using BankingApplication_backend.Data;
using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class EmpRepo:IEmpRepo
    {
        private readonly BankingAppDbContext _context; 
        public EmpRepo(BankingAppDbContext context) { _context = context; }

        public async Task<IEnumerable<Employee>> GetEmployeesByOrgId(int orgId, string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Employees.AsQueryable();
            // Filter by organization ID
            query = query.Where(e => e.OrganisationId == orgId);
            // Search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.EmployeeName.Contains(searchTerm));
                // Assuming Employee has a Name property
            }
            // Fetch employees with pagination
            var employees = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
