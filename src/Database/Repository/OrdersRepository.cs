using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customers.Database.Repository
{
  public class OrdersRepository : IOrdersRepository
  {
    private readonly ILogger<OrdersRepository> _logger;
    private readonly OrdersDbContext _dbContext;

    public OrdersRepository(
      ILogger<OrdersRepository> logger,
      OrdersDbContext dbContext)
    {
      _logger = logger;
      _dbContext = dbContext;
    }
    public async Task<Orders> GetAsync(int id)
    {
      return await _dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
    }

    public async Task<IEnumerable<Orders>> GetAllAsync()
    {
      return await _dbContext.Orders.ToListAsync();
    }

    public async Task<Orders> GetByCustomerIdAsync(string customerId)
    {
      return await _dbContext.Orders.FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task<IEnumerable<Orders>> GetAllByCustomerIdAsync(string customerId)
    {
      return await _dbContext.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
    }
  }
}