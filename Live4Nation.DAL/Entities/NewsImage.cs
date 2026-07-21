namespace Live4Nation.DAL.Entities
{
    public class NewsImage
    {
        public int Id { get; set; }

        public int NewsId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string? Caption { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public News News { get; set; } = null!;
    }
}