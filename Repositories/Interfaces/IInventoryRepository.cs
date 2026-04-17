using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory> GetByMedicineIdAsync(int medicineId);
        Task<Inventory> UpdateAsync(Inventory inventory);
        Task<IEnumerable<Inventory>> GetLowStockAsync();
        Task<Inventory> CreateAsync(Inventory inventory);
    }

}
