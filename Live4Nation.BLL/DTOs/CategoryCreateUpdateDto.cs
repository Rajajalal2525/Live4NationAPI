using System.ComponentModel.DataAnnotations;

namespace Live4Nation.BLL.DTOs
{
    public class CategoryCreateUpdateDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(180)]
        public string? Slug { get; set; }

        public int? ParentId { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}