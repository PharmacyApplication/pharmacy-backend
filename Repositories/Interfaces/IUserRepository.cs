using PharmacyAPI.Models;

namespace PharmacyAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
    }
}