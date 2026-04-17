using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;

namespace PharmacyAPI.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Prescription> AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task<IEnumerable<Prescription>> GetByUserIdAsync(int userId)
        {
            return await _context.Prescriptions
                .Include(p => p.Medicine)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync();
        }

        public async Task<Prescription?> GetByIdAsync(int prescriptionId)
        {
            return await _context.Prescriptions
                .Include(p => p.Medicine)
                .FirstOrDefaultAsync(p => p.PrescriptionId == prescriptionId);
        }

        public async Task<Prescription?> GetApprovedPrescriptionAsync(int userId, int medicineId)
        {
            return await _context.Prescriptions
                .FirstOrDefaultAsync(p =>
                    p.UserId == userId &&
                    p.MedicineId == medicineId &&
                    p.Status == "Approved");
        }

        public async Task<Prescription> UpdateAsync(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }
    }
}