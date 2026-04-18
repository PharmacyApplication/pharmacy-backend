using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;

namespace PharmacyAPI.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly AppDbContext _context;

        public OrderStatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithUsersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.PlacedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Medicine)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
