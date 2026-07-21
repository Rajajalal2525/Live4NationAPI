using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class VideoService : IVideoService
    {
        private readonly ApplicationDbContext _context;

        public VideoService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static VideoDto MapToDto(Video entity)
        {
            return new VideoDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ThumbnailImage = entity.ThumbnailImage,
                VideoUrl = entity.VideoUrl,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        public async Task<List<VideoDto>> GetAllAsync()
        {
            return await _context.Videos
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new VideoDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ThumbnailImage = x.ThumbnailImage,
                    VideoUrl = x.VideoUrl,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<VideoDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Videos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : MapToDto(entity);
        }

        public async Task<VideoDto> CreateAsync(VideoCreateUpdateDto dto)
        {
            var entity = new Video
            {
                Title = dto.Title.Trim(),
                ThumbnailImage = dto.ThumbnailImage.Trim(),
                VideoUrl = dto.VideoUrl.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _context.Videos.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(int id, VideoCreateUpdateDto dto)
        {
            var entity = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.Title = dto.Title.Trim();
            entity.ThumbnailImage = dto.ThumbnailImage.Trim();
            entity.VideoUrl = dto.VideoUrl.Trim();
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _context.Videos.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}