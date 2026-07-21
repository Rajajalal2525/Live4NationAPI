using Live4Nation.BLL.DTOs;
using Live4Nation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Live4Nation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;

        public NewsController(ICategoryService categoryService, INewsService newsService)
        {
            _categoryService = categoryService;
            _newsService = newsService;
        }

        #region Category Endpoints

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllAsync();
            
            // Agar list khaali hai to [] ki jagah 404 message jayega
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No categories found." });
            }
            
            return Ok(result);
        }

        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return Ok(result);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Category data cannot be null." });
            }

            var created = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, created);
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateUpdateDto dto)
        {
            var updated = await _categoryService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new { message = $"Category with ID {id} not found to update." });
            }

            return Ok(new { message = "Category updated successfully." });
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return Ok(new { message = "Category deleted successfully." });
        }

        #endregion

        #region News Core Endpoints

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            var result = await _newsService.GetAllAsync();
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No news articles found." });
            }
            
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var result = await _newsService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = $"News with ID {id} not found." });
            }

            return Ok(result);
        }

        [HttpGet("{id:int}/details")]
        public async Task<IActionResult> GetNewsDetails(int id)
        {
            var result = await _newsService.GetDetailsByIdAsync(id);

            if (result == null)
            {
                return NotFound(new { message = $"Details for News ID {id} not found." });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] NewsCreateUpdateDto dto)
        {
            var created = await _newsService.CreateAsync(dto);

            if (created == null)
            {
                return BadRequest(new { message = "Failed to create news. Please check if the category ID is valid." });
            }

            return CreatedAtAction(nameof(GetNewsById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsCreateUpdateDto dto)
        {
            var updated = await _newsService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new { message = $"News with ID {id} not found or category ID is invalid." });
            }

            return Ok(new { message = "News updated successfully." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var deleted = await _newsService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"News with ID {id} not found." });
            }

            return Ok(new { message = "News deleted successfully." });
        }

        #endregion

        #region News Special Lists Endpoints (Filters)

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestNews([FromQuery] int take = 10)
        {
            var result = await _newsService.GetLatestAsync(take);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No latest news found." });
            }
            
            return Ok(result);
        }

        [HttpGet("breaking")]
        public async Task<IActionResult> GetBreakingNews([FromQuery] int take = 10)
        {
            var result = await _newsService.GetBreakingAsync(take);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No breaking news found." });
            }
            
            return Ok(result);
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingNews([FromQuery] int take = 10)
        {
            var result = await _newsService.GetTrendingAsync(take);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No trending news found." });
            }
            
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedNews([FromQuery] int take = 10)
        {
            var result = await _newsService.GetFeaturedAsync(take);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No featured news found." });
            }
            
            return Ok(result);
        }

        [HttpGet("top-story")]
        public async Task<IActionResult> GetTopStoryNews([FromQuery] int take = 10)
        {
            var result = await _newsService.GetTopStoryAsync(take);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = "No top stories found." });
            }
            
            return Ok(result);
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<IActionResult> GetNewsByCategory(int categoryId)
        {
            var result = await _newsService.GetByCategoryAsync(categoryId);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = $"No news found for Category ID {categoryId}." });
            }
            
            return Ok(result);
        }

        #endregion

        #region News Images Endpoints

        [HttpPost("{newsId:int}/images")]
        public async Task<IActionResult> AddImage(int newsId, [FromBody] NewsImageCreateDto dto)
        {
            var created = await _newsService.AddImageAsync(newsId, dto);

            if (created == null)
            {
                return NotFound(new { message = $"Failed to add image. News with ID {newsId} not found." });
            }

            return Ok(created);
        }

        [HttpGet("{newsId:int}/images")]
        public async Task<IActionResult> GetImagesByNewsId(int newsId)
        {
            var result = await _newsService.GetImagesByNewsIdAsync(newsId);
            
            if (result == null || !result.Any())
            {
                return NotFound(new { message = $"No images found for News ID {newsId}." });
            }
            
            return Ok(result);
        }

        [HttpDelete("images/{imageId:int}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var deleted = await _newsService.DeleteImageAsync(imageId);

            if (!deleted)
            {
                return NotFound(new { message = $"Image with ID {imageId} not found." });
            }

            return Ok(new { message = "Image deleted successfully." });
        }

        #endregion

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