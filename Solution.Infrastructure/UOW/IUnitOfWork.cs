using Microsoft.EntityFrameworkCore;
using Solution.DAL;

namespace Solution.Infrastructure.UOW
{

    public interface IUnitOfWork
    {
        DbSet<Customer> Customers { get; }
        DbSet<Order> Orders { get; }
        DbSet<Item> Items { get; }
        DbSet<Product> Products { get; }

        void SaveChanges();
    }

}
