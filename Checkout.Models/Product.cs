using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace Checkout.Models
{
  public class Product
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    [JsonIgnore]
    public Collection<Item> Items { get; set; }
  }
}
