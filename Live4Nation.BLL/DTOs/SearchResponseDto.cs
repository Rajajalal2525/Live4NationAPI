namespace Live4Nation.BLL.DTOs
{
    public class SearchResponseDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string? Author { get; set; }
        public string? Location { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsBreaking { get; set; }
        public bool IsTrending { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsTopStory { get; set; }
        public long ViewCount { get; set; }
    }
}