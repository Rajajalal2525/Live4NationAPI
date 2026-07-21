using System.ComponentModel.DataAnnotations;

namespace Live4Nation.BLL.DTOs
{
    public class VideoCreateUpdateDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ThumbnailImage { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}