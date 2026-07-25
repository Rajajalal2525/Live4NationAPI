namespace Live4Nation.BLL.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public List<CategoryDto> SubCategories { get; set; } = new();
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}