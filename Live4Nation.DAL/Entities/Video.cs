namespace Live4Nation.DAL.Entities
{
    public class Video
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ThumbnailImage { get; set; } = string.Empty;

        public string VideoUrl { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}