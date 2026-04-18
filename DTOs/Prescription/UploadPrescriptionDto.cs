using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.DTOs.Prescription
{
    public class UploadPrescriptionDto
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}