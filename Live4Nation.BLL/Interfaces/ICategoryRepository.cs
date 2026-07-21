using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface ICategoryService
    {
       Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CategoryCreateUpdateDto dto);
        Task<bool> UpdateAsync(int id, CategoryCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}