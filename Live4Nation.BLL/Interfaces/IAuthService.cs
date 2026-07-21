using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
        Task<bool> ChangePasswordAsync(int adminId, ChangePasswordDto dto);
        Task<AdminProfileDto?> GetProfileAsync(int adminId);
    }
}