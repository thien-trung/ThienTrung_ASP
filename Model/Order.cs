using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranThienTrung2122110179.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        // Không cần phải Include User, chỉ lưu userId
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

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
