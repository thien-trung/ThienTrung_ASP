using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThienTrung2122110179.Model;
using TranThienTrung2122110179.Data;
using TranThienTrung2122110179.DTO;
using TranThienTrung2122110179.DTO.TranThienTrung2122110179.DTO;
using Microsoft.AspNetCore.Authorization;

namespace TranThienTrung2122110179.Controllers
{
    [Authorize] // Thêm dòng này để yêu cầu token
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            var orderDetails = await _context.OrderDetails
                                              .ToListAsync();
            return Ok(orderDetails);
        }

        // GET: api/OrderDetails/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails
                                             //.Include(od => od.Order)
                                             //.Include(od => od.Product)
                                             .FirstOrDefaultAsync(od => od.Id == id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> CreateOrderDetail(OrderDetailDTO dto)
        {
            if (dto == null)
                return BadRequest();

            var detail = new OrderDetail
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                TotalPrice = dto.TotalPrice ?? dto.Quantity * dto.UnitPrice,
                CreatedBy = dto.CreatedBy,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedBy = dto.UpdatedBy,
                UpdatedAt = dto.UpdatedAt
            };

            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderDetail), new { id = detail.Id }, detail);
        }

        // PUT: api/OrderDetails/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, OrderDetailDTO dto)
        {
            var detail = await _context.OrderDetails.FindAsync(id);
            if (detail == null)
                return NotFound();

            detail.OrderId = dto.OrderId;
            detail.ProductId = dto.ProductId;
            detail.Quantity = dto.Quantity;
            detail.UnitPrice = dto.UnitPrice;
            detail.TotalPrice = dto.TotalPrice ?? dto.Quantity * dto.UnitPrice;
            detail.UpdatedBy = dto.UpdatedBy;
            detail.UpdatedAt = dto.UpdatedAt ?? DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/OrderDetails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
