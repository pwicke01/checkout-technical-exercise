using Checkout.Data;
using Checkout.Models;
using Checkout.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Domain
{
  public class BasketService
  {
    private readonly CheckoutContext _checkoutContext;
    private readonly ProductService _productService;

    public BasketService(CheckoutContext checkoutContext, ProductService productService)
    {
      _checkoutContext = checkoutContext;
      _productService = productService;
    }

    public async Task AddItemsToBasket(Guid customerId, AddItemsRequest request)
    {
      var basket = await GetCustomerBasket(customerId);

      if (basket == null)
      {
        basket = await CreateBasketForCustomer(customerId);
      }

      var item = basket.Items?.FirstOrDefault(x => x.ProductId == request.ProductId);
      if(item != null)
      {
        item.Quantity += request.Quantity;

        _checkoutContext.Items.Update(item);
        await _checkoutContext.SaveChangesAsync();
      }
      else
      {
        var products = await _productService.GetProducts();
        if (products.Any(x => x.Id == request.ProductId))
        {
          item = new Item
          {
            UpdatedAt = DateTime.Now,
            BasketId = basket.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity
          };

          await _checkoutContext.Items.AddAsync(item);
          await _checkoutContext.SaveChangesAsync();
        }
        else
        {
          throw new Exception("The specified product does not exist");
        }
      }
    }


    public async Task RemoveItemsFromBasket(Guid customerId, RemoveItemsRequest request)
    {
      var basket = await GetCustomerBasket(customerId);

      var item = basket?.Items?.FirstOrDefault(x => x.ProductId == request.ProductId);

      var quantity = request.Quantity ?? 0;

      if (item != null && quantity > 0)
      {
        if(request.RemoveAll ?? false || quantity >= item.Quantity)
        {
          _checkoutContext.Remove(item);
        }
        else
        {
          item.Quantity -= quantity;
          _checkoutContext.Items.Update(item);
        }

        await _checkoutContext.SaveChangesAsync();
      }
    }

    public async Task<Basket> GetCustomerBasket(Guid customerId)
    {
      return await _checkoutContext.Baskets
        .Include(x => x.Items)
        .ThenInclude(x => x.Product)
        .Where(x => x.CustomerId == customerId)
        .FirstOrDefaultAsync();
    }

    public async Task ClearCustomerBasket(Guid customerId)
    {
      var basket = await GetCustomerBasket(customerId);

      var items = basket?.Items;
      if (items != null)
      {
        _checkoutContext.Items.RemoveRange(items);
        await _checkoutContext.SaveChangesAsync();
      }
    }

    private async Task<Basket> CreateBasketForCustomer(Guid customerId)
    {
      var basket = await _checkoutContext.Baskets.AddAsync(new Basket { CustomerId = customerId });
      await _checkoutContext.SaveChangesAsync();

      return basket.Entity;
    }

  }
}
