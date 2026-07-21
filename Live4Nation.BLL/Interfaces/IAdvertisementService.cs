using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface IAdvertisementService
    {
        Task<List<AdvertisementDto>> GetAllAsync();
        Task<AdvertisementDto?> GetByIdAsync(int id);
        Task<AdvertisementDto> CreateAsync(AdvertisementCreateUpdateDto dto);
        Task<bool> UpdateAsync(int id, AdvertisementCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}