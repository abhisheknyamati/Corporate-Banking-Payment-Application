using System.ComponentModel.DataAnnotations;

namespace BankingApplication_backend.Models
{
    public class Download
    {

        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; } // e.g., "application/pdf"

        public byte[] Content { get; set; }
    }
}
