using System.ComponentModel.DataAnnotations;

namespace Live4Nation.BLL.DTOs
{
    public class NewsImageCreateDto
    {
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Caption { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }
}