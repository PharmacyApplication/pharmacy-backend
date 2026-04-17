using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace PharmacyAPI.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                .Include(i => i.Medicine)
                .OrderBy(i => i.Medicine.Name)
                .ToListAsync();
        }

        public async Task<Inventory> GetByMedicineIdAsync(int medicineId)
        {
            return await _context.Inventories
                .Include(i => i.Medicine)
                .FirstOrDefaultAsync(i => i.MedicineId == medicineId);
        }

        public async Task<Inventory> UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task<IEnumerable<Inventory>> GetLowStockAsync()
        {
            return await _context.Inventories
                .Include(i => i.Medicine)
                .Where(i => i.QuantityInStock <= i.ReorderLevel)
                .OrderBy(i => i.QuantityInStock)
                .ToListAsync();
        }

        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }
    }

}
