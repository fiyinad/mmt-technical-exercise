using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Customers.Database;
using Customers.Database.Entities;
using Customers.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CustomerTests.Database.Repository
{
  public class OrdersRepositoryTests
  {
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<NullLogger<OrdersRepository>> mockLogger =
      new Mock<NullLogger<OrdersRepository>>();
    private readonly DbContextOptionsBuilder<OrdersDbContext> batchOptionsBuilder =
      new DbContextOptionsBuilder<OrdersDbContext>()
      .UseInMemoryDatabase(databaseName: "orders")
      .EnableServiceProviderCaching(false);

    public OrdersRepositoryTests()
    {
    }

    [Fact]
    public async Task GetAsync_ReturnsExistingOrder_Successfully()
    {
      // arrange
      using (var ordersDbContext = new OrdersDbContext(batchOptionsBuilder.Options))
      {
        int orderId = 501, orderItemId = 502, productId = 503;
        var ordersRepository = new OrdersRepository(mockLogger.Object, ordersDbContext);
        var orderItems = new OrderItems
        {
          OrderId = orderId,
          OrderItemId = orderItemId,
          Price = _fixture.Create<decimal>(),
          ProductId = productId,
          Quantity = 4,
          Returnable = false
        };
        var orders = new Orders()
        {
          OrderId = orderId,
          CustomerId = "test@email.com",
          ContainsGift = false,
          DeliveryExpected = new DateTime(2021, 06, 30),
          OrderDate = new DateTime(2021, 06, 15),
          OrderItems = new List<OrderItems> { orderItems },
          OrderSource = "website",
          ShippingMode = "DHL"
        };
        ordersDbContext.Add(orders);
        await ordersDbContext.SaveChangesAsync();

        // act
        var result = await ordersRepository.GetAsync(orderId);

        // assert
        Assert.NotNull(result);
        Assert.Equal(orders, result);
      }
    }

    [Fact]
    public async Task GetAllAsync_ReturnsExistingOrder_Successfully()
    {
      // arrange
      using (var ordersDbContext = new OrdersDbContext(batchOptionsBuilder.Options))
      {
        int orderId = 501, orderItemId = 502, productId = 503;
        var ordersRepository = new OrdersRepository(mockLogger.Object, ordersDbContext);
        var orderItemsOne = new OrderItems
        {
          OrderId = orderId,
          OrderItemId = orderItemId,
          Price = _fixture.Create<decimal>(),
          ProductId = productId,
          Quantity = 4,
          Returnable = false
        };
        var orderItemsTwo = new OrderItems
        {
          OrderId = orderId + 10,
          OrderItemId = orderItemId + 10,
          Price = _fixture.Create<decimal>(),
          ProductId = productId + 10,
          Quantity = 4,
          Returnable = false
        };
        var ordersOne = new Orders()
        {
          OrderId = orderId,
          CustomerId = "test@email.com",
          ContainsGift = false,
          DeliveryExpected = new DateTime(2021, 06, 30),
          OrderDate = new DateTime(2021, 06, 15),
          OrderItems = new List<OrderItems> { orderItemsOne },
          OrderSource = "website",
          ShippingMode = "DHL"
        };
        var ordersTwo = new Orders()
        {
          OrderId = orderId + 10,
          CustomerId = "testone@email.com",
          ContainsGift = true,
          DeliveryExpected = new DateTime(2021, 07, 30),
          OrderDate = new DateTime(2021, 07, 15),
          OrderItems = new List<OrderItems> { orderItemsTwo },
          OrderSource = "mobile",
          ShippingMode = "FedEx"
        };
        ordersDbContext.Add(ordersOne);
        ordersDbContext.Add(ordersTwo);
        await ordersDbContext.SaveChangesAsync();

        var result = await ordersRepository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Collection(result,
          item => Assert.Equal(ordersOne.OrderId, item.OrderId),
          item => Assert.Equal(ordersTwo.OrderId, item.OrderId));
        Assert.Equal(2, result.Count());
      }
    }

    [Fact]
    public async Task GetByCustomerIdAsync_ReturnsExistingOrder_Successfully()
    {
      // arrange
      using (var ordersDbContext = new OrdersDbContext(batchOptionsBuilder.Options))
      {
        int orderId = 501, orderItemId = 502, productId = 503;
        string customerId = "test@email.com";
        var ordersRepository = new OrdersRepository(mockLogger.Object, ordersDbContext);
        var orderItems = new OrderItems
        {
          OrderId = orderId,
          OrderItemId = orderItemId,
          Price = _fixture.Create<decimal>(),
          ProductId = productId,
          Quantity = 4,
          Returnable = false
        };
        var orders = new Orders()
        {
          OrderId = orderId,
          CustomerId = customerId,
          ContainsGift = false,
          DeliveryExpected = new DateTime(2021, 06, 30),
          OrderDate = new DateTime(2021, 06, 15),
          OrderItems = new List<OrderItems> { orderItems },
          OrderSource = "website",
          ShippingMode = "DHL"
        };
        ordersDbContext.Add(orders);
        await ordersDbContext.SaveChangesAsync();

        // act
        var result = await ordersRepository.GetAllByCustomerIdAsync(customerId);

        // assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Collection(result, item => Assert.Equal(orders.OrderId, item.OrderId));
      }
    }
  }
}