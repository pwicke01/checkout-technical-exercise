﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Checkout.Models
{
  public class Basket
  {
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal RunningTotal
    {
      get
      {
        return Items?.Sum(x => x.Product?.Price * x.Quantity) ?? 0M;
      }
    }

    public Collection<Item> Items { get; set; }
  }
}
