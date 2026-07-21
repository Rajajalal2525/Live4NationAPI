using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDto>> GetAllAsync();
        Task<NewsDto?> GetByIdAsync(int id);
        Task<NewsDetailsDto?> GetDetailsByIdAsync(int id);

        Task<List<NewsDto>> GetLatestAsync(int take = 10);
        Task<List<NewsDto>> GetBreakingAsync(int take = 10);
        Task<List<NewsDto>> GetTrendingAsync(int take = 10);
        Task<List<NewsDto>> GetFeaturedAsync(int take = 10);
        Task<List<NewsDto>> GetTopStoryAsync(int take = 10);
        Task<List<NewsDto>> GetByCategoryAsync(int categoryId);

        Task<NewsDto?> CreateAsync(NewsCreateUpdateDto dto);
        Task<bool> UpdateAsync(int id, NewsCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<NewsImageDto?> AddImageAsync(int newsId, NewsImageCreateDto dto);
        Task<List<NewsImageDto>> GetImagesByNewsIdAsync(int newsId);
        Task<bool> DeleteImageAsync(int imageId);
    }
}