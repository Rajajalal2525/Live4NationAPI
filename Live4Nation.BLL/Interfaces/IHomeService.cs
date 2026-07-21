using Live4Nation.BLL.DTOs;

namespace Live4Nation.BLL.Interfaces
{
    public interface IHomeService
    {
        Task<HomeResponseDto> GetHomeAsync(int sectionTake = 10, int categoryTake = 5);
    }
}