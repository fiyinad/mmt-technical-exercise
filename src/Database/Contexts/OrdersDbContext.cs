using Customers.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.Database
{
    public class OrdersDbContext : DbContext
    {
      public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
      {
      }

      public DbSet<Orders> Orders { get; set; }

      public DbSet<Products> Products { get; set; }

      public DbSet<OrderItems> OrderItems { get; set; }
    }
}