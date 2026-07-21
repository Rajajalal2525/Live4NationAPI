namespace Live4Nation.BLL.DTOs
{
    public class NewsImageDto
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}