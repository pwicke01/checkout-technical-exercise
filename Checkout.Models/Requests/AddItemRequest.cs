using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Models.Requests
{
  public class AddItemRequest
  {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
  }
}
