using PharmacyAPI.DTOs.Auth;
using PharmacyAPI.Models;

namespace PharmacyAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<User> GetProfileAsync(int userId);
        Task<User> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    }
}