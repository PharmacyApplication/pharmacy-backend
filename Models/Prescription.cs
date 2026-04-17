using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        public int UserId { get; set; }
        public int MedicineId { get; set; }
        [Required]
        public string FilePath { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending | Approved | Rejected
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public int? ReviewedBy { get; set; }
        public string? RejectionReason { get; set; }

        public User? User { get; set; }
        public Medicine? Medicine { get; set; }
    }
}