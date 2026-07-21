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

        [HttpGet("search")]
        public async Task<IActionResult> SearchNews([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Keyword is required." });
            }

            var result = await _newsService.SearchAsync(keyword, page, pageSize);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNewsListing([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null, [FromQuery] string? location = null, [FromQuery] string? search = null, [FromQuery] bool? isBreaking = null, [FromQuery] bool? isTrending = null, [FromQuery] bool? isFeatured = null, [FromQuery] bool? isTopStory = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] string? sortBy = null)
        {
            try
            {
                var result = await _newsService.GetAllPagedAsync(page, pageSize, categoryId, location, search, isBreaking, isTrending, isFeatured, isTopStory, fromDate, toDate, sortBy);

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                // Service layer se aane wali custom managed errors ke liye
                return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                // Kisi bhi unexpected system crash ke liye
                return StatusCode(500, new { message = "An unexpected error occurred on the server.", details = ex.Message });
            }
        }


        [HttpGet("{id:int}/related")]
        public async Task<IActionResult> GetRelatedNews(int id, [FromQuery] int take = 6)
        {
            try
            {
                var result = await _newsService.GetRelatedAsync(id, take);

                if (result == null)
                {
                    return NotFound(new { message = $"News with ID {id} not found." });
                }

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                // Service layer se aane wali custom/expected errors ke liye
                return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                // Kisi bhi unexpected system crash ya exception ke liye
                return StatusCode(500, new { message = "An unexpected error occurred while fetching related news.", details = ex.Message });
            }
        }


        [HttpGet("{id:int}/navigation")]
        public async Task<IActionResult> GetNewsNavigation(int id)
        {
            var result = await _newsService.GetNavigationAsync(id);

            if (result == null)
            {
                return NotFound(new { message = $"News with ID {id} not found." });
            }

            return Ok(result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularNews([FromQuery] int take = 10)
        {
            var result = await _newsService.GetPopularAsync(take);
            return Ok(result);
        }

        [HttpGet("location/{location}")]
        public async Task<IActionResult> GetNewsByLocation(string location, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return BadRequest(new { message = "Location is required." });
            }

            var result = await _newsService.GetNewsByLocationAsync(location, page, pageSize);
            return Ok(result);
        }




    }
}