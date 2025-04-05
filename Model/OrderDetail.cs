using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranThienTrung2122110179.Model
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; } // Số lượng sản phẩm

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // Giá từng sản phẩm

        [NotMapped] // Không lưu vào database
        public decimal TotalPrice { get; set; } // Tổng tiền (sẽ tính toán khi lấy dữ liệu)
                                                // Thông tin tạo
        [MaxLength(255)]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Thông tin cập nhật
        [MaxLength(255)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }
}
