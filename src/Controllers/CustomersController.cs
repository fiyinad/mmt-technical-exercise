using System.Threading.Tasks;
using Customers.Models.Requests;
using Customers.Models.Responses;
using Customers.Response;
using Customers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace Customers.Controllers
{
  /// <inheritdoc />
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class CustomersController : ControllerBase
  {
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomersService _service;

    /// <summary>
    /// Creates an instance of <see cref="CustomersController"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public CustomersController(
      ILogger<CustomersController> logger,
      ICustomersService service)
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// Gets a customers most recent order
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetOrder([FromBody]CustomerOrderRequest request)
    {
      var response = await _service.GetOrderAsync(request);
      return response.GetResult();
    }
  }
}