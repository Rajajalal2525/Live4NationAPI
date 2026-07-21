using System;
using System.Collections.Generic;

namespace Live4Nation.BLL.DTOs
{
    public class HomeResponseDto
    {
        public NewsDto? HeroTopStory { get; set; }

        public List<NewsDto> BreakingNews { get; set; } = new();
        public List<NewsDto> LatestNews { get; set; } = new();
        public List<NewsDto> TrendingNews { get; set; } = new();
        public List<NewsDto> FeaturedNews { get; set; } = new();
        public List<NewsDto> TopStories { get; set; } = new();

        public List<HomeCategoryNewsDto> CategoryWiseNews { get; set; } = new();

        public List<GalleryDto> LatestGallery { get; set; } = new();
        public List<VideoDto> LatestVideos { get; set; } = new();

        public HomeAdvertisementsDto Advertisements { get; set; } = new();

        public List<CategoryDto> Menu { get; set; } = new();

        public HomeFooterDto? Footer { get; set; }
    }

    public class HomeCategoryNewsDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public List<NewsDto> News { get; set; } = new();
    }

    public class HomeAdvertisementsDto
    {
        public List<AdvertisementDto> Top { get; set; } = new();
        public List<AdvertisementDto> Sidebar { get; set; } = new();
        public List<AdvertisementDto> Middle { get; set; } = new();
        public List<AdvertisementDto> Footer { get; set; } = new();
        public List<AdvertisementDto> Other { get; set; } = new();
    }

    public class HomeFooterDto
    {
        public string WebsiteName { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string? Favicon { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? YouTube { get; set; }
        public string? FooterText { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}