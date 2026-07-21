using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                DisplayOrder = category.DisplayOrder,
                IsActive = category.IsActive,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
            };
        }

        private static string BuildSlug(string name)
        {
            return string.Join("-", name
                .Trim()
                .ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return category == null ? null : MapToDto(category);
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateUpdateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name.Trim(),
                Slug = string.IsNullOrWhiteSpace(dto.Slug) ? BuildSlug(dto.Name) : dto.Slug.Trim(),
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return MapToDto(category);
        }

        public async Task<bool> UpdateAsync(int id, CategoryCreateUpdateDto dto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            category.Name = dto.Name.Trim();
            category.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? BuildSlug(dto.Name) : dto.Slug.Trim();
            category.DisplayOrder = dto.DisplayOrder;
            category.IsActive = dto.IsActive;
            category.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}