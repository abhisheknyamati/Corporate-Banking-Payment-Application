using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public int EmployeeSalary { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual Organisation Organisation { get; set; }
        public int AccountNumber { get; set; }
        public string IFSC { get; set; }
        public bool IsActive { get; set; }
    }
}
