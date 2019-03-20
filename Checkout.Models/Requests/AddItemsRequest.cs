using System;

namespace Checkout.Models.Requests
{
  public class AddItemsRequest
  {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
  }
}
