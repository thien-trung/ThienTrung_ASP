using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TranThienTrung2122110179.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        // Constructor nhận IConfiguration để truy cập các giá trị từ file cấu hình (appsettings.json hoặc các nguồn cấu hình khác)
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        // Phương thức tạo token JWT cho một người dùng dựa trên email


        public string GenerateToken(string email, int userId)
        {
            // Định nghĩa các claims cho token
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, email), // Claim cho tên người dùng (email)
        new Claim(JwtRegisteredClaimNames.Email, email), // Claim cho email
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Claim cho UserId
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Claim cho JTI (Unique Identifier cho mỗi token)
    };

            // Lấy khóa bí mật từ file cấu hình (đảm bảo không lưu trực tiếp trong mã nguồn)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Cấu hình phương thức ký token sử dụng thuật toán HMAC SHA-256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo JWT token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"], // Người phát hành token (Issuer)
                audience: _config["Jwt:Audience"], // Người nhận token (Audience)
                claims: claims, // Claims đã định nghĩa
                expires: DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
                signingCredentials: creds // Phương thức ký token
            );

            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Phương thức xác thực token JWT
        public string ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]); // Lấy khóa bí mật từ cấu hình

            try
            {
                // Cấu hình xác thực token
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true, // Kiểm tra tính hợp lệ của Issuer
                    ValidateAudience = true, // Kiểm tra tính hợp lệ của Audience
                    ValidateLifetime = true, // Kiểm tra thời gian hết hạn của token
                    ValidateIssuerSigningKey = true, // Kiểm tra tính hợp lệ của key ký token
                    ValidIssuer = _config["Jwt:Issuer"], // Issuer hợp lệ
                    ValidAudience = _config["Jwt:Audience"], // Audience hợp lệ
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Khóa bí mật để xác minh chữ ký
                    ClockSkew = TimeSpan.Zero // Không có độ trễ thời gian
                }, out SecurityToken validatedToken);

                // Trả về tên người dùng (email) từ principal
                return principal.Identity.Name;
            }
            catch
            {
                // Trả về null nếu token không hợp lệ
                return null;
            }
        }
    }
}
