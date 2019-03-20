using Checkout.Models;
using Checkout.Models.Requests;
using Checkout.WebTools;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Client
{
  public class CheckoutClient
  {
    private readonly HttpClient _http;

    public CheckoutClient(HttpClient http, string baseUrl)
    {
      if (baseUrl == null)
        throw new ArgumentNullException("Client must have a base URL");

      http.BaseAddress = new Uri(baseUrl);
      _http = http;
    }

    public async Task AddItemsToBasket(AddItemsRequest request)
    {
      await _http.Put("api/basket/items", request);
    }

    
    public async Task RemoveItemsFromBasket(RemoveItemsRequest request)
    {
      await _http.Delete("api/basket/items", request);
    }

    public async Task<Basket> GetCustomerBasket()
    {
      return await _http.Get<Basket>("api/basket");
    }

    public async Task ClearCustomerBasket()
    {
      await _http.Delete("api/basket/clear");
    }
    
    public async Task<Product[]> GetProducts()
    {
      return await _http.Get<Product[]>("api/product");
    }
  }
}
