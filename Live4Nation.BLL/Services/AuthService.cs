using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration; // 👈 Yeh line add kijiye

namespace Live4Nation.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Admin> _passwordHasher = new();

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateToken(Admin admin)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
            var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
            var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");
            var expiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.FullName),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
{
    // 1. Database se email aur IsActive check karke admin nikalega
    var admin = await _context.Admins
        .FirstOrDefaultAsync(x => x.Email == dto.Email && x.IsActive);

    if (admin == null)
    {
        return null;
    }

    // 2. 👇 Plain Text Password Match (Bina kisi hashing ke direct check)
    if (admin.PasswordHash != dto.Password)
    {
        return null; // Agar match nahi hua toh login fail
    }

    // 3. Agar password sahi hai toh token banega
    var token = GenerateToken(admin);

    return new LoginResponseDto
    {
        Id = admin.Id,
        FullName = admin.FullName,
        Email = admin.Email,
        ProfileImage = admin.ProfileImage,
        Token = token,
        ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "60"))
    };
}
     public async Task<bool> ChangePasswordAsync(int adminId, ChangePasswordDto dto)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == adminId && x.IsActive);

            if (admin == null)
            {
                return false;
            }

            var currentResult = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, dto.CurrentPassword);
            if (currentResult == PasswordVerificationResult.Failed)
            {
                return false;
            }

            admin.PasswordHash = _passwordHasher.HashPassword(admin, dto.NewPassword);
            admin.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AdminProfileDto?> GetProfileAsync(int adminId)
        {
            var admin = await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == adminId && x.IsActive);

            if (admin == null)
            {
                return null;
            }

            return new AdminProfileDto
            {
                Id = admin.Id,
                FullName = admin.FullName,
                Email = admin.Email,
                ProfileImage = admin.ProfileImage,
                IsActive = admin.IsActive,
                CreatedDate = admin.CreatedDate,
                UpdatedDate = admin.UpdatedDate
            };
        }
    }
}