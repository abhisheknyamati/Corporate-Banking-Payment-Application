using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication_backend.Models
{
    public class EmpTransaction
    {
        [Key]
        public int EmpTransactionId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string IsApproved { get; set; }
        public int AccountNumber { get; set; }
        public string IFSC { get; set; }
        public int Amount { get; set; }
        [ForeignKey("Organisation")]
        public int OrgID { get; set; }
        [ValidateNever]
        public virtual Organisation Organisation { get; set; }
        public DateTime EmployeeTransactionDate { get; set; }
    }
}
