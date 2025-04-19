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

    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return Ok(reviews);
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // POST: api/Review
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(ReviewDTO reviewDto)
        {
            // Chuyển đổi từ DTO sang Entity
            var review = new Review
            {
                ProductId = reviewDto.ProductId,
                UserId = reviewDto.UserId,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                CreatedAt = reviewDto.CreatedAt
            };

            // Thêm đánh giá mới vào cơ sở dữ liệu
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, ReviewDTO reviewDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid review ID.");
            }

            // Tìm đánh giá cần cập nhật
            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin bài đánh giá
            existingReview.ProductId = reviewDto.ProductId;
            existingReview.UserId = reviewDto.UserId;
            existingReview.Comment = reviewDto.Comment;
            existingReview.Rating = reviewDto.Rating;
            existingReview.CreatedAt = reviewDto.CreatedAt;

            _context.Entry(existingReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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


        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }


}
