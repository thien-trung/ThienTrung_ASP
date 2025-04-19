namespace TranThienTrung2122110179.DTO
{
    using System;
    using System.ComponentModel.DataAnnotations;

    namespace TranThienTrung2122110179.DTO
    {
        public class ShoppingCartDTO
        {
            [Required]
            public int UserId { get; set; }

            [Required]
            public int ProductId { get; set; }

            [Required]
            public int Quantity { get; set; }

            [Required]
            public decimal TotalPrice { get; set; }

            public string CreatedBy { get; set; }
            public DateTime? CreatedAt { get; set; }


        }
    }

}
