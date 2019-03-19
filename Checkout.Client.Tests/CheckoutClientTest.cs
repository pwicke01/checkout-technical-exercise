using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

    [TestMethod]
    public async Task Test_Framework_Compatible()
    {
      var custs = await _checkoutClient.GetCustomers();
      Assert.IsTrue(custs != null && custs.Any());
    }
  }
}
