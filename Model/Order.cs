using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TranThienTrung2122110179.Model
{
    public class Order
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now; // Ngày đặt hàng

        [Required]
        public int UserId { get; set; } // Khách hàng đặt hàng

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } // Tổng tiền đơn hàng

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Trạng thái đơn hàng

        [MaxLength(500)]
        public string ShippingAddress { get; set; } // Địa chỉ giao hàng

        public ICollection<OrderDetail> OrderDetails { get; set; } // Danh sách sản phẩm trong đơn hàng
    }
}

