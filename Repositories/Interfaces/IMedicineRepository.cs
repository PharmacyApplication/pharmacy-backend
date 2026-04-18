using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<Medicine>> GetAllActiveAsync();
        Task<IEnumerable<Medicine>> GetAllIncludingInactiveAsync(); // NEW
        Task<Medicine> GetByIdAsync(int id);
        Task<IEnumerable<Medicine>> GetByCategoryAsync(int categoryId);
        Task<Medicine> CreateAsync(Medicine medicine);
        Task<Medicine> UpdateAsync(Medicine medicine);
        Task<bool> SoftDeleteAsync(int id);   // marks IsActive = false
        Task<bool> HardDeleteAsync(int id);   // permanently removes row
        Task<string> GetCategoryNameAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}