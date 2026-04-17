using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<Medicine>> GetAllActiveAsync();
        Task<Medicine> GetByIdAsync(int id);
        Task<IEnumerable<Medicine>> GetByCategoryAsync(int categoryId);
        Task<Medicine> CreateAsync(Medicine medicine);
        Task<Medicine> UpdateAsync(Medicine medicine);
        Task<bool> SoftDeleteAsync(int id);
        Task<string> GetCategoryNameAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }

}
