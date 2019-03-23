using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.Models;
using Checkout.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkout.Client.Tests
{
  [TestClass]
  public class CheckoutClientTest
  {
    private readonly CheckoutClient _checkoutClient;

    public CheckoutClientTest()
    {
      _checkoutClient = new CheckoutClient(new HttpClient(), "https://localhost:5001/");
    }

    /// <summary>
    /// Tests to ensure that this .NET Framework project can call the .NET Core API via the .NET Standard client library
    /// </summary>
    [TestMethod]
    public async Task Test_Framework_Compatible()
    {
      var products = await _checkoutClient.GetProducts();
      Assert.IsTrue(products.Any());
    }

    /// <summary>
    /// Tests adding an item to a customer's basket. This is a copy of the test in Checkout.Api.Tests but using the client rather than calling the service directly.
    /// The main purpose of this is to demonstrate that the client library works, including the persistence of cookies across API calls during the session.
    /// </summary>
    [TestMethod]
    public async Task Test_Add_Item_To_Basket()
    {
      var products = await _checkoutClient.GetProducts();

      var product = products.FirstOrDefault();

      Assert.IsNotNull(product);

      int expectedQuantity = 2;

      await _checkoutClient.AddItemsToBasket(new AddItemsRequest
      {
        ProductId = product.Id,
        Quantity = expectedQuantity
      });

      int actualQuantity = await GetNumberOfItemsInBasket(product.Id);

      Assert.AreEqual(expectedQuantity, actualQuantity);
    }

    private async Task<int> GetNumberOfItemsInBasket(Guid? productId = null)
    {
      var basket = await _checkoutClient.GetCustomerBasket();

      Assert.IsNotNull(basket);

      int quantity = 0;

      if (productId.HasValue)
      {
        quantity = basket.Items?.FirstOrDefault(x => x.ProductId == productId)?.Quantity ?? 0;
      }
      else
      {
        quantity = basket.Items?.Sum(x => x.Quantity) ?? 0;
      }

      return quantity;
    }
  }
}
