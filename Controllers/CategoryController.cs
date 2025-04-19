using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TranThienTrung2122110179.Data;
    using TranThienTrung2122110179.DTO;
    using TranThienTrung2122110179.Model;

    namespace TranThienTrung2122110179.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]   
    //[Authorize]

    public class CategoryController : ControllerBase
        {
            private readonly AppDbContext _context;

            public CategoryController(AppDbContext context)
            {
                _context = context;
            }

            // GET: api/Category
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
            {
                return await _context.Categories
                    .Include(c => c.Products)
                    .ToListAsync();
            }

            // GET: api/Category/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Category>> GetCategory(int id)
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return NotFound();

                return category;
            }

            // POST: api/Category
            [HttpPost]
            public async Task<ActionResult<Category>> CreateCategory(CategoryDto dto)
            {
                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    CreatedBy = dto.CreatedBy,
                    CreatedAt = DateTime.Now
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }


            // PUT: api/Category/5
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound();

                category.Name = dto.Name;
                category.Description = dto.Description;
                category.CreatedBy = dto.CreatedBy;
                category.CreatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return NoContent();
            }


            // DELETE: api/Category/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCategory(int id)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return NotFound();

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
