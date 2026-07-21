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
        Task<long?> IncrementViewCountAsync(int id);

        Task<NewsImageDto?> AddImageAsync(int newsId, NewsImageCreateDto dto);
        Task<List<NewsImageDto>> GetImagesByNewsIdAsync(int newsId);
        Task<bool> DeleteImageAsync(int imageId);

        // Home page task 

        Task<NewsDetailPageDto?> GetDetailPageAsync(int id);

        Task<PagedResultDto<SearchResponseDto>> SearchAsync(string keyword, int page, int pageSize);

        Task<PagedResultDto<NewsDto>> GetAllPagedAsync(int page = 1, int pageSize = 10, int? categoryId = null, string? location = null, string? search = null, bool? isBreaking = null, bool? isTrending = null, bool? isFeatured = null, bool? isTopStory = null, DateTime? fromDate = null, DateTime? toDate = null, string? sortBy = null);
        Task<List<SearchResponseDto>?> GetRelatedAsync(int id, int take = 6);


        Task<NewsNavigationResponseDto?> GetNavigationAsync(int id);
        Task<List<SearchResponseDto>> GetPopularAsync(int take = 10);
        Task<PagedResultDto<SearchResponseDto>> GetNewsByLocationAsync(string location, int page = 1, int pageSize = 10);

    }
}