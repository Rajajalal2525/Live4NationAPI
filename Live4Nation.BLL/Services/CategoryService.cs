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
                ParentId = category.ParentId,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
            };
        }

        private static void SortTree(ICollection<CategoryDto> categories)
        {
            foreach (var category in categories)
            {
                SortTree(category.SubCategories);
            }

            var sortedCategories = categories
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            categories.Clear();

            foreach (var category in sortedCategories)
            {
                categories.Add(category);
            }
        }

        private static List<CategoryDto> BuildCategoryTree(IEnumerable<Category> categories)
        {
            var categoryMap = categories.ToDictionary(
                category => category.Id,
                category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug,
                    DisplayOrder = category.DisplayOrder,
                    IsActive = category.IsActive,
                    ParentId = category.ParentId,
                    CreatedDate = category.CreatedDate,
                    UpdatedDate = category.UpdatedDate
                });

            foreach (var category in categories)
            {
                if (category.ParentId.HasValue && categoryMap.TryGetValue(category.ParentId.Value, out var parentCategory))
                {
                    parentCategory.SubCategories.Add(categoryMap[category.Id]);
                }
            }

            var rootCategories = categoryMap.Values
                .Where(category => category.ParentId == null)
                .ToList();

            SortTree(rootCategories);
            return rootCategories;
        }

        private static IEnumerable<CategoryDto> FlattenTree(CategoryDto category)
        {
            yield return category;

            foreach (var subCategory in category.SubCategories)
            {
                foreach (var nestedCategory in FlattenTree(subCategory))
                {
                    yield return nestedCategory;
                }
            }
        }

        private static bool WouldCreateCycle(Dictionary<int, int?> parentMap, int categoryId, int? parentId)
        {
            var currentParentId = parentId;

            while (currentParentId.HasValue)
            {
                if (currentParentId.Value == categoryId)
                {
                    return true;
                }

                if (!parentMap.TryGetValue(currentParentId.Value, out currentParentId))
                {
                    break;
                }
            }

            return false;
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
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return BuildCategoryTree(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            var tree = BuildCategoryTree(categories);

            return tree
                .SelectMany(FlattenTree)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateUpdateDto dto)
        {
            if (dto.ParentId.HasValue)
            {
                var parentExists = await _context.Categories.AnyAsync(x => x.Id == dto.ParentId.Value);
                if (!parentExists)
                {
                    throw new ArgumentException($"Parent category with ID {dto.ParentId.Value} not found.");
                }
            }

            var category = new Category
            {
                Name = dto.Name.Trim(),
                Slug = string.IsNullOrWhiteSpace(dto.Slug) ? BuildSlug(dto.Name) : dto.Slug.Trim(),
                ParentId = dto.ParentId,
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

            if (dto.ParentId.HasValue)
            {
                if (dto.ParentId.Value == id)
                {
                    throw new ArgumentException("A category cannot be its own parent.");
                }

                var categoryGraph = await _context.Categories
                    .AsNoTracking()
                    .Select(x => new { x.Id, x.ParentId })
                    .ToListAsync();

                var parentMap = categoryGraph.ToDictionary(x => x.Id, x => x.ParentId);

                if (!parentMap.ContainsKey(dto.ParentId.Value))
                {
                    throw new ArgumentException($"Parent category with ID {dto.ParentId.Value} not found.");
                }

                if (WouldCreateCycle(parentMap, id, dto.ParentId))
                {
                    throw new ArgumentException("The selected parent category would create a circular hierarchy.");
                }
            }

            category.Name = dto.Name.Trim();
            category.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? BuildSlug(dto.Name) : dto.Slug.Trim();
            category.ParentId = dto.ParentId;
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

            var hasChildCategories = await _context.Categories.AnyAsync(x => x.ParentId == id);
            if (hasChildCategories)
            {
                throw new InvalidOperationException("Cannot delete a category that has subcategories.");
            }

            var hasNews = await _context.News.AnyAsync(x => x.CategoryId == id);
            if (hasNews)
            {
                throw new InvalidOperationException("Cannot delete a category that is used by news articles.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}