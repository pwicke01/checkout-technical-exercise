using System;

namespace Checkout.Models.Requests
{
  public class RemoveItemsRequest
  {
    public Guid ProductId { get; set; }
    public int? Quantity { get; set; }
    public bool? RemoveAll { get; set; }
  }
}
