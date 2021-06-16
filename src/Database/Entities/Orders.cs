using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Database.Entities
{
  [Table("Orders")]
  public class Orders
  {
    [Key]
    public int OrderId { get; set; }

    public string CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DeliveryExpected { get; set; }

    public bool ContainsGift { get; set; }

    public string ShippingMode { get; set; }

    public string OrderSource { get; set; }

    public List<OrderItems> OrderItems { get; set; }
  }
}