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
    }
}