using Microsoft.EntityFrameworkCore;
using QualityBags.Models;

namespace QualityBags.Data
{
    /// <summary>
    /// Database context class for Quality Bags
    /// </summary>
    public class QualityBagsContext: DbContext
    {
        public QualityBagsContext(DbContextOptions<QualityBagsContext> options): base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderDetail>().ToTable("Order Detail");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Supplier>().ToTable("Supplier");
        }
    }
}
