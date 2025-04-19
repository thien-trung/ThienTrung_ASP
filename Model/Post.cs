namespace TranThienTrung2122110179.Model
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }

        public virtual User Author { get; set; }
    }

}
