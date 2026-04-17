using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory> GetByMedicineIdAsync(int medicineId);
        Task<Inventory> CreateAsync(Inventory inventory);
        Task<Inventory> UpdateAsync(Inventory inventory);
        Task<bool> DeleteAsync(int inventoryId);  // NEW: for hard-delete cleanup
        Task<IEnumerable<Inventory>> GetLowStockAsync();
    }
}