using Checkout.Models;
using Checkout.WebTools;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Client
{
  public class CheckoutClient
  {
    private readonly HttpClient _http;

    public CheckoutClient(HttpClient http, string baseUri)
    {
      _http = http;
      _http.BaseAddress = new Uri(baseUri);
    }

    public async Task<Customer[]> GetCustomers()
    {
      return await _http.Get<Customer[]>("/api/checkout/customer");
    }
  }
}
