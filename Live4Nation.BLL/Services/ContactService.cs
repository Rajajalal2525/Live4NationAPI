using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Live4Nation.DAL.Data;
using Live4Nation.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Live4Nation.BLL.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static ContactDto MapToDto(Contact entity)
        {
            return new ContactDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Mobile = entity.Mobile,
                Subject = entity.Subject,
                Message = entity.Message,
                IsRead = entity.IsRead,
                CreatedDate = entity.CreatedDate
            };
        }

        public async Task<ContactDto> CreateAsync(ContactCreateDto dto)
        {
            var entity = new Contact
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                Mobile = string.IsNullOrWhiteSpace(dto.Mobile) ? null : dto.Mobile.Trim(),
                Subject = dto.Subject.Trim(),
                Message = dto.Message.Trim(),
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.Contacts.Add(entity);
            await _context.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<List<ContactDto>> GetAllAsync()
        {
            return await _context.Contacts
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new ContactDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    Subject = x.Subject,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _context.Contacts.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}