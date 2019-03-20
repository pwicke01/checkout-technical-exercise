using Checkout.Domain;
using Checkout.Models;
using Checkout.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Checkout.Api.Controllers
{
  [Route("api/product")]
  public class ProductController : ControllerBase
  {
    private readonly IServiceProvider _serviceProvider;

    public ProductController(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Retrieves a list of all Products from the Database
    /// </summary>
    /// <returns>An array of Product</returns>
    [HttpGet]
    public async Task<Product[]> GetAll()
    {
      return await _serviceProvider.New<ProductService>().GetProducts();
    }

  }
}
