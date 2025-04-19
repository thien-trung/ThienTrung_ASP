using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThienTrung2122110179.Model;
using System.Linq;
using System.Threading.Tasks;
using TranThienTrung2122110179.Data;
using TranThienTrung2122110179.DTO.TranThienTrung2122110179.DTO;
using Microsoft.AspNetCore.Authorization;


namespace TranThienTrung2122110179.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class ShoppingCartsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShoppingCartsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        {
            var shoppingCarts = await _context.ShoppingCarts
                .Include(s => s.User)  // Bao gồm thông tin người dùng (nếu cần)
                .Include(s => s.Product)  // Bao gồm thông tin sản phẩm (nếu cần)
                .ToListAsync();

            return Ok(shoppingCarts);
        }

        // GET: api/ShoppingCarts/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetUserShoppingCart(int userId)
        {
            var shoppingCarts = await _context.ShoppingCarts
                .Where(s => s.UserId == userId)
                .Include(s => s.Product) // Include thông tin sản phẩm
                .ToListAsync();

            if (shoppingCarts == null || !shoppingCarts.Any())
            {
                return NotFound($"Không tìm thấy giỏ hàng cho người dùng có ID: {userId}");
            }

            return Ok(shoppingCarts);
        }

        // POST: api/ShoppingCarts
        [HttpPost]
        public async Task<IActionResult> CreateShoppingCart([FromBody] ShoppingCartDTO cartDto)
        {
            try
            {
                // Kiểm tra điều kiện số lượng
                if (cartDto.Quantity <= 0)
                {
                    return BadRequest("Số lượng sản phẩm phải lớn hơn 0.");
                }

                var user = await _context.Users.FindAsync(cartDto.UserId);
                var product = await _context.Products.FindAsync(cartDto.ProductId);

                if (user == null || product == null)
                {
                    return NotFound("User hoặc Product không tồn tại.");
                }

                var cart = new ShoppingCart
                {
                    UserId = cartDto.UserId,
                    ProductId = cartDto.ProductId,
                    Quantity = cartDto.Quantity,
                    TotalPrice = product.Price * cartDto.Quantity,
                    CreatedBy = cartDto.CreatedBy ?? "API",
                    CreatedAt = cartDto.CreatedAt ?? DateTime.Now
                };

                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();

                return Ok(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return BadRequest($"Lỗi khi lưu vào database: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoppingCart(int id, [FromBody] ShoppingCartDTO cartDto)
        {
            var existingCart = await _context.ShoppingCarts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCart == null)
            {
                return NotFound("ShoppingCart không tồn tại.");
            }

            var product = await _context.Products.FindAsync(cartDto.ProductId);
            if (product == null)
            {
                return NotFound("Product không tồn tại.");
            }

            existingCart.UserId = cartDto.UserId;
            existingCart.ProductId = cartDto.ProductId;
            existingCart.Quantity = cartDto.Quantity;
            existingCart.TotalPrice = product.Price * cartDto.Quantity;


            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi cập nhật: {ex.Message}");
            }
        }


        // DELETE: api/ShoppingCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();

            return NoContent();  // Trả về HTTP 204 khi xóa thành công
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCarts.Any(e => e.Id == id);
        }   
    }
}
