using Newtonsoft.Json;

namespace Customers.Models.Requests
{
    public class CustomerOrderRequest
    {
      [JsonProperty("user")]
      public string User { get; set; }

      [JsonProperty("customerId")]
      public string CustomerId { get; set; }
    }
}