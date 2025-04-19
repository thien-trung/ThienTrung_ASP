using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Chuyển đổi từ Product sang ProductDTO
        private ProductDTO ConvertToDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Brand = product.Brand,
                IsActive = product.IsActive
            };
        }

        // Thêm sản phẩm mới
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDto)
        {
            // Kiểm tra nếu sản phẩm đã tồn tại
            if (_context.Products.Any(p => p.Name == productDto.Name))
            {
                return Conflict("Sản phẩm đã tồn tại.");
            }

            // Chuyển từ DTO sang Entity Product
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                CategoryId = productDto.CategoryId,
                Brand = productDto.Brand,
                IsActive = productDto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = productDto.CreatedBy
            };

            // Thêm sản phẩm vào database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Chuyển đổi Entity sang DTO và trả về
            var createdProductDto = ConvertToDTO(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, createdProductDto);
        }

        // Sửa sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("ID sản phẩm không khớp.");
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            // Cập nhật thông tin sản phẩm từ DTO
            existingProduct.Name = productDto.Name;
            existingProduct.Price = productDto.Price;
            existingProduct.Stock = productDto.Stock;
            existingProduct.Description = productDto.Description;
            existingProduct.ImageUrl = productDto.ImageUrl;
            existingProduct.CategoryId = productDto.CategoryId;
            existingProduct.Brand = productDto.Brand;
            existingProduct.IsActive = productDto.IsActive;
            existingProduct.CreatedAt = DateTime.Now;
            existingProduct.CreatedBy = productDto.CreatedBy;

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
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            // Chuyển đổi Entity sang DTO
            var productDto = ConvertToDTO(product);
            return Ok(productDto);  // Trả về ProductDTO
        }

        // Lấy danh sách tất cả sản phẩm
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        //{
        //    var products = await _context.Products.ToListAsync();
        //    var productDtos = products.Select(p => ConvertToDTO(p)).ToList();
        //    return Ok(productDtos);  // Trả về danh sách ProductDTO
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Số trang và kích thước trang phải lớn hơn 0.");
            }

            var totalProducts = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productDtos = products.Select(p => ConvertToDTO(p)).ToList();

            var paginationMetadata = new
            {
                totalCount = totalProducts,
                pageSize = pageSize,
                currentPage = pageNumber,
                totalPages = totalPages
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(productDtos); // Trả về danh sách ProductDTO và thông tin phân trang trong Header
        }

    }
}
