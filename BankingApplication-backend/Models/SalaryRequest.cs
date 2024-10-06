using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication_backend.Models
{
    public class SalaryRequest
    {
        [Key]
        public int SalaryRequestId { get; set; }
        [ForeignKey("Organisation")]
        public int OrgID { get; set; }
        [ValidateNever]
        public virtual Organisation Organisation { get; set; }
        public int EmployeeIds { get; set; }
        public string Status { get; set; }
    }
}
