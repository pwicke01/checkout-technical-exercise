using Checkout.Domain;
using Checkout.Models;
using Checkout.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Api.Controllers
{
  [Route("product")]
  public class ProductController : ControllerBase
  {
    private readonly IServiceProvider _serviceProvider;

    public ProductController(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public async Task<Product[]> GetAll()
    {
      return await _serviceProvider.New<ProductService>().GetAll();
    }

  }
}
