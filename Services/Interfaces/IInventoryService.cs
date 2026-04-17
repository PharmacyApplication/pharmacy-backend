using PharmacyAPI.DTOs.Medicine;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDto>> GetAllInventoryAsync();
        Task<InventoryDto> UpdateInventoryAsync(int medicineId, UpdateInventoryDto dto);
        Task<IEnumerable<InventoryDto>> GetLowStockAsync();
    }

}
