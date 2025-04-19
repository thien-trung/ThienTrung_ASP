namespace TranThienTrung2122110179.Model
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string Size { get; set; }   // Optional
        public string Color { get; set; }  // Optional
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
    }

}
