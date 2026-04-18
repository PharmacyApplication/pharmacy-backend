using PharmacyAPI.DTOs.Medicine;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public MedicineService(IMedicineRepository medicineRepository,
                               IInventoryRepository inventoryRepository)
        {
            _medicineRepository = medicineRepository;
            _inventoryRepository = inventoryRepository;
        }

        // Active medicines only (for public/customer views)
        public async Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync()
        {
            var medicines = await _medicineRepository.GetAllActiveAsync();
            var result = new List<MedicineDto>();
            foreach (var m in medicines)
            {
                var inventory = await _inventoryRepository.GetByMedicineIdAsync(m.MedicineId);
                result.Add(MapToDto(m, inventory));
            }
            return result;
        }

        // All medicines including inactive (for admin manage view)
        public async Task<IEnumerable<MedicineDto>> GetAllMedicinesIncludingInactiveAsync()
        {
            var medicines = await _medicineRepository.GetAllIncludingInactiveAsync();
            var result = new List<MedicineDto>();
            foreach (var m in medicines)
            {
                var inventory = await _inventoryRepository.GetByMedicineIdAsync(m.MedicineId);
                result.Add(MapToDto(m, inventory));
            }
            return result;
        }

        public async Task<MedicineDto> GetMedicineByIdAsync(int id)
        {
            var medicine = await _medicineRepository.GetByIdAsync(id);
            if (medicine == null) return null;
            var inventory = await _inventoryRepository.GetByMedicineIdAsync(id);
            return MapToDto(medicine, inventory);
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicinesByCategoryAsync(int categoryId)
        {
            var medicines = await _medicineRepository.GetByCategoryAsync(categoryId);
            var result = new List<MedicineDto>();
            foreach (var m in medicines)
            {
                var inventory = await _inventoryRepository.GetByMedicineIdAsync(m.MedicineId);
                result.Add(MapToDto(m, inventory));
            }
            return result;
        }

        public async Task<MedicineDto> CreateMedicineAsync(CreateMedicineDto dto)
        {
            var medicine = new Medicine
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                RequiresPrescription = dto.RequiresPrescription,
                ImageUrl = dto.ImageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _medicineRepository.CreateAsync(medicine);

            var inventory = new Inventory
            {
                MedicineId = created.MedicineId,
                QuantityInStock = dto.QuantityInStock,
                ReorderLevel = dto.ReorderLevel,
                LastUpdated = DateTime.UtcNow
            };
            await _inventoryRepository.CreateAsync(inventory);

            var full = await _medicineRepository.GetByIdAsync(created.MedicineId);
            return MapToDto(full, inventory);
        }

        public async Task<MedicineDto> UpdateMedicineAsync(int id, UpdateMedicineDto dto)
        {
            var medicine = await _medicineRepository.GetByIdAsync(id);
            if (medicine == null) return null;

            medicine.Name = dto.Name;
            medicine.Description = dto.Description;
            medicine.Price = dto.Price;
            medicine.CategoryId = dto.CategoryId;
            medicine.RequiresPrescription = dto.RequiresPrescription;
            medicine.ImageUrl = dto.ImageUrl;
            medicine.IsActive = dto.IsActive;

            var updated = await _medicineRepository.UpdateAsync(medicine);

            // Also sync inventory when editing a medicine
            var inventory = await _inventoryRepository.GetByMedicineIdAsync(id);
            if (inventory != null)
            {
                inventory.QuantityInStock = dto.QuantityInStock;
                inventory.ReorderLevel = dto.ReorderLevel;
                inventory.LastUpdated = DateTime.UtcNow;
                await _inventoryRepository.UpdateAsync(inventory);
            }
            else
            {
                // Safety: create inventory if somehow missing
                inventory = new Inventory
                {
                    MedicineId = id,
                    QuantityInStock = dto.QuantityInStock,
                    ReorderLevel = dto.ReorderLevel,
                    LastUpdated = DateTime.UtcNow
                };
                await _inventoryRepository.CreateAsync(inventory);
            }

            return MapToDto(updated, inventory);
        }

        // Soft delete — marks IsActive = false (reversible)
        public async Task<bool> DeleteMedicineAsync(int id)
        {
            return await _medicineRepository.SoftDeleteAsync(id);
        }

        // Hard delete — permanently removes medicine and its inventory
        public async Task<bool> HardDeleteMedicineAsync(int id)
        {
            // Remove inventory record first (FK constraint)
            var inventory = await _inventoryRepository.GetByMedicineIdAsync(id);
            if (inventory != null)
                await _inventoryRepository.DeleteAsync(inventory.InventoryId);

            return await _medicineRepository.HardDeleteAsync(id);
        }

        // Restore — marks IsActive = true
        public async Task<MedicineDto> RestoreMedicineAsync(int id)
        {
            var medicine = await _medicineRepository.GetByIdAsync(id);
            if (medicine == null) return null;

            medicine.IsActive = true;
            var updated = await _medicineRepository.UpdateAsync(medicine);
            var inventory = await _inventoryRepository.GetByMedicineIdAsync(id);
            return MapToDto(updated, inventory);
        }

        private MedicineDto MapToDto(Medicine m, Inventory inventory)
        {
            return new MedicineDto
            {
                MedicineId = m.MedicineId,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                CategoryId = m.CategoryId,
                CategoryName = m.Category?.CategoryName ?? string.Empty,
                RequiresPrescription = m.RequiresPrescription,
                ImageUrl = m.ImageUrl,
                IsActive = m.IsActive,
                CreatedAt = m.CreatedAt,
                QuantityInStock = inventory?.QuantityInStock ?? 0,
                ReorderLevel = inventory?.ReorderLevel ?? 0,
                IsLowStock = inventory != null && inventory.QuantityInStock < inventory.ReorderLevel
            };
        }

        public async Task<IEnumerable<object>> GetAllCategoriesAsync()
        {
            var categories = await _medicineRepository.GetAllCategoriesAsync();
            return categories.Select(c => new
            {
                categoryId = c.CategoryId,
                categoryName = c.CategoryName,
                description = c.Description
            });
        }
    }
}