using System.ComponentModel.DataAnnotations;

namespace TranThienTrung2122110179.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        // KHÔNG CẦN REQUIRED
        public ICollection<Product>? Products { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
