using System.ComponentModel.DataAnnotations;

namespace TranThienTrung2122110179.Model
{
   public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } // Tên danh mục

        [MaxLength(1000)]
        public string Description { get; set; } // Mô tả danh mục

        public ICollection<Product> Products { get; set; } // Danh sách sản phẩm thuộc danh mục
    }

}
