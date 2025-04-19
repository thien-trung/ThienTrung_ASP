using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TranThienTrung2122110179.Model;

public class ShoppingCart
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }  // Có thể loại bỏ trong quá trình gửi request nếu không cần.

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }  // Có thể loại bỏ trong quá trình gửi request nếu không cần.

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;


}
