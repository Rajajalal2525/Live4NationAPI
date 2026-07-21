namespace Live4Nation.BLL.DTOs
{
    public class NewsNavigationResponseDto
    {
        public SearchResponseDto? PreviousNews { get; set; }
        public SearchResponseDto? NextNews { get; set; }
    }
}