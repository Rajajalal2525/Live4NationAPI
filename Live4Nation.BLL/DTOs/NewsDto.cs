namespace Live4Nation.BLL.DTOs
{
    public class NewsDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;

        public ICollection<NewsImageDto> NewsImages { get; set; } = new List<NewsImageDto>();

        public string? Author { get; set; }
        public string? Location { get; set; }

        public DateTime PublishedDate { get; set; }

        public bool IsBreaking { get; set; }
        public bool IsTrending { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsTopStory { get; set; }

        public int Priority { get; set; }
        public long ViewCount { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}