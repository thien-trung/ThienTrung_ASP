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

    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return Ok(stocks);
        }

        // GET: api/Stock/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock);
        }
        [HttpPost]
        public async Task<ActionResult<Stock>> CreateStock(StockDTO stockDto)
        {
            // Chuyển đổi từ DTO sang Entity
            var stock = new Stock
            {
                ProductId = stockDto.ProductId,
                Size = stockDto.Size,
                Color = stockDto.Color,
                Quantity = stockDto.Quantity
            };

            // Thêm stock mới vào cơ sở dữ liệu
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stock);
        }
        // PUT: api/Stock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, StockDTO stockDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid stock ID.");
            }

            // Tìm stock cần cập nhật
            var existingStock = await _context.Stocks.FindAsync(id);
            if (existingStock == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin stock
            existingStock.ProductId = stockDto.ProductId;
            existingStock.Size = stockDto.Size;
            existingStock.Color = stockDto.Color;
            existingStock.Quantity = stockDto.Quantity;

            _context.Entry(existingStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Stock/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }

}
