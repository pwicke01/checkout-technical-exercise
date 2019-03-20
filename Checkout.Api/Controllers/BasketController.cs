using Checkout.Domain;
using Checkout.Models;
using Checkout.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Checkout.Utils;

namespace Checkout.Api.Controllers
{
  [Route("api/basket")]
  public class BasketController : ControllerBase
  {
    private readonly IServiceProvider _serviceProvider;
    
    public BasketController(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    [HttpPut("items")]
    public async Task<IActionResult> AddItems([FromBody] AddItemsRequest request)
    {
      if (request == null || request.Quantity <= 0)
        return BadRequest();

      var customer = await GetCustomer();

      try
      {
        await _serviceProvider.New<BasketService>().AddItemsToBasket(customer.Id, request);
      }
      catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }

      return Ok();
    }

    [HttpDelete("items")]
    public async Task<IActionResult> RemoveItems([FromBody] RemoveItemsRequest request)
    {
      if (request == null)
        return BadRequest();

      var customer = await GetCustomer();

      await _serviceProvider.New<BasketService>().RemoveItemsFromBasket(customer.Id, request);

      return Ok();
    }

    [HttpGet]
    public async Task<Basket> GetCustomerBasket()
    {
      var customer = await GetCustomer();

      return await _serviceProvider.New<BasketService>().GetCustomerBasket(customer.Id);
    }

    [HttpDelete("clear")]
    public async Task ClearCustomerBasket()
    {
      var customer = await GetCustomer();

      await _serviceProvider.New<BasketService>().ClearCustomerBasket(customer.Id);
    }

    private async Task<Customer> GetCustomer()
    {
      var userId = Request.Cookies["user-id"];

      if (!string.IsNullOrEmpty(userId))
      {
        var customer = await _serviceProvider.New<CustomerService>().GetCustomerById(new Guid(userId));

        if (customer == null)
        {
          customer = new Customer
          {
            Id = new Guid(userId),
            Name = "Guest"
          };

          await _serviceProvider.New<CustomerService>().AddCustomer(customer);
        }

        return customer;
      }
      else
      {
        var customer = new Customer
        {
          Name = "Guest"
        };

        await _serviceProvider.New<CustomerService>().AddCustomer(customer);

        Response.Cookies.Append("user-id", customer.Id.ToString());
        return customer;
      }
    }

  }
}
