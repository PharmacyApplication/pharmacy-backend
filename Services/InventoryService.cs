using PharmacyAPI.DTOs.Medicine;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllInventoryAsync()
        {
            var inventories = await _inventoryRepository.GetAllAsync();
            return inventories.Select(MapToDto);
        }

        public async Task<InventoryDto> UpdateInventoryAsync(int medicineId, UpdateInventoryDto dto)
        {
            var inventory = await _inventoryRepository.GetByMedicineIdAsync(medicineId);
            if (inventory == null) return null;

            inventory.QuantityInStock = dto.QuantityInStock;
            inventory.ReorderLevel = dto.ReorderLevel;
            inventory.LastUpdated = DateTime.UtcNow;

            var updated = await _inventoryRepository.UpdateAsync(inventory);
            return MapToDto(updated);
        }

        public async Task<IEnumerable<InventoryDto>> GetLowStockAsync()
        {
            var inventories = await _inventoryRepository.GetLowStockAsync();
            return inventories.Select(MapToDto);
        }

        private InventoryDto MapToDto(PharmacyAPI.Models.Inventory i)
        {
            return new InventoryDto
            {
                InventoryId = i.InventoryId,
                MedicineId = i.MedicineId,
                MedicineName = i.Medicine?.Name ?? string.Empty,
                QuantityInStock = i.QuantityInStock,
                ReorderLevel = i.ReorderLevel,
                LastUpdated = i.LastUpdated
            };
        }
    }

}
