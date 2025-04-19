using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThienTrung2122110179.Model;
using TranThienTrung2122110179.Data;
using TranThienTrung2122110179.DTO;
using Microsoft.AspNetCore.Authorization;

namespace TranThienTrung2122110179.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails).ToListAsync();
            return Ok(orders);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]

        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]

        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest();
            }

            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate,
                ShippingAddress = orderDto.ShippingAddress,
                Status = orderDto.Status,
                TotalAmount = orderDto.TotalAmount,
                CreatedBy = orderDto.CreatedBy,
                CreatedAt = orderDto.CreatedAt ?? DateTime.Now,
                UpdatedBy = orderDto.UpdatedBy,
                UpdatedAt = orderDto.UpdatedAt
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateOrder(int id, OrderDTO orderDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.UserId = orderDto.UserId;
            order.OrderDate = orderDto.OrderDate;
            order.ShippingAddress = orderDto.ShippingAddress;
            order.Status = orderDto.Status;
            order.TotalAmount = orderDto.TotalAmount;
            order.UpdatedBy = orderDto.UpdatedBy;
            order.UpdatedAt = orderDto.UpdatedAt ?? DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
