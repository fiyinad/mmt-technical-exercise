using Newtonsoft.Json;

namespace Customers.Models.Responses
{
  public class Customer
  {
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }
  }
}