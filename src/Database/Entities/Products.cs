using System.ComponentModel.DataAnnotations;

namespace Customers.Database.Entities
{
  public class Products
  {
    [Key]
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal PackHeight { get; set; }

    public decimal PackWidth { get; set; }

    public decimal PackWeight { get; set; }

    public string Colour { get; set; }

    public string Size { get; set; }
  }
}