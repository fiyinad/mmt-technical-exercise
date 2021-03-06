using System.Collections.Generic;
using Newtonsoft.Json;

namespace Customers.Models.Responses
{
  public class Order
  {
    [JsonProperty("orderNumber")]
    public string OrderNumber { get; set; }

    [JsonProperty("orderDate")]
    public string OrderDate { get; set; }

    [JsonProperty("deliveryAddress")]
    public string DeliveryAddress { get; set; }

    [JsonProperty("orderItems")]
    public List<OrderItem> OrderItems { get; set; }

    [JsonProperty("deliveryExpected")]
    public string DeliveryExpected { get; set; }
  }
}