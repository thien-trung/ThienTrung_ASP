namespace TranThienTrung2122110179.Model
{
    public class Deal
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public decimal DiscountPercent { get; set; } // % giảm
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Product Product { get; set; }
    }

}
