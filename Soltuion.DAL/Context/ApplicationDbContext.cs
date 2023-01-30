using Microsoft.EntityFrameworkCore;

namespace Solution.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> OrderItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Method intentionally left empty.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId);
            modelBuilder.Entity<Item>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Customer>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().HasMany(x => x.Items).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Order>().HasMany(x => x.Items).WithOne().HasForeignKey(a=>a.OrderId);
        }
    }
}

