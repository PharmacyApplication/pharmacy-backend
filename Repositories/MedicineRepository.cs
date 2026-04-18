using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PharmacyAPI.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly AppDbContext _context;

        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medicine>> GetAllActiveAsync()
        {
            return await _context.Medicines
                .Include(m => m.Category)
                .Where(m => m.IsActive)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        // NEW: returns ALL medicines including inactive ones (for admin views)
        public async Task<IEnumerable<Medicine>> GetAllIncludingInactiveAsync()
        {
            return await _context.Medicines
                .Include(m => m.Category)
                .OrderByDescending(m => m.IsActive)  // active first
                .ThenBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Medicine> GetByIdAsync(int id)
        {
            return await _context.Medicines
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MedicineId == id);
        }

        public async Task<IEnumerable<Medicine>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Medicines
                .Include(m => m.Category)
                .Where(m => m.CategoryId == categoryId && m.IsActive)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Medicine> CreateAsync(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
            return medicine;
        }

        public async Task<Medicine> UpdateAsync(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
            return medicine;
        }

        // Soft delete — reversible, marks IsActive = false
        public async Task<bool> SoftDeleteAsync(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return false;

            medicine.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // Hard delete — permanent, removes the row entirely
        public async Task<bool> HardDeleteAsync(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null) return false;

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetCategoryNameAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            return category?.CategoryName;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }
    }
}