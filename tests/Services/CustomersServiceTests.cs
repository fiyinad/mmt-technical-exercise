using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Customers.Clients;
using Customers.Database.Repository;
using Customers.Mappers;
using Customers.Models.Requests;
using Customers.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CustomerTests.Services
{
  public class CustomersServiceTests
  {
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<NullLogger<CustomersService>> _logger =
      new Mock<NullLogger<CustomersService>>();
    private readonly Mock<IValidator<CustomerOrderRequest>> _customerOrderRequestValidator =
      new Mock<IValidator<CustomerOrderRequest>>();
    private readonly Mock<ICustomerClient> _customerClient =
      new Mock<ICustomerClient>();
    private readonly Mock<IOrdersRepository> _repository =
      new Mock<IOrdersRepository>();
    private readonly CustomersService _service;

    public CustomersServiceTests()
    {
      var mapperConfiguration = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MapperProfile>();
      });
      IMapper mapper = new Mapper(mapperConfiguration);
      _service =
        new CustomersService(
          _logger.Object, 
          _customerOrderRequestValidator.Object, 
          _customerClient.Object, 
          _repository.Object, 
          mapper);
    }

    [Fact]
    public async Task GetOrderAsync_Fails_WhenValidationFails()
    {
      // arrange 
      var validationResult =
        new Mock<ValidationResult>();
      validationResult
        .SetupGet(_ => _.IsValid)
        .Returns(false);
      _customerOrderRequestValidator
        .Setup(_ => _.ValidateAsync(It.IsAny<CustomerOrderRequest>(), new CancellationToken()))
        .ReturnsAsync(validationResult.Object);

      // act 
      var result =
        await _service.GetOrderAsync(It.IsAny<CustomerOrderRequest>());

      // assert
      Assert.NotNull(result);
      Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task GetOrderAsync_Fails_WhenExceptionIsThrown()
    {
      // arrange 
      var customerOrderRequestObject = _fixture.Create<CustomerOrderRequest>();

      var validationResult =
        new Mock<ValidationResult>();
      validationResult
        .SetupGet(_ => _.IsValid)
        .Returns(true);
      _customerOrderRequestValidator
        .Setup(_ => _.ValidateAsync(It.IsAny<CustomerOrderRequest>(), new CancellationToken()))
        .ThrowsAsync(new Exception());

      // act
      var result = await _service.GetOrderAsync(customerOrderRequestObject);

      // assert
      Assert.NotNull(result);
      Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
    }

    // TODO: add more happy/unhappy path tests
  }
}