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

        public async Task<int> GetTotalCountByOrgId(int orgId)
        {
            return await _transactionRepository.GetTotalCountByOrgId(orgId);
        }

        public async Task<List<EmpTransaction>> GetTransactionsByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            return await _transactionRepository.GetTransactionsByOrgId(orgId, searchTerm, startDate, endDate, pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountByOrgId(int orgId, string searchTerm, DateTime? startDate, DateTime? endDate)
        {
            return await _transactionRepository.GetTotalCountByOrgId(orgId, searchTerm, startDate, endDate);
        }
    }

}


