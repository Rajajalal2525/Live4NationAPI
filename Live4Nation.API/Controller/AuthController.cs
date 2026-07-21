using System.Security.Claims;
using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(adminIdClaim, out var adminId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var updated = await _authService.ChangePasswordAsync(adminId, dto);

            if (!updated)
            {
                return BadRequest(new { message = "Current password is wrong or admin not found." });
            }

            return Ok(new { message = "Password changed successfully." });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(adminIdClaim, out var adminId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var profile = await _authService.GetProfileAsync(adminId);

            if (profile == null)
            {
                return NotFound(new { message = "Profile not found." });
            }

            return Ok(profile);
        }
    }
}