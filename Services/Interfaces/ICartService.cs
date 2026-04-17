using PharmacyAPI.DTOs.Order;

namespace PharmacyAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task<CartDto> AddToCartAsync(int userId, int medicineId, int quantity, int? prescriptionId);
        Task RemoveCartItemAsync(int userId, int cartItemId);
    }
}
