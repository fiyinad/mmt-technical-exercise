using System.Threading.Tasks;
using Customers.Models.Requests;
using Customers.Models.Responses;
using Customers.Response;

namespace Customers.Services
{
    public interface ICustomersService
    {
      /// <summary>
      /// Retrieves a customers most recent order
      /// </summary>
      /// <param name="request"></param>
      /// <returns></returns>
      Task<IResponse<CustomerOrderResponse>> GetOrderAsync(CustomerOrderRequest request);
    }
}