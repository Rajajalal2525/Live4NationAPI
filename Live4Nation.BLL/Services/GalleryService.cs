using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly ApplicationDbContext _context;

        public GalleryService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static GalleryDto MapToDto(Gallery entity)
        {
            return new GalleryDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        public async Task<List<GalleryDto>> GetAllAsync()
        {
            return await _context.Galleries
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new GalleryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<GalleryDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Galleries
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : MapToDto(entity);
        }

        public async Task<GalleryDto> CreateAsync(GalleryCreateUpdateDto dto)
        {
            var entity = new Gallery
            {
                Title = dto.Title.Trim(),
                ImageUrl = dto.ImageUrl.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _context.Galleries.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(int id, GalleryCreateUpdateDto dto)
        {
            var entity = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.Title = dto.Title.Trim();
            entity.ImageUrl = dto.ImageUrl.Trim();
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _context.Galleries.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}