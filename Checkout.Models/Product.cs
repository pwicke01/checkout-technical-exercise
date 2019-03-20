using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
