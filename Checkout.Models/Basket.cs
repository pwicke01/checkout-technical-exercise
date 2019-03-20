using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Checkout.Models
{
  public class Basket
  {
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    public Collection<Item> Items { get; set; }
  }
}
