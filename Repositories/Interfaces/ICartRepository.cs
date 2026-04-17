using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart> GetOrCreateCartAsync(int userId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        Task ClearCartItemsAsync(int cartId);
    }
}
