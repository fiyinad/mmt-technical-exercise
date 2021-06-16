using System.Collections.Generic;
using System.Threading.Tasks;
using Customers.Database.Entities;

namespace Customers.Database.Repository
{
  public interface IOrdersRepository
  {
    Task<Orders> GetByCustomerIdAsync(string customerId);

    Task<Orders> GetAsync(int id);
    
    Task<IEnumerable<Orders>> GetAllAsync();

    Task<IEnumerable<Orders>> GetAllByCustomerIdAsync(string customerId);
  }
}