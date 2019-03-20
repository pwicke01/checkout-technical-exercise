using Checkout.Data;
using Checkout.Domain;
using Checkout.Models;
using Checkout.Models.Requests;
using Checkout.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Tests
{
  public class CheckoutApiTests
  {
    private readonly Customer _customer;
    private readonly IServiceProvider _serviceProvider;

    public CheckoutApiTests()
    {
      var services = new ServiceCollection();
      services.AddDbContext<CheckoutContext>(opt => opt.UseInMemoryDatabase("Checkout"));

      services.AddScoped<CustomerService>();
      services.AddScoped<BasketService>();
      services.AddScoped<ProductService>();

      _serviceProvider = services.BuildServiceProvider();

      _customer = new Customer
      {
        Id = Guid.NewGuid(),
        Name = "Test Guest"
      };
    }

    [Fact]
    public async Task Test_Products_Exist()
    {
      var products = await GetProducts();
      Assert.NotEmpty(products);
    }

    [Fact]
    public async Task Test_Add_Item_To_Basket()
    {
      var products = await GetProducts();

      var product = products.FirstOrDefault();

      Assert.NotNull(product);

      int expectedQuantity = 2;

      await AddItemsToBasket(product.Id, expectedQuantity);

      int actualQuantity = await GetNumberOfItemsInBasket(product.Id);

      Assert.Equal(expectedQuantity, actualQuantity);
    }

    [Fact]
    public async Task Test_Remove_Item_From_Basket()
    {
      var products = await GetProducts();

      var product = products.FirstOrDefault();

      Assert.NotNull(product);

      int expectedQuantity = 2;

      await AddItemsToBasket(product.Id, expectedQuantity);

      int itemsToRemove = 1;

      await _serviceProvider.New<BasketService>().RemoveItemsFromBasket(_customer.Id, new RemoveItemsRequest
      {
        ProductId = product.Id,
        Quantity = itemsToRemove
      });

      int actualQuantity = await GetNumberOfItemsInBasket(product.Id);

      Assert.Equal(expectedQuantity - itemsToRemove, actualQuantity);

      await _serviceProvider.New<BasketService>().RemoveItemsFromBasket(_customer.Id, new RemoveItemsRequest
      {
        ProductId = product.Id,
        RemoveAll = true
      });

      actualQuantity = await GetNumberOfItemsInBasket(product.Id);

      Assert.Equal(0, actualQuantity);
    }

    [Fact]
    public async Task Test_Clear_Basket()
    {
      var products = await GetProducts();

      foreach(var p in products)
      {
        await AddItemsToBasket(p.Id, 3);
      }

      int actualQuantity = await GetNumberOfItemsInBasket();

      Assert.NotEqual(0, actualQuantity);

      await _serviceProvider.New<BasketService>().ClearCustomerBasket(_customer.Id);

      actualQuantity = await GetNumberOfItemsInBasket();

      Assert.Equal(0, actualQuantity);
    }

    private async Task<int> GetNumberOfItemsInBasket(Guid? productId = null)
    {
      var basket = await _serviceProvider.New<BasketService>().GetCustomerBasket(_customer.Id);

      Assert.NotNull(basket);

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

    private async Task AddItemsToBasket(Guid productId, int quantity)
    {
      await _serviceProvider.New<BasketService>().AddItemsToBasket(_customer.Id, new AddItemsRequest
      {
        ProductId = productId,
        Quantity = quantity
      });
    }

    private async Task<Product[]> GetProducts()
    {
      return await _serviceProvider.New<ProductService>().GetProducts();
    }

  }
}
