using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN232.LaptopShop.Service;
using PRN232.LaptopShop.Service.DTO;

namespace PRN232.LaptopShop.API.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService categoryService;
        public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(int pageIndex = 1, int pageSize = 10)
        {
            var categories = await categoryService.GetAllCategories(pageIndex, pageSize);
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest category)
        {
            var newCategory = await categoryService.AddCategory(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.CategoryId }, newCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest category)
        {
            var updatedCategory = await categoryService.UpdateCategory(id,category);
            if (updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await categoryService.DeleteCategory(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
