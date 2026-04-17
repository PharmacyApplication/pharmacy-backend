using PharmacyAPI.Data;
using PharmacyAPI.DTOs.Order;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly AppDbContext _context; // Only used to look up medicine details

        public CartService(
            ICartRepository cartRepository,
            IPrescriptionRepository prescriptionRepository,
            AppDbContext context)
        {
            _cartRepository = cartRepository;
            _prescriptionRepository = prescriptionRepository;
            _context = context;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await _cartRepository.GetOrCreateCartAsync(userId);
            return MapToDto(cart);
        }

        public async Task<CartDto> AddToCartAsync(int userId, int medicineId, int quantity, int? prescriptionId)
        {
            // Fetch medicine to check RequiresPrescription
            var medicine = await _context.Medicines.FindAsync(medicineId);
            if (medicine == null)
                throw new KeyNotFoundException($"Medicine {medicineId} not found.");

            if (medicine.RequiresPrescription)
            {
                var approved = await _prescriptionRepository.GetApprovedPrescriptionAsync(userId, medicineId);
                if (approved == null)
                    throw new InvalidOperationException("Prescription required and not approved");
            }

            var cart = await _cartRepository.GetOrCreateCartAsync(userId);

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                MedicineId = medicineId,
                Quantity = quantity,
                PrescriptionId = prescriptionId
            };

            await _cartRepository.AddCartItemAsync(cartItem);

            // Reload cart with updated items
            var updatedCart = await _cartRepository.GetOrCreateCartAsync(userId);
            return MapToDto(updatedCart);
        }

        public async Task RemoveCartItemAsync(int userId, int cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null)
                throw new KeyNotFoundException($"Cart item {cartItemId} not found.");

            // Ensure the item belongs to the requesting user's cart
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || cartItem.CartId != cart.CartId)
                throw new UnauthorizedAccessException("Cart item does not belong to this user.");

            await _cartRepository.RemoveCartItemAsync(cartItem);
        }

        private static CartDto MapToDto(Cart cart) => new CartDto
        {
            CartId = cart.CartId,
            UserId = cart.UserId,
            CreatedAt = cart.CreatedAt,
            Items = cart.CartItems?.Select(ci => new CartItemDto
            {
                CartItemId = ci.CartItemId,
                MedicineId = ci.MedicineId,
                MedicineName = ci.Medicine?.Name ?? string.Empty,
                Price = ci.Medicine?.Price ?? 0,
                Quantity = ci.Quantity,
                PrescriptionId = ci.PrescriptionId
            }).ToList() ?? new()
        };
    }
}
