using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface IGalleryService
    {
        Task<List<GalleryDto>> GetAllAsync();
        Task<GalleryDto?> GetByIdAsync(int id);
        Task<GalleryDto> CreateAsync(GalleryCreateUpdateDto dto);
        Task<bool> UpdateAsync(int id, GalleryCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}