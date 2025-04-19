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

    public class PostController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            // Trả về tất cả bài viết
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            // Tìm kiếm bài viết theo id
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // POST: api/Post
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(PostDTO postDto)
        {
            // Chuyển đổi từ DTO sang Entity
            var post = new Post
            {
                Title = postDto.Title,
                Content = postDto.Content,
                CreatedAt = postDto.CreatedAt
            };

            // Thêm bài viết mới vào cơ sở dữ liệu
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, PostDTO postDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid post ID.");
            }

            // Tìm bài viết cần cập nhật
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin bài viết
            existingPost.Title = postDto.Title;
            existingPost.Content = postDto.Content;
            existingPost.CreatedAt = postDto.CreatedAt;

            _context.Entry(existingPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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


        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Kiểm tra xem bài viết có tồn tại không
        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }

}
