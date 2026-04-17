using PharmacyAPI.DTOs.Auth;
using PharmacyAPI.Helpers;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Email already registered.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var created = await _userRepository.CreateAsync(user);
            var token = _jwtHelper.GenerateToken(created);

            return new AuthResponseDto
            {
                Token = token,
                UserId = created.UserId,
                FullName = created.FullName,
                Email = created.Email,
                Role = created.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password.");

            if (!user.IsActive)
                throw new Exception("Account is disabled.");

            var token = _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<User> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");
            return user;
        }

        public async Task<User> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            if (!string.IsNullOrEmpty(dto.FullName)) user.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.PhoneNumber)) user.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrEmpty(dto.Address)) user.Address = dto.Address;

            return await _userRepository.UpdateAsync(user);
        }
    }
}