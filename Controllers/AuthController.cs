using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.DTOs.Auth;
using PharmacyAPI.Helpers;
using PharmacyAPI.Services.Interfaces;

namespace PharmacyAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IAuthService authService, JwtHelper jwtHelper)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _jwtHelper.GetUserIdFromToken(User);
            if (userId == null) return Unauthorized();

            try
            {
                var user = await _authService.GetProfileAsync(userId.Value);
                return Ok(new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.Address,
                    user.Role,
                    user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = _jwtHelper.GetUserIdFromToken(User);
            if (userId == null) return Unauthorized();

            try
            {
                var user = await _authService.UpdateProfileAsync(userId.Value, dto);
                return Ok(new
                {
                    user.UserId,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    user.Address,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}