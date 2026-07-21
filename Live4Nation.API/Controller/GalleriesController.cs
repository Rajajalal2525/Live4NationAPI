using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GalleriesController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleriesController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _galleryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _galleryService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = "Gallery not found." });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GalleryCreateUpdateDto dto)
        {
            var created = await _galleryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GalleryCreateUpdateDto dto)
        {
            var updated = await _galleryService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new { message = "Gallery not found." });
            }

            return Ok(new { message = "Gallery updated successfully." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _galleryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Gallery not found." });
            }

            return Ok(new { message = "Gallery deleted successfully." });
        }
    }
}