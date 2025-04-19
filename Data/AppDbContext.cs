using Microsoft.EntityFrameworkCore;
using TranThienTrung2122110179.Model;

namespace TranThienTrung2122110179.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Khai báo DbSet cho các bảng trong database
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }


        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Post> Posts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.TotalPrice)
                .HasColumnType("decimal(18,2)"); 

            base.OnModelCreating(modelBuilder);
        }

    }
}

