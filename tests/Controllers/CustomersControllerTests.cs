using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Customers.Controllers;
using Customers.Models.Requests;
using Customers.Models.Responses;
using Customers.Response;
using Customers.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CustomerTests.Controllers
{
  public class CustomerControllerTests
  {
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<NullLogger<CustomersController>> _logger =
      new Mock<NullLogger<CustomersController>>();
    private readonly Mock<ICustomersService> _service =
      new Mock<ICustomersService>();
    private readonly CustomersController _controller;

    public CustomerControllerTests()
    {
      _controller = new CustomersController(
        _logger.Object,
        _service.Object);
    }

    [Fact]
    public async Task Create_ReturnsResponseFromService()
    {
      // arrange 
      var serviceResponse =
        new Response<CustomerOrderResponse>(HttpStatusCode.OK);
      _service.Setup(_ => _.GetOrderAsync(It.IsAny<CustomerOrderRequest>())).ReturnsAsync(serviceResponse);

      // act 
      var response = await _controller.GetOrder(It.IsAny<CustomerOrderRequest>());

      // assert
      Assert.NotNull(response);
      Assert.IsType<StatusCodeResult>(response);
      Assert.Equal((int)HttpStatusCode.OK, (response as StatusCodeResult).StatusCode);
    }
  }
}