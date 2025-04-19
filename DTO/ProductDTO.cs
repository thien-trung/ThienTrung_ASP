namespace TranThienTrung2122110179.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; } // ID sản phẩm
        public string Name { get; set; } // Tên sản phẩm
        public decimal Price { get; set; } // Giá sản phẩm
        public int Stock { get; set; } // Số lượng tồn kho  
        public string Description { get; set; } // Mô tả sản phẩm
        public string ImageUrl { get; set; } // Đường dẫn ảnh sản phẩm
        public int CategoryId { get; set; } // Khóa ngoại liên kết với bảng Category
        public string Brand { get; set; } // Thương hiệu
        public bool IsActive { get; set; } // Trạng thái hoạt động
        public string CreatedBy { get; set; } // Người tạo
        public DateTime CreatedAt { get; set; } // Ngày tạo sản phẩm

    }
}

