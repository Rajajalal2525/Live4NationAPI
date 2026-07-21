namespace Live4Nation.DAL.Entities
{
    public class WebsiteSetting
    {
        public int Id { get; set; }

        public string WebsiteName { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;

        public string? Favicon { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        public string? Twitter { get; set; }

        public string? YouTube { get; set; }

        public string? FooterText { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}