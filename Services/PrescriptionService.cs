using Microsoft.AspNetCore.Http;
using PharmacyAPI.DTOs.Prescription;
using PharmacyAPI.Helpers;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyAPI.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly FileUploadHelper _fileUploadHelper;

        public PrescriptionService(
            IPrescriptionRepository prescriptionRepository,
            FileUploadHelper fileUploadHelper)
        {
            _prescriptionRepository = prescriptionRepository;
            _fileUploadHelper = fileUploadHelper;
        }

        public async Task<PrescriptionDto> UploadPrescriptionAsync(int userId, int medicineId, IFormFile file)
        {
            var filePath = await _fileUploadHelper.SavePrescriptionFileAsync(file);

            var prescription = new Prescription
            {
                UserId = userId,
                MedicineId = medicineId,
                FilePath = filePath,
                Status = "Pending",
                UploadedAt = DateTime.UtcNow
            };

            var saved = await _prescriptionRepository.AddAsync(prescription);
            return MapToDto(saved);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetUserPrescriptionsAsync(int userId)
        {
            var prescriptions = await _prescriptionRepository.GetByUserIdAsync(userId);
            return prescriptions.Select(MapToDto);
        }

        public async Task<PrescriptionDto> ReviewPrescriptionAsync(
            int prescriptionId, string status, string? rejectionReason, int reviewedBy)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(prescriptionId);
            if (prescription == null)
                throw new KeyNotFoundException($"Prescription {prescriptionId} not found.");

            if (status != "Approved" && status != "Rejected")
                throw new ArgumentException("Status must be 'Approved' or 'Rejected'.");

            prescription.Status = status;
            prescription.ReviewedAt = DateTime.UtcNow;
            prescription.ReviewedBy = reviewedBy;
            prescription.RejectionReason = status == "Rejected" ? rejectionReason : null;

            var updated = await _prescriptionRepository.UpdateAsync(prescription);
            return MapToDto(updated);
        }

        private static PrescriptionDto MapToDto(Prescription p) => new PrescriptionDto
        {
            PrescriptionId = p.PrescriptionId,
            UserId = p.UserId,
            MedicineId = p.MedicineId,
            MedicineName = p.Medicine?.Name ?? string.Empty,
            FilePath = p.FilePath,
            Status = p.Status,
            UploadedAt = p.UploadedAt,
            ReviewedAt = p.ReviewedAt,
            ReviewedBy = p.ReviewedBy,
            RejectionReason = p.RejectionReason
        };
    }
}