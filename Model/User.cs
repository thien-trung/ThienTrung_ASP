using System.ComponentModel.DataAnnotations;

namespace TranThienTrung2122110179.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } // Họ và tên

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } // Email

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } // Mật khẩu đã mã hóa

        [MaxLength(15)]
        public string PhoneNumber { get; set; } // Số điện thoại

        [MaxLength(500)]
        public string Address { get; set; } // Địa chỉ

        [Required]
        public bool IsAdmin { get; set; } = false; // Phân quyền (Admin hoặc User)

        // Thông tin tạo
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Ngày tạo tài khoản

        [MaxLength(255)]
        public string CreatedBy { get; set; }


        // Thông tin cập nhật
        [MaxLength(255)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }
}
