using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Database.Entities
{
  [Table("OrderItems")]
  public class OrderItems
  {
    [Key]
    public int OrderItemId { get; set; }

    [ForeignKey("Orders")]
    public int OrderId { get; set; }
    public Orders Orders { get; set; }

    [ForeignKey("Products")]
    public int ProductId { get; set; }
    public Products Products { get; set; }

    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    public bool Returnable { get; set; }
  }
}