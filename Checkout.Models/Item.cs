using Newtonsoft.Json;
using System;

namespace Checkout.Models
{
  public class Item
  {
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public Basket Basket { get; set; }
    
    public Product Product { get; set; }
  }
}
