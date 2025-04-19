using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TranThienTrung2122110179.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Có thể tính tự động từ Quantity * UnitPrice

        [MaxLength(255)]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Thông tin cập nhật
        [MaxLength(255)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
