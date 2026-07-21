using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _videoService.GetAllAsync();
            
            // Agar videos ki list khaali hai ya null hai to proper message jayega
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No videos found ." });
            }
            
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _videoService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = $"Video with ID {id} not found." });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VideoCreateUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Video data cannot be null." });
            }

            var created = await _videoService.CreateAsync(dto);
            
            if (created == null)
            {
                return BadRequest(new { message = "Failed to create video. Please check your data." });
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VideoCreateUpdateDto dto)
        {
            var updated = await _videoService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new { message = $"Video with ID {id} not found to update." });
            }

            return Ok(new { message = "Video updated successfully." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _videoService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Video with ID {id} not found to delete." });
            }

            return Ok(new { message = "Video deleted successfully." });
        }
    }
}