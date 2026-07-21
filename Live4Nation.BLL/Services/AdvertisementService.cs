using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationDbContext _context;

        public AdvertisementService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static AdvertisementDto MapToDto(Advertisement entity)
        {
            return new AdvertisementDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = entity.ImageUrl,
                RedirectUrl = entity.RedirectUrl,
                Position = entity.Position,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        public async Task<List<AdvertisementDto>> GetAllAsync()
        {
            return await _context.Advertisements
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new AdvertisementDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    RedirectUrl = x.RedirectUrl,
                    Position = x.Position,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<AdvertisementDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Advertisements
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : MapToDto(entity);
        }

        public async Task<AdvertisementDto> CreateAsync(AdvertisementCreateUpdateDto dto)
        {
            var entity = new Advertisement
            {
                Title = dto.Title.Trim(),
                ImageUrl = dto.ImageUrl.Trim(),
                RedirectUrl = string.IsNullOrWhiteSpace(dto.RedirectUrl) ? null : dto.RedirectUrl.Trim(),
                Position = dto.Position.Trim(),
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _context.Advertisements.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(int id, AdvertisementCreateUpdateDto dto)
        {
            var entity = await _context.Advertisements.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.Title = dto.Title.Trim();
            entity.ImageUrl = dto.ImageUrl.Trim();
            entity.RedirectUrl = string.IsNullOrWhiteSpace(dto.RedirectUrl) ? null : dto.RedirectUrl.Trim();
            entity.Position = dto.Position.Trim();
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Advertisements.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _context.Advertisements.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}