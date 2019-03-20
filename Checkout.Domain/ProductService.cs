using Checkout.Data;
using Checkout.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Domain
{
  public class ProductService
  {
    private readonly CheckoutContext _checkoutContext;

    public ProductService(CheckoutContext checkoutContext)
    {
      _checkoutContext = checkoutContext;
    }

    public async Task<Product[]> GetProducts()
    {
      if (!await _checkoutContext.Products.AnyAsync())
      {
        await _checkoutContext.Database.EnsureCreatedAsync();
      }
      return await _checkoutContext.Products.ToArrayAsync();
    }
  }
}
