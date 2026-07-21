using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

         private readonly INewsService _newsService;

        public HomeController(IHomeService homeService, INewsService newsService)
        {
            _homeService = homeService;
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHome([FromQuery] int sectionTake = 10, [FromQuery] int categoryTake = 5)
        {
            if (sectionTake <= 0 || sectionTake > 50)
            {
                return BadRequest(new { message = "sectionTake must be between 1 and 50." });
            }

            if (categoryTake <= 0 || categoryTake > 20)
            {
                return BadRequest(new { message = "categoryTake must be between 1 and 20." });
            }

            var result = await _homeService.GetHomeAsync(sectionTake, categoryTake);
            return Ok(result);
        }

        
        // Home Page Specific Endpoint

         [HttpGet("{id:int}/page")]
public async Task<IActionResult> GetNewsDetailPage(int id)
{
    var result = await _newsService.GetDetailPageAsync(id);

    if (result == null)
    {
        return NotFound(new { message = $"News with ID {id} not found." });
    }

    return Ok(result);
}

    }
}