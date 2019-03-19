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

    public BasketService(CheckoutContext checkoutContext)
    {
      _checkoutContext = checkoutContext;
    }

    public async Task PutItemInBasket(Guid customerId, AddItemRequest request)
    {
      var basket = await GetCustomerBasket(customerId);

      if (basket == null)
      {
        basket = await CreateBasketForCustomer(customerId);
      }

      if(basket.Items != null && basket.Items.Any(x => x.ProductId == request.ProductId))
      {
        var item = basket.Items.Where(x => x.ProductId == request.ProductId).FirstOrDefault();
        item.Quantity += 1;

        _checkoutContext.Items.Update(item);
        await _checkoutContext.SaveChangesAsync();
      }
      else
      {
        var item = new Item
        {
          UpdatedAt = DateTime.Now,
          BasketId = basket.Id,
          ProductId = request.ProductId,
          Quantity = request.Quantity
        };

        await _checkoutContext.Items.AddAsync(item);
        await _checkoutContext.SaveChangesAsync();
      }
    }

    public async Task<Basket> GetCustomerBasket(Guid customerId)
    {
      return await _checkoutContext.Baskets
        .Include(x => x.Items)
        .Where(x => x.CustomerId == customerId)
        .FirstOrDefaultAsync();
    }

    private async Task<Basket> CreateBasketForCustomer(Guid customerId)
    {
      var basket = await _checkoutContext.Baskets.AddAsync(new Basket { CustomerId = customerId });
      await _checkoutContext.SaveChangesAsync();

      return basket.Entity;
    }

  }
}
