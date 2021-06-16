using System;
using System.Net.Http;
using System.Threading.Tasks;
using Customers.Clients.Responses;
using Customers.Response;
using Customers.Services;
using Customers.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Customers.Clients
{
  public class CustomerClient : BaseService, ICustomerClient
  {
    private readonly ILogger<CustomerClient> _logger;
    private readonly HttpClient _httpClient;
    private readonly IOptions<CustomerClientSettings> _settings;

    public CustomerClient(
      ILogger<CustomerClient> logger,
      HttpClient httpClient,
      IOptions<CustomerClientSettings> settings) : base(logger)
    {
      _logger = logger;
      _httpClient = httpClient;
      _settings = settings;
    }

    /// <inheritdoc />
    public async Task<IResponse<CustomerDetails>> GetCustomerDetailsAsync(string email)
    {
      try
      {
        // validation
        if (string.IsNullOrWhiteSpace(email))
        {
          string errorMessage = $"The parameter '{email}' is null empty or whitespace";
          return BadRequest<CustomerDetails>(errorMessage);
        }

        var validEmails = _settings.Value.ValidEmails;
        if (!validEmails.Contains(email))
        {
          string errorMessage = $"The email '{email}' you've supplied is an invalid email";
          return BadRequest<CustomerDetails>(errorMessage);
        }

        var url = $"{_settings.Value.BaseURL}{_settings.Value.CustomerDetailsEndpoint}?email={email}&code={_settings.Value.ApiKey}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
          _logger.LogError($"Response from Customer Details API was unsuccessful: {response.StatusCode} - {response.ReasonPhrase}");
          return new Response.Response<CustomerDetails>(response.StatusCode);
        }

        var responseContent =
          await response.Content.ReadAsStringAsync();
        var customerDetails =
          JsonConvert.DeserializeObject<CustomerDetails>(responseContent);
        return Success<CustomerDetails>(customerDetails);
      }
      catch (Exception ex)
      {
        return Failure<CustomerDetails>(ex.Message);
      }
    }
  }
}