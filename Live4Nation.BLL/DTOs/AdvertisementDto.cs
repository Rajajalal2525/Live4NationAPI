namespace Live4Nation.BLL.DTOs
{
    public class AdvertisementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? RedirectUrl { get; set; }
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}