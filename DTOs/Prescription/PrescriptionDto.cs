using System;

namespace PharmacyAPI.DTOs.Prescription
{
    public class PrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public int UserId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Pending / Approved / Rejected
        public DateTime UploadedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public int? ReviewedBy { get; set; }
        public string? RejectionReason { get; set; }
    }
}