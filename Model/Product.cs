using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranThienTrung2122110179.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } // Tên sản phẩm

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá sản phẩm

        [Required]
        public int Stock { get; set; } // Số lượng tồn kho  

        [MaxLength(1000)]
        public string Description { get; set; } // Mô tả sản phẩm

        [MaxLength(255)]
        public string ImageUrl { get; set; } // Đường dẫn ảnh sản phẩm

        [Required]
        public int CategoryId { get; set; } // Khóa ngoại liên kết với bảng Category

        [ForeignKey("CategoryId")]
        //public Category Category { get; set; }

        [Required]
        [MaxLength(255)]
        public string Brand { get; set; } // Thương hiệu

        [Required]
        public bool IsActive { get; set; } = true; // Trạng thái hoạt động

        // Thông tin tạo
        [MaxLength(255)]
        public string CreatedBy { get; set; } // Người tạo

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Ngày tạo sản phẩm




    }
}
