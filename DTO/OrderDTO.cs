
    namespace TranThienTrung2122110179.DTO
    {
        public class OrderDTO
        {
            public int UserId { get; set; }
            public DateTime OrderDate { get; set; }
            public string ShippingAddress { get; set; }
            public string Status { get; set; }
            public decimal TotalAmount { get; set; }

            public string CreatedBy { get; set; }
            public DateTime? CreatedAt { get; set; } = DateTime.Now;

            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }


