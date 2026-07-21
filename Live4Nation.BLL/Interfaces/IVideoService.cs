using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface IVideoService
    {
        Task<List<VideoDto>> GetAllAsync();
        Task<VideoDto?> GetByIdAsync(int id);
        Task<VideoDto> CreateAsync(VideoCreateUpdateDto dto);
        Task<bool> UpdateAsync(int id, VideoCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}