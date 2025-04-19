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

    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Wishlist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlists()
        {
            var wishlists = await _context.Wishlists.ToListAsync();
            return Ok(wishlists);
        }

     // POST: api/Wishlist
    [HttpPost]
    public async Task<ActionResult<Wishlist>> CreateWishlist(WishlistDTO wishlistDto)
    {
        var wishlist = new Wishlist
        {
            UserId = wishlistDto.UserId,
            ProductId = wishlistDto.ProductId
        };

        _context.Wishlists.Add(wishlist);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWishlists), new { id = wishlist.Id }, wishlist);
    }


    // DELETE: api/Wishlist/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
