using System.ComponentModel.DataAnnotations;

namespace Live4Nation.BLL.DTOs
{
    public class AdvertisementCreateUpdateDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(500)]
        public string? RedirectUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}