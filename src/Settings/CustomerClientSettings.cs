using System.Collections.Generic;

namespace Customers.Settings
{
  public class CustomerClientSettings
  {
    public string BaseURL { get; set; }

    public string ApiKey { get; set; }

    public string CustomerDetailsEndpoint { get; set; }

    public List<string> ValidEmails { get; set; }  = new List<string>
    {
      "cat.owner@mmtdigital.co.uk",
      "dog.owner@fake-customer.com",
      "sneeze@fake-customer.com",
      "santa@north-pole.lp.com"
    };
  }
}