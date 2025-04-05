using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThienTrung2122110179.Data;
using TranThienTrung2122110179.Model;

namespace TranThienTrung2122110179.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Thêm sản phẩm mới
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            // Kiểm tra nếu sản phẩm đã tồn tại
            if (_context.Products.Any(p => p.Name == product.Name))
            {
                return Conflict("Sản phẩm đã tồn tại.");
            }

            // Thêm sản phẩm vào database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Trả về thông tin sản phẩm với CategoryId mà không cần lấy dữ liệu Category
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // Sửa sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID sản phẩm không khớp.");
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;  // Chỉ cập nhật CategoryId
            existingProduct.Brand = product.Brand;
            existingProduct.IsActive = product.IsActive;
            existingProduct.UpdatedBy = product.UpdatedBy;
            existingProduct.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Lấy thông tin sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            return product;  // Trả về thông tin sản phẩm chỉ với CategoryId
        }

        // Lấy danh sách tất cả sản phẩm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();  // Trả về tất cả sản phẩm mà không cần thông tin về Category
        }
    }
}
