using PharmacyAPI.DTOs.Medicine;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync();
        Task<MedicineDto> GetMedicineByIdAsync(int id);
        Task<IEnumerable<MedicineDto>> GetMedicinesByCategoryAsync(int categoryId);
        Task<MedicineDto> CreateMedicineAsync(CreateMedicineDto dto);
        Task<MedicineDto> UpdateMedicineAsync(int id, UpdateMedicineDto dto);
        Task<bool> DeleteMedicineAsync(int id);
        Task<IEnumerable<object>> GetAllCategoriesAsync();
        Task<IEnumerable<MedicineDto>> GetAllMedicinesIncludingInactiveAsync();
        Task<bool> HardDeleteMedicineAsync(int id);
        Task<MedicineDto> RestoreMedicineAsync(int id);
    }

}
