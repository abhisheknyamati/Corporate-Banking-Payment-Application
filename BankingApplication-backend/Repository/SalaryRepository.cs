using BankingApplication_backend.Data;
using BankingApplication_backend.DTOs;
using BankingApplication_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Repository
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly BankingAppDbContext _context;

        public SalaryRepository(BankingAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalaryRequest>> GetSalaryRequestsAsync()
        {
            return await _context.salaryRequests.Include(o=>o.Organisation).ToListAsync();
        }

        public async Task<SalaryRequest> GetSalaryRequestByIdAsync(int id)
        {
            return await _context.salaryRequests.FirstOrDefaultAsync(r => r.EmployeeIds == id);
        }

        public async Task<EmpTransaction> GetTransactionByEmployeeAndOrgAsync(int employeeId, int orgId)
        {
            return await _context.EmpTransactions.FirstOrDefaultAsync(t => t.EmployeeId == employeeId && t.OrgID == orgId);
        }

        public async Task<Organisation> GetOrganisationByIdAsync(int orgId)
        {
            return await _context.Organisations
                .Include(o => o.Account)
                .FirstOrDefaultAsync(o => o.OrganisationId == orgId);
        }

        public void RemoveSalaryRequest(SalaryRequest request)
        {
            _context.salaryRequests.Remove(request);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> SendSalaryRequestByUserId(SalaryRequestDto requestDto)
        {
            Organisation org = await _context.Organisations.FirstOrDefaultAsync(o => o.UserId == requestDto.OrgID);

            if (requestDto.EmployeeIds == null || !requestDto.EmployeeIds.Any())
            {
                return new BadRequestObjectResult("Employee IDs cannot be empty.");
            }

            foreach (var empId in requestDto.EmployeeIds)
            {
                var salaryRequest = new SalaryRequest
                {
                    OrgID = org.OrganisationId,
                    EmployeeIds = empId,
                    Status = "Pending",
                };

                _context.salaryRequests.Add(salaryRequest);
            }

            foreach (var empId in requestDto.EmployeeIds)
            {
                var employee = await _context.Employees.FindAsync(empId);
                if (employee != null)
                {
                    var transaction = new EmpTransaction
                    {
                        EmployeeName = employee.EmployeeName,
                        EmployeeEmail = employee.EmployeeEmail,
                        IsApproved = "Pending",
                        AccountNumber = employee.AccountNumber,
                        IFSC = employee.IFSC,
                        Amount = employee.EmployeeSalary,
                        OrgID = org.OrganisationId,
                        EmployeeId = empId
                    };

                    _context.EmpTransactions.Add(transaction);
                }
                else
                {
                    return new NotFoundObjectResult($"Employee with ID {empId} not found.");
                }
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { message = "Salary request submitted successfully." });
        }

        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (employee == null) return new BadRequestObjectResult("Invalid employee data.");

            var organisation = await _context.Organisations.FindAsync(employee.OrganisationId);
            if (organisation == null) return new NotFoundObjectResult("Organisation not found.");
            employee.IsActive = true;

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Employee added successfully.");
        }

        public async Task<IActionResult> AddOrgEmployeeByUserId(Employee employee)
        {
            var userId = employee.OrganisationId;
            Organisation organisation = await _context.Organisations.FirstOrDefaultAsync(x => x.UserId == userId);

            if (organisation == null)
            {
                return new NotFoundObjectResult("Organization not found for this user.");
            }

            var newEmployee = new Employee
            {
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.EmployeeEmail,
                EmployeeSalary = employee.EmployeeSalary,
                OrganisationId = organisation.OrganisationId,
                AccountNumber = employee.AccountNumber,
                IFSC = employee.IFSC,
                IsActive = true,
            };

            try
            {
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> SendSalaryRequest(SalaryRequestDto requestDto)
        {
            Organisation org = await _context.Organisations.FirstOrDefaultAsync(o => o.OrganisationId == requestDto.OrgID);

            if (requestDto.EmployeeIds == null || !requestDto.EmployeeIds.Any())
            {
                return new BadRequestObjectResult("Employee IDs cannot be empty.");
            }

            foreach (var empId in requestDto.EmployeeIds)
            {
                var salaryRequest = new SalaryRequest
                {
                    OrgID = org.OrganisationId,
                    EmployeeIds = empId,
                    Status = "Pending",
                };

                _context.salaryRequests.Add(salaryRequest);
            }

            foreach (var empId in requestDto.EmployeeIds)
            {
                var employee = await _context.Employees.FindAsync(empId);
                if (employee != null)
                {
                    var transaction = new EmpTransaction
                    {
                        EmployeeName = employee.EmployeeName,
                        EmployeeEmail = employee.EmployeeEmail,
                        IsApproved = "Pending",
                        AccountNumber = employee.AccountNumber,
                        IFSC = employee.IFSC,
                        Amount = employee.EmployeeSalary,
                        OrgID = org.OrganisationId,
                        EmployeeId = empId,
                        EmployeeTransactionDate = DateTime.Now
                        
                    };

                    _context.EmpTransactions.Add(transaction);
                }
                else
                {
                    return new NotFoundObjectResult($"Employee with ID {empId} not found.");
                }
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(new { message = "Salary request submitted successfully." });
        }
    }
}
