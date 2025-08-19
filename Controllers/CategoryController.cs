
using ExpenseTracker.Data;
using ExpenseTracker.Filters;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET : api/category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.OrderBy(c => c.Title).ToListAsync();
            return Ok(categories);
        }

        // POST : api/category
        [HttpPost]
        [ValidateUniqueCategory]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // PUT : api/category/1
        [HttpPut("{id}")]
        [ValidateUpdateCategory]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);

            existingCategory!.Title = category.Title;
            existingCategory.Icon = category.Icon;
            existingCategory.Type = category.Type;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE : api/category/1
        [HttpDelete("{id}")]
        [ValidateCategoryId]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            _context.Categories.Remove(category!);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}