using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface IContactService
    {
        Task<ContactDto> CreateAsync(ContactCreateDto dto);
        Task<List<ContactDto>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}