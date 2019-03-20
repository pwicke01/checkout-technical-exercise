using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Models.Requests
{
  public class AddItemsRequest
  {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
  }
}
