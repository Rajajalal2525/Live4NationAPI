using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementsController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _advertisementService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _advertisementService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = "Advertisement not found." });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdvertisementCreateUpdateDto dto)
        {
            var created = await _advertisementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdvertisementCreateUpdateDto dto)
        {
            var updated = await _advertisementService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new { message = "Advertisement not found." });
            }

            return Ok(new { message = "Advertisement updated successfully." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _advertisementService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Advertisement not found." });
            }

            return Ok(new { message = "Advertisement deleted successfully." });
        }
        
    }
}