using Microsoft.EntityFrameworkCore;
using Solution.DAL;
using Solution.DAL.Context;

namespace Solution.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<Customer> Customers => _context.Customers;
        public DbSet<Order> Orders => _context.Orders;
        public DbSet<Item> Items => _context.OrderItems;
        public DbSet<Product> Products => _context.Products;

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
