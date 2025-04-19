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
    [Authorize]
    public class DealsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DealsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Deals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deal>>> GetDeals()
        {
            var deals = await _context.Deals.ToListAsync();
            return Ok(deals);
        }

        // GET: api/Deals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Deal>> GetDeal(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                return NotFound();
            }

            return Ok(deal);
        }

        // POST: api/Deals
        [HttpPost]
        public async Task<IActionResult> CreateDeal(DealDTO dealDto)
        {
            if (dealDto == null)
                return BadRequest("Invalid data.");

            // Lấy Product từ cơ sở dữ liệu dựa trên productId
            var product = await _context.Products.FindAsync(dealDto.ProductId);

            if (product == null)
                return NotFound("Product not found.");

            // Tạo Deal từ thông tin nhận được
            var deal = new Deal
            {
                ProductId = dealDto.ProductId,
                DiscountPercent = dealDto.DiscountPercent,
                StartDate = dealDto.StartDate,
                EndDate = dealDto.EndDate,
                Product = product // Gán product đã tìm được vào deal
            };

            // Lưu Deal vào cơ sở dữ liệu
            _context.Deals.Add(deal);

            try
            {
                await _context.SaveChangesAsync(); // Lưu vào cơ sở dữ liệu
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok("Deal created successfully.");
        }
        // PUT: api/Deals/5
        // PUT: api/Deals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeal(int id, [FromBody] DealDTO dealDto)
        {
            if (dealDto == null)
                return BadRequest("Invalid data.");

            var existingDeal = await _context.Deals.FindAsync(id);
            if (existingDeal == null)
                return NotFound("Deal not found.");

            // Kiểm tra ProductId mới có hợp lệ không (nếu có thay đổi)
            var product = await _context.Products.FindAsync(dealDto.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            // Cập nhật thông tin deal
            existingDeal.ProductId = dealDto.ProductId;
            existingDeal.DiscountPercent = dealDto.DiscountPercent;
            existingDeal.StartDate = dealDto.StartDate;
            existingDeal.EndDate = dealDto.EndDate;
            existingDeal.Product = product;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok("Deal updated successfully.");
        }


        // DELETE: api/Deals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeal(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
            {
                return NotFound();
            }

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DealExists(int id)
        {
            return _context.Deals.Any(e => e.Id == id);
        }
    }

}
