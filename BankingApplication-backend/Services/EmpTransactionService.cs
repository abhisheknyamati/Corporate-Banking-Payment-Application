using BankingApplication_backend.Models;
using BankingApplication_backend.Repository;

namespace BankingApplication_backend.Services
{
    public class EmpTransactionService : IEmpTransactionService
    {
        private readonly IEmpTransactionRepository _transactionRepository;
        public EmpTransactionService(IEmpTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionStatusCount>> GetEmployeeTransactionStatusCountsAsync()
        {
            return await _transactionRepository.GetEmployeeTransactionStatusCountsAsync();
        }



        public async Task<IEnumerable<EmpTransaction>> GetEmployeeSalaryDisbursements(int organizationId, string status)
        {
            return await _transactionRepository.GetEmployeeSalaryDisbursements(organizationId, status);
        }
    }
}
