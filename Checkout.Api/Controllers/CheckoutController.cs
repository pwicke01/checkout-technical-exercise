using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Data;
using Checkout.Domain;
using Checkout.Models;
using Checkout.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers
{
  [Route("api/checkout")]
  [ApiController]
  public class CheckoutController : ControllerBase
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly CheckoutContext _checkoutContext;

    public CheckoutController(IServiceProvider serviceProvider, CheckoutContext checkoutContext)
    {
      _serviceProvider = serviceProvider;
      _checkoutContext = checkoutContext;
    }

    [HttpPut("customer")]
    public async Task AddCustomer([FromBody] Customer customer)
    {
      customer.Id = Guid.NewGuid();
      customer.Name = "Jerry";

      await _serviceProvider.New<CheckoutService>().AddCustomer(customer);
    }

    [HttpGet("customer")]
    public async Task<Customer[]> GetCustomers()
    {
      return await _serviceProvider.New<CheckoutService>().GetCustomers();
    }
  }
}
