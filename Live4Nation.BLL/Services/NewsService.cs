using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;

        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static string BuildSlug(string title)
        {
            return string.Join("-", title
                .Trim()
                .ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static NewsDto MapToDto(News news)
        {
            return new NewsDto
            {
                Id = news.Id,
                CategoryId = news.CategoryId,
                CategoryName = news.Category != null ? news.Category.Name : string.Empty,
                Title = news.Title,
                Slug = news.Slug,
                ShortDescription = news.ShortDescription,
                Description = news.Description,
                ThumbnailImage = news.ThumbnailImage,
                Author = news.Author,
                Location = news.Location,
                PublishedDate = news.PublishedDate,
                IsBreaking = news.IsBreaking,
                IsTrending = news.IsTrending,
                IsFeatured = news.IsFeatured,
                IsTopStory = news.IsTopStory,
                Priority = news.Priority,
                ViewCount = news.ViewCount,
                IsActive = news.IsActive,
                CreatedDate = news.CreatedDate,
                UpdatedDate = news.UpdatedDate
            };
        }

        private static NewsImageDto MapImageToDto(NewsImage image)
        {
            return new NewsImageDto
            {
                Id = image.Id,
                NewsId = image.NewsId,
                ImageUrl = image.ImageUrl,
                Caption = image.Caption,
                DisplayOrder = image.DisplayOrder,
                CreatedDate = image.CreatedDate
            };
        }

        private IQueryable<News> NewsQuery()
        {
            return _context.News
                .AsNoTracking()
                .Include(x => x.Category);
        }

        public async Task<List<NewsDto>> GetAllAsync()
        {
            return await NewsQuery()
                .OrderByDescending(x => x.PublishedDate)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<NewsDto?> GetByIdAsync(int id)
        {
            var news = await NewsQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            return news == null ? null : MapToDto(news);
        }

        public async Task<NewsDetailsDto?> GetDetailsByIdAsync(int id)
        {
            var news = await _context.News
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.NewsImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news == null)
            {
                return null;
            }

            return new NewsDetailsDto
            {
                News = MapToDto(news),
                Images = news.NewsImages
                    .OrderBy(x => x.DisplayOrder)
                    .Select(MapImageToDto)
                    .ToList()
            };
        }

        public async Task<List<NewsDto>> GetLatestAsync(int take = 10)
        {
            return await NewsQuery()
                .OrderByDescending(x => x.PublishedDate)
                .Take(take)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<List<NewsDto>> GetBreakingAsync(int take = 10)
        {
            return await NewsQuery()
                .Where(x => x.IsBreaking)
                .OrderByDescending(x => x.PublishedDate)
                .Take(take)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<List<NewsDto>> GetTrendingAsync(int take = 10)
        {
            return await NewsQuery()
                .Where(x => x.IsTrending)
                .OrderByDescending(x => x.PublishedDate)
                .Take(take)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<List<NewsDto>> GetFeaturedAsync(int take = 10)
        {
            return await NewsQuery()
                .Where(x => x.IsFeatured)
                .OrderByDescending(x => x.PublishedDate)
                .Take(take)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<List<NewsDto>> GetTopStoryAsync(int take = 10)
        {
            return await NewsQuery()
                .Where(x => x.IsTopStory)
                .OrderByDescending(x => x.PublishedDate)
                .Take(take)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<List<NewsDto>> GetByCategoryAsync(int categoryId)
        {
            return await NewsQuery()
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.PublishedDate)
                .Select(x => new NewsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Description = x.Description,
                    ThumbnailImage = x.ThumbnailImage,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    Priority = x.Priority,
                    ViewCount = x.ViewCount,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<NewsDto?> CreateAsync(NewsCreateUpdateDto dto)
        {
            var categoryExists = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return null;
            }

            var entity = new News
            {
                CategoryId = dto.CategoryId,
                Title = dto.Title.Trim(),
                Slug = string.IsNullOrWhiteSpace(dto.Slug) ? BuildSlug(dto.Title) : dto.Slug.Trim(),
                ShortDescription = dto.ShortDescription.Trim(),
                Description = dto.Description.Trim(),
                ThumbnailImage = dto.ThumbnailImage.Trim(),
                Author = string.IsNullOrWhiteSpace(dto.Author) ? null : dto.Author.Trim(),
                Location = string.IsNullOrWhiteSpace(dto.Location) ? null : dto.Location.Trim(),
                PublishedDate = dto.PublishedDate,
                IsBreaking = dto.IsBreaking,
                IsTrending = dto.IsTrending,
                IsFeatured = dto.IsFeatured,
                IsTopStory = dto.IsTopStory,
                Priority = dto.Priority,
                ViewCount = dto.ViewCount,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _context.News.Add(entity);
            await _context.SaveChangesAsync();

            var created = await _context.News
                .AsNoTracking()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            return created == null ? null : MapToDto(created);
        }

        public async Task<bool> UpdateAsync(int id, NewsCreateUpdateDto dto)
        {
            var entity = await _context.News.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            var categoryExists = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return false;
            }

            entity.CategoryId = dto.CategoryId;
            entity.Title = dto.Title.Trim();
            entity.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? BuildSlug(dto.Title) : dto.Slug.Trim();
            entity.ShortDescription = dto.ShortDescription.Trim();
            entity.Description = dto.Description.Trim();
            entity.ThumbnailImage = dto.ThumbnailImage.Trim();
            entity.Author = string.IsNullOrWhiteSpace(dto.Author) ? null : dto.Author.Trim();
            entity.Location = string.IsNullOrWhiteSpace(dto.Location) ? null : dto.Location.Trim();
            entity.PublishedDate = dto.PublishedDate;
            entity.IsBreaking = dto.IsBreaking;
            entity.IsTrending = dto.IsTrending;
            entity.IsFeatured = dto.IsFeatured;
            entity.IsTopStory = dto.IsTopStory;
            entity.Priority = dto.Priority;
            entity.ViewCount = dto.ViewCount;
            entity.IsActive = dto.IsActive;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.News.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            _context.News.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<NewsImageDto?> AddImageAsync(int newsId, NewsImageCreateDto dto)
        {
            var newsExists = await _context.News.AnyAsync(x => x.Id == newsId);
            if (!newsExists)
            {
                return null;
            }

            var image = new NewsImage
            {
                NewsId = newsId,
                ImageUrl = dto.ImageUrl.Trim(),
                Caption = string.IsNullOrWhiteSpace(dto.Caption) ? null : dto.Caption.Trim(),
                DisplayOrder = dto.DisplayOrder,
                CreatedDate = DateTime.UtcNow
            };

            _context.NewsImages.Add(image);
            await _context.SaveChangesAsync();

            return MapImageToDto(image);
        }

        public async Task<List<NewsImageDto>> GetImagesByNewsIdAsync(int newsId)
        {
            return await _context.NewsImages
                .AsNoTracking()
                .Where(x => x.NewsId == newsId)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Id)
                .Select(x => new NewsImageDto
                {
                    Id = x.Id,
                    NewsId = x.NewsId,
                    ImageUrl = x.ImageUrl,
                    Caption = x.Caption,
                    DisplayOrder = x.DisplayOrder,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _context.NewsImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (image == null)
            {
                return false;
            }

            _context.NewsImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
public async Task<NewsDetailPageDto?> GetDetailPageAsync(int id)
{
    // 1. Pehle main news fetch karein (Properly Awaited)
    var news = await _context.News
        .AsNoTracking()
        .Include(x => x.Category)
        .Include(x => x.NewsImages)
        .FirstOrDefaultAsync(x => x.Id == id);

    if (news == null)
    {
        return null;
    }

    var currentNewsDto = new NewsDto
    {
        Id = news.Id,
        CategoryId = news.CategoryId,
        CategoryName = news.Category != null ? news.Category.Name : string.Empty,
        Title = news.Title,
        Slug = news.Slug,
        ShortDescription = news.ShortDescription,
        Description = news.Description,
        ThumbnailImage = news.ThumbnailImage,
        Author = news.Author,
        Location = news.Location,
        PublishedDate = news.PublishedDate,
        IsBreaking = news.IsBreaking,
        IsTrending = news.IsTrending,
        IsFeatured = news.IsFeatured,
        IsTopStory = news.IsTopStory,
        Priority = news.Priority,
        ViewCount = news.ViewCount + 1, // Response me hi badha hua view count bhej dete hain
        IsActive = news.IsActive,
        CreatedDate = news.CreatedDate,
        UpdatedDate = news.UpdatedDate
    };

    // 2. Sequential Awaits: Sabhi database calls ko ek-ek karke await karenge taaki DbContext crash na ho.
    
    var relatedNews = await _context.News
        .AsNoTracking()
        .Include(x => x.Category)
        .Where(x => x.IsActive && x.CategoryId == news.CategoryId && x.Id != id)
        .OrderByDescending(x => x.PublishedDate)
        .Take(5)
        .Select(x => new NewsDto
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            Title = x.Title,
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Description = x.Description,
            ThumbnailImage = x.ThumbnailImage,
            Author = x.Author,
            Location = x.Location,
            PublishedDate = x.PublishedDate,
            IsBreaking = x.IsBreaking,
            IsTrending = x.IsTrending,
            IsFeatured = x.IsFeatured,
            IsTopStory = x.IsTopStory,
            Priority = x.Priority,
            ViewCount = x.ViewCount,
            IsActive = x.IsActive,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate
        })
        .ToListAsync();

    // Independent services calls (line-by-line await)
    var latestNews = await GetLatestAsync(10);
    var trendingNews = await GetTrendingAsync(10);
    var breakingNews = await GetBreakingAsync(10);

    var previousNews = await _context.News
        .AsNoTracking()
        .Include(x => x.Category)
        .Where(x => x.IsActive && x.PublishedDate < news.PublishedDate)
        .OrderByDescending(x => x.PublishedDate)
        .ThenByDescending(x => x.Id)
        .Select(x => new NewsDto
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            Title = x.Title,
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Description = x.Description,
            ThumbnailImage = x.ThumbnailImage,
            Author = x.Author,
            Location = x.Location,
            PublishedDate = x.PublishedDate,
            IsBreaking = x.IsBreaking,
            IsTrending = x.IsTrending,
            IsFeatured = x.IsFeatured,
            IsTopStory = x.IsTopStory,
            Priority = x.Priority,
            ViewCount = x.ViewCount,
            IsActive = x.IsActive,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate
        })
        .FirstOrDefaultAsync();

    var nextNews = await _context.News
        .AsNoTracking()
        .Include(x => x.Category)
        .Where(x => x.IsActive && x.PublishedDate > news.PublishedDate)
        .OrderBy(x => x.PublishedDate)
        .ThenBy(x => x.Id)
        .Select(x => new NewsDto
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            CategoryName = x.Category.Name,
            Title = x.Title,
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Description = x.Description,
            ThumbnailImage = x.ThumbnailImage,
            Author = x.Author,
            Location = x.Location,
            PublishedDate = x.PublishedDate,
            IsBreaking = x.IsBreaking,
            IsTrending = x.IsTrending,
            IsFeatured = x.IsFeatured,
            IsTopStory = x.IsTopStory,
            Priority = x.Priority,
            ViewCount = x.ViewCount,
            IsActive = x.IsActive,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate
        })
        .FirstOrDefaultAsync();

    var images = await _context.NewsImages
        .AsNoTracking()
        .Where(x => x.NewsId == id)
        .OrderBy(x => x.DisplayOrder)
        .ThenBy(x => x.Id)
        .Select(x => new NewsImageDto
        {
            Id = x.Id,
            NewsId = x.NewsId,
            ImageUrl = x.ImageUrl,
            Caption = x.Caption,
            DisplayOrder = x.DisplayOrder,
            CreatedDate = x.CreatedDate
        })
        .ToListAsync();

    var advertisements = await _context.Advertisements
        .AsNoTracking()
        .Where(x => x.IsActive)
        .OrderByDescending(x => x.CreatedDate)
        .Select(x => new AdvertisementDto
        {
            Id = x.Id,
            Title = x.Title,
            ImageUrl = x.ImageUrl,
            RedirectUrl = x.RedirectUrl,
            Position = x.Position,
            IsActive = x.IsActive,
            CreatedDate = x.CreatedDate
        })
        .ToListAsync();

    // 3. ViewCount update karne ka sahi tarika (Kyunki main entity AsNoTracking hai)
    // Ek alag direct query se execute karenge taaki performance bani rahe aur error na aaye
    await _context.Database.ExecuteSqlRawAsync(
        "UPDATE News SET ViewCount = ViewCount + 1 WHERE Id = {0}", id);

    return new NewsDetailPageDto
    {
        News = currentNewsDto,
        Images = images,
        RelatedNews = relatedNews,
        LatestNews = latestNews,
        TrendingNews = trendingNews,
        BreakingNews = breakingNews,
        PreviousNews = previousNews,
        NextNews = nextNews,
        Advertisements = new NewsDetailPageAdvertisementsDto
        {
            Top = advertisements.Where(x => string.Equals(x.Position, "Top", StringComparison.OrdinalIgnoreCase)).ToList(),
            Middle = advertisements.Where(x => string.Equals(x.Position, "Middle", StringComparison.OrdinalIgnoreCase)).ToList(),
            Sidebar = advertisements.Where(x => string.Equals(x.Position, "Sidebar", StringComparison.OrdinalIgnoreCase)).ToList(),
            Bottom = advertisements.Where(x => string.Equals(x.Position, "Bottom", StringComparison.OrdinalIgnoreCase) || string.Equals(x.Position, "Footer", StringComparison.OrdinalIgnoreCase)).ToList()
        }
    };
}


 public async Task<PagedResultDto<SearchResponseDto>> SearchAsync(string keyword, int page, int pageSize)
        {
            var normalizedKeyword = keyword.Trim().ToLower();
            var query = _context.News
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Where(x =>
                    x.Title.ToLower().Contains(normalizedKeyword) ||
                    x.ShortDescription.ToLower().Contains(normalizedKeyword) ||
                    x.Description.ToLower().Contains(normalizedKeyword) ||
                    (x.Author != null && x.Author.ToLower().Contains(normalizedKeyword)) ||
                    (x.Location != null && x.Location.ToLower().Contains(normalizedKeyword)) ||
                    x.Category.Name.ToLower().Contains(normalizedKeyword));

            var totalRecords = await query.CountAsync();
            var totalPages = totalRecords == 0
                ? 0
                : (int)Math.Ceiling(totalRecords / (double)pageSize);

            var items = await query
                .OrderByDescending(x => x.PublishedDate)
                .ThenByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SearchResponseDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    Title = x.Title,
                    Slug = x.Slug,
                    ThumbnailImage = x.ThumbnailImage,
                    ShortDescription = x.ShortDescription,
                    Author = x.Author,
                    Location = x.Location,
                    PublishedDate = x.PublishedDate,
                    IsBreaking = x.IsBreaking,
                    IsTrending = x.IsTrending,
                    IsFeatured = x.IsFeatured,
                    IsTopStory = x.IsTopStory,
                    ViewCount = x.ViewCount
                })
                .ToListAsync();

            return new PagedResultDto<SearchResponseDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Items = items
            };
    
            } 


public async Task<PagedResultDto<NewsDto>> GetAllPagedAsync(int page = 1, int pageSize = 10, int? categoryId = null, string? location = null, string? search = null, bool? isBreaking = null, bool? isTrending = null, bool? isFeatured = null, bool? isTopStory = null, DateTime? fromDate = null, DateTime? toDate = null, string? sortBy = null)
{
    try
    {
        // 1. Pagination Inputs Validation
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        // 2. Base Query Setup
        IQueryable<News> query = _context.News
            .AsNoTracking()
            .Where(x => x.IsActive);

        // 3. Conditional Filtering
        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            var normalizedLocation = location.Trim();
            query = query.Where(x => x.Location != null && x.Location.Contains(normalizedLocation));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();
            query = query.Where(x =>
                x.Title.Contains(keyword) ||
                x.ShortDescription.Contains(keyword) ||
                x.Description.Contains(keyword) ||
                (x.Author != null && x.Author.Contains(keyword)) ||
                (x.Location != null && x.Location.Contains(keyword)) ||
                (x.Category != null && x.Category.Name.Contains(keyword)));
        }

        if (isBreaking.HasValue) query = query.Where(x => x.IsBreaking == isBreaking.Value);
        if (isTrending.HasValue) query = query.Where(x => x.IsTrending == isTrending.Value);
        if (isFeatured.HasValue) query = query.Where(x => x.IsFeatured == isFeatured.Value);
        if (isTopStory.HasValue) query = query.Where(x => x.IsTopStory == isTopStory.Value);

        // 4. Date Filtering
        if (fromDate.HasValue)
        {
            var from = fromDate.Value.Date;
            query = query.Where(x => x.PublishedDate >= from);
        }

        if (toDate.HasValue)
        {
            var to = toDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(x => x.PublishedDate <= to);
        }

        // 5. Dynamic Sorting
        query = (sortBy ?? "Latest").Trim().ToLowerInvariant() switch
        {
            "oldest" => query.OrderBy(x => x.PublishedDate).ThenBy(x => x.Id),
            "mostviewed" => query.OrderByDescending(x => x.ViewCount).ThenByDescending(x => x.PublishedDate),
            "priority" => query.OrderBy(x => x.Priority).ThenByDescending(x => x.PublishedDate),
            _ => query.OrderByDescending(x => x.PublishedDate).ThenByDescending(x => x.Id)
        };

        // 6. Execution & Pagination
        var totalRecords = await query.CountAsync();
        var totalPages = totalRecords == 0 ? 0 : (int)Math.Ceiling(totalRecords / (double)pageSize);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new NewsDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.Name : string.Empty,
                Title = x.Title,
                Slug = x.Slug,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                ThumbnailImage = x.ThumbnailImage,
                Author = x.Author,
                Location = x.Location,
                PublishedDate = x.PublishedDate,
                IsBreaking = x.IsBreaking,
                IsTrending = x.IsTrending,
                IsFeatured = x.IsFeatured,
                IsTopStory = x.IsTopStory,
                Priority = x.Priority,
                ViewCount = x.ViewCount,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            })
            .ToListAsync();

        return new PagedResultDto<NewsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Items = items
        };
    }
    catch (Exception ex)
    {
        // Yahan aap apna logger use kar sakte hain, jaise: _logger.LogError(ex, "Error occurred in GetAllPagedAsync");
        // Abhi ke liye ye exception ko properly details ke sath re-throw karega taaki controller use safely capture kar sake
        throw new ApplicationException("An error occurred while fetching paged news data.", ex);
    }
}
            
            
            
            
            
            
            
   }
}