using PharmacyAPI.DTOs.Prescription;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<PrescriptionDto> UploadPrescriptionAsync(int userId, int medicineId, IFormFile file);
        Task<IEnumerable<PrescriptionDto>> GetUserPrescriptionsAsync(int userId);
        Task<PrescriptionDto> ReviewPrescriptionAsync(int prescriptionId, string status, string? rejectionReason, int reviewedBy);
    }
}
