using TranThienTrung2122110179.Model;
using TranThienTrung2122110179.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranThienTrung2122110179.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TranThienTrung2122110179.DTO;

namespace TranThienTrung2122110179.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public UserController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
                return Conflict(new { message = "Email đã được sử dụng" });

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Password = hashedPassword,
                Address = model.Address,
                Description = model.Description
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user.Email, user.Id);

            return Ok(new { message = "Đăng ký thành công", token, user.Name, user.Email, user.Id });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return Unauthorized(new { message = "Sai email hoặc mật khẩu", error = "Invalid credentials" });

            var newToken = _jwtService.GenerateToken(user.Email, user.Id);

            return Ok(new { message = "Đăng nhập thành công", token = newToken, user.Name, user.Email, user.Id });
        }

        // Phương thức Profile - Lấy thông tin người dùng từ JWT token
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var userId = GetUserIdFromToken(); // Lấy user ID từ token

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound(new { message = "Người dùng không tìm thấy" });

            // Trả về thông tin người dùng dưới dạng DTO
            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Description = user.Description
            };

            return Ok(userDTO);
        }

        // Lấy danh sách tất cả người dùng
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    Description = u.Description
                })
                .ToList();

            if (users == null || users.Count == 0)
                return NotFound(new { message = "Không có người dùng nào" });

            return Ok(users);
        }

        // Cập nhật thông tin người dùng
        // Cập nhật thông tin người dùng dựa theo ID truyền vào
        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDTO model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = "Người dùng không tìm thấy" });

            user.Name = model.Name ?? user.Name;
            user.Email = model.Email ?? user.Email;
            user.Phone = model.Phone ?? user.Phone;
            user.Address = model.Address ?? user.Address;
            user.Description = model.Description ?? user.Description;

            _context.Users.Update(user);
            _context.SaveChanges();

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Description = user.Description
            };

            return Ok(new { message = "Cập nhật thành công", user = userDTO });
        }


        // Xóa người dùng
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = "Người dùng không tìm thấy" });

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(new { message = "Tài khoản đã bị xóa thành công" });
        }
        // Phương thức phụ trợ để lấy UserId từ token JWT
        private int GetUserIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}
