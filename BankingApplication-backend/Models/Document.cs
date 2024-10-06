using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BankingApplication_backend.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        [ForeignKey("Organisation")]
        public int? OrganisationId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual Organisation Organisation { get; set; }
        [ForeignKey("Bank")]
        public int? BankId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual Bank Bank { get; set; }

    }
}
