using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Customers.Clients;
using Customers.Settings;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Xunit;
using MsOptions = Microsoft.Extensions.Options;

namespace CustomerTests.Clients
{
  public class CustomerClientTests
  {
    private readonly Mock<NullLogger<CustomerClient>> mockLogger;
    private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;
    private readonly CustomerClient customerClient;

    public CustomerClientTests()
    {
      mockLogger = new Mock<NullLogger<CustomerClient>>();
      var mockClientFactory = new Mock<IHttpClientFactory>();
      mockHttpMessageHandler = new Mock<HttpMessageHandler>();

      var mockClient = new HttpClient(mockHttpMessageHandler.Object);
      mockClientFactory.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(mockClient);

      customerClient = new CustomerClient(mockLogger.Object, mockClient, MsOptions.Options.Create(new CustomerClientSettings()
      {
        BaseURL = "http://www.test.com/",
        ApiKey = "1CrsOooSHlV15C7OYnLY0DHjBHyjzoI8LNHITV04cNCyNCahecPDhw==",
        CustomerDetailsEndpoint = "api/GetUserDetails",
        ValidEmails = new List<string>{
          "test@test.com"
        }
      }));
    }

    [Fact]
    public async Task GetCustomerDetailsAsync_WithValidEmail_ReturnsCustomerDetails()
    {
      // arrange
      var email = "test@test.com";
      var expectedUrl =
        $"http://www.test.com/api/GetUserDetails?email={email}&code=1CrsOooSHlV15C7OYnLY0DHjBHyjzoI8LNHITV04cNCyNCahecPDhw==";
      mockHttpMessageHandler.Protected()
      .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.ToString() == expectedUrl), ItExpr.IsAny<CancellationToken>())
      .ReturnsAsync(new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent("{\"email\":\"test@test.com\",\"customerId\":\"C34454\",\"website\":true,\"firstName\":\"Charlie\",\"lastName\":\"Cat\",\"lastLoggedIn\":\"03-Nov-2020 10:22\",\"houseNumber\":\"1a\",\"street\":\"Uppingham Gate\",\"town\":\"Uppingham\",\"postcode\":\"LE15 9NY\",\"preferredLanguage\":\"en-gb\"}")
      });

      // act
      var result = await customerClient.GetCustomerDetailsAsync(email);

      // assert
      Assert.NotNull(result);
      Assert.NotNull(result.Data);
      Assert.Equal(email, result.Data.Email);
      mockHttpMessageHandler.VerifyAll();
    }

    // TODO: add more happy/unhappy path unit testing
  }
}