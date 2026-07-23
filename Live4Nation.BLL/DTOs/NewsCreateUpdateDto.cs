using System.ComponentModel.DataAnnotations;

namespace Live4Nation.BLL.DTOs
{
    public class NewsCreateUpdateDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Slug { get; set; }

        [Required]
        [StringLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public ICollection<NewsImageCreateDto> NewsImages { get; set; } = new List<NewsImageCreateDto>();


        [StringLength(150)]
        public string? Author { get; set; }

        [StringLength(150)]
        public string? Location { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public bool IsBreaking { get; set; } = false;
        public bool IsTrending { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public bool IsTopStory { get; set; } = false;

        public int Priority { get; set; } = 0;
        public long ViewCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}