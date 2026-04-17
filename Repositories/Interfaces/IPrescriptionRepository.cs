using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IPrescriptionRepository
    {
      public Task<Prescription> AddAsync(Prescription prescription);
      public Task<IEnumerable<Prescription>> GetByUserIdAsync(int userId);
      public Task<Prescription?> GetByIdAsync(int prescriptionId);
      public Task<Prescription?> GetApprovedPrescriptionAsync(int userId, int medicineId);
      public Task<Prescription> UpdateAsync(Prescription prescription);
    }
}
