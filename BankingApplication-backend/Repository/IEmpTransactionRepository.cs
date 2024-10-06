using BankingApplication_backend.Models;

namespace BankingApplication_backend.Repository
{
    public interface IEmpTransactionRepository
    {
        Task<IEnumerable<EmpTransaction>> GetEmployeeSalaryDisbursements(int organizationId, string status);
    }
}
