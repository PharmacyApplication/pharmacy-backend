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

            // Create initial inventory record with 0 stock
            var inventory = new Inventory
            {
                MedicineId = created.MedicineId,
                QuantityInStock = 0,
                ReorderLevel = 10,
                LastUpdated = DateTime.UtcNow
            };
            await _inventoryRepository.CreateAsync(inventory);

            // Reload to get category name
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
            var inventory = await _inventoryRepository.GetByMedicineIdAsync(id);
            return MapToDto(updated, inventory);
        }

        public async Task<bool> DeleteMedicineAsync(int id)
        {
            return await _medicineRepository.SoftDeleteAsync(id);
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
                ReorderLevel = inventory?.ReorderLevel ?? 0
            };
        }
    }

}
