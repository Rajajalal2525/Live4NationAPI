namespace Live4Nation.BLL.DTOs
{
    public class NewsDetailsDto
    {
        public NewsDto News { get; set; } = new();
        public List<NewsImageDto> Images { get; set; } = new();
    }
}