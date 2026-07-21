using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class HomeService : IHomeService
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly IGalleryService _galleryService;
        private readonly IVideoService _videoService;
        private readonly IAdvertisementService _advertisementService;
        private readonly ApplicationDbContext _context;

        public HomeService(
            INewsService newsService,
            ICategoryService categoryService,
            IGalleryService galleryService,
            IVideoService videoService,
            IAdvertisementService advertisementService,
            ApplicationDbContext context)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _galleryService = galleryService;
            _videoService = videoService;
            _advertisementService = advertisementService;
            _context = context;
        }

      public async Task<HomeResponseDto> GetHomeAsync(int sectionTake = 10, int categoryTake = 5)
        {
            sectionTake = sectionTake <= 0 ? 10 : sectionTake;
            categoryTake = categoryTake <= 0 ? 5 : categoryTake;

            // 1. Sequential awaits: Har ek method ko line-by-line await kiya taaki DbContext thread crash na ho.
            var latestNewsData = await _newsService.GetLatestAsync(sectionTake * 2);
            var breakingNewsData = await _newsService.GetBreakingAsync(sectionTake * 2);
            var trendingNewsData = await _newsService.GetTrendingAsync(sectionTake * 2);
            var featuredNewsData = await _newsService.GetFeaturedAsync(sectionTake * 2);
            var topStoryNewsData = await _newsService.GetTopStoryAsync(sectionTake * 2);

            var categoriesData = await _categoryService.GetAllAsync();
            var galleriesData = await _galleryService.GetAllAsync();
            var videosData = await _videoService.GetAllAsync();
            var advertisementsData = await _advertisementService.GetAllAsync();

            var footerData = await _context.WebsiteSettings
                .AsNoTracking()
                .OrderByDescending(x => x.UpdatedDate)
                .FirstOrDefaultAsync();

            // 2. In-memory filter aur data extraction (.Result ko completely hata diya hai)
            var latestNews = latestNewsData
                .Where(x => x.IsActive)
                .Take(sectionTake)
                .ToList();

            var breakingNews = breakingNewsData
                .Where(x => x.IsActive)
                .Take(sectionTake)
                .ToList();

            var trendingNews = trendingNewsData
                .Where(x => x.IsActive)
                .Take(sectionTake)
                .ToList();

            var featuredNews = featuredNewsData
                .Where(x => x.IsActive)
                .Take(sectionTake)
                .ToList();

            var topStories = topStoryNewsData
                .Where(x => x.IsActive)
                .Take(sectionTake)
                .ToList();

            var heroTopStory = topStories.FirstOrDefault() ?? latestNews.FirstOrDefault();

            var menu = categoriesData
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToList();

            var categoryWiseNews = await BuildCategoryWiseNewsAsync(menu, categoryTake);

            var latestGallery = galleriesData
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .Take(sectionTake)
                .ToList();

            var latestVideos = videosData
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .Take(sectionTake)
                .ToList();

            var advertisements = BuildAdvertisements(advertisementsData);

            var footer = footerData == null ? null : MapFooter(footerData);

            return new HomeResponseDto
            {
                HeroTopStory = heroTopStory,
                BreakingNews = breakingNews,
                LatestNews = latestNews,
                TrendingNews = trendingNews,
                FeaturedNews = featuredNews,
                TopStories = topStories,
                CategoryWiseNews = categoryWiseNews,
                LatestGallery = latestGallery,
                LatestVideos = latestVideos,
                Advertisements = advertisements,
                Menu = menu,
                Footer = footer
            };
        }

        private async Task<List<HomeCategoryNewsDto>> BuildCategoryWiseNewsAsync(List<CategoryDto> activeCategories, int categoryTake)
        {
            var categoryIds = activeCategories.Select(x => x.Id).ToList();

            if (categoryIds.Count == 0)
            {
                return new List<HomeCategoryNewsDto>();
            }

            var allCategoryNews = await _context.News
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.IsActive && categoryIds.Contains(x.CategoryId))
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

            var result = new List<HomeCategoryNewsDto>();

            foreach (var category in activeCategories)
            {
                var items = allCategoryNews
                    .Where(x => x.CategoryId == category.Id)
                    .Take(categoryTake)
                    .ToList();

                if (items.Count == 0)
                {
                    continue;
                }

                result.Add(new HomeCategoryNewsDto
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    CategorySlug = category.Slug,
                    News = items
                });
            }

            return result;
        }

        private static HomeAdvertisementsDto BuildAdvertisements(List<AdvertisementDto> allAdvertisements)
        {
            var activeAds = allAdvertisements
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return new HomeAdvertisementsDto
            {
                Top = activeAds.Where(x => IsPosition(x.Position, "Top")).ToList(),
                Sidebar = activeAds.Where(x => IsPosition(x.Position, "Sidebar")).ToList(),
                Middle = activeAds.Where(x => IsPosition(x.Position, "Middle")).ToList(),
                Footer = activeAds.Where(x => IsPosition(x.Position, "Footer") || IsPosition(x.Position, "Bottom")).ToList(),
                Other = activeAds.Where(x =>
                    !IsPosition(x.Position, "Top") &&
                    !IsPosition(x.Position, "Sidebar") &&
                    !IsPosition(x.Position, "Middle") &&
                    !IsPosition(x.Position, "Footer") &&
                    !IsPosition(x.Position, "Bottom"))
                    .ToList()
            };
        }

        private static bool IsPosition(string value, string expected)
        {
            return string.Equals(value?.Trim(), expected, StringComparison.OrdinalIgnoreCase);
        }

        private static HomeFooterDto MapFooter(WebsiteSetting entity)
        {
            return new HomeFooterDto
            {
                WebsiteName = entity.WebsiteName,
                Logo = entity.Logo,
                Favicon = entity.Favicon,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address,
                Facebook = entity.Facebook,
                Instagram = entity.Instagram,
                Twitter = entity.Twitter,
                YouTube = entity.YouTube,
                FooterText = entity.FooterText,
                UpdatedDate = entity.UpdatedDate
            };
        }
    }
}