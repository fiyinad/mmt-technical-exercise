using System.Threading.Tasks;
using Customers.Clients.Responses;
using Customers.Response;

namespace Customers.Clients
{
  public interface ICustomerClient
  {
    /// <summary>
    /// Gets customer details
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<IResponse<CustomerDetails>> GetCustomerDetailsAsync(string email);
  }
}