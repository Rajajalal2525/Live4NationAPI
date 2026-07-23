using System.ComponentModel.DataAnnotations;

namespace Live4Nation.BLL.DTOs
{
    public class NewsImageCreateDto
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Caption { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }
}