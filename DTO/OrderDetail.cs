namespace TranThienTrung2122110179.DTO
{
    namespace TranThienTrung2122110179.DTO
    {
        public class OrderDetailDTO
        {
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal? TotalPrice { get; set; } // Nếu null thì sẽ tính tự động

            public string CreatedBy { get; set; }
            public DateTime? CreatedAt { get; set; }

            public string UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }

}
