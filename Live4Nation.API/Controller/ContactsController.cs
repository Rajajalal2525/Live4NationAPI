using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactCreateDto dto)
        {
            var created = await _contactService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactService.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _contactService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Contact not found." });
            }

            return Ok(new { message = "Contact deleted successfully." });
        }
    }
}