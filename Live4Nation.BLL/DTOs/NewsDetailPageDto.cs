namespace Live4Nation.BLL.DTOs
{
    public class NewsDetailPageDto
    {
        public NewsDto News { get; set; } = new();
        public List<NewsImageDto> Images { get; set; } = new();
        public List<NewsDto> RelatedNews { get; set; } = new();
        public List<NewsDto> LatestNews { get; set; } = new();
        public List<NewsDto> TrendingNews { get; set; } = new();
        public List<NewsDto> BreakingNews { get; set; } = new();
        public NewsDto? PreviousNews { get; set; }
        public NewsDto? NextNews { get; set; }
        public NewsDetailPageAdvertisementsDto Advertisements { get; set; } = new();
    }

    public class NewsDetailPageAdvertisementsDto
    {
        public List<AdvertisementDto> Top { get; set; } = new();
        public List<AdvertisementDto> Middle { get; set; } = new();
        public List<AdvertisementDto> Sidebar { get; set; } = new();
        public List<AdvertisementDto> Bottom { get; set; } = new();
    }
}