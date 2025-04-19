namespace TranThienTrung2122110179.Model
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public int Rating { get; set; } // 1 đến 5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }

}
