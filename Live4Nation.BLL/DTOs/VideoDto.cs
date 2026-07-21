namespace Live4Nation.BLL.DTOs
{
    public class VideoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}