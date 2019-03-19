using Checkout.Data;
using Checkout.Domain;
using Checkout.Models;
using Checkout.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Utils;

namespace Checkout.Api.Controllers
{
  [Route("api/basket")]
  [ApiController]
  public class BasketController : ControllerBase
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly CheckoutContext _checkoutContext;

    public BasketController(IServiceProvider serviceProvider, CheckoutContext checkoutContext)
    {
      _serviceProvider = serviceProvider;
      _checkoutContext = checkoutContext;
    }

    [HttpPut("addItem")]
    public async Task AddItem([FromBody] AddItemRequest request)
    {
      if (request == null)
        BadRequest();

      var customer = await GetCustomer();

      await _serviceProvider.New<BasketService>().PutItemInBasket(customer.Id, request);
    }

    [HttpGet("getCustomerBasket")]
    public async Task<Basket> GetCustomerBasket()
    {
      var customer = await GetCustomer();

      return await _serviceProvider.New<BasketService>().GetCustomerBasket(customer.Id);
    }


    [HttpDelete]
    public void ClearCookies()
    {
      Response.Cookies.Delete("user-id");
    }


    private async Task<Customer> GetCustomer()
    {
      var userId = Request.Cookies["user-id"];

      if (string.IsNullOrEmpty(userId))
      {
        var customer = new Customer
        {
          Id = Guid.NewGuid(),
          Name = "Guest"
        };

        await _serviceProvider.New<CustomerService>().AddCustomer(customer);

        Response.Cookies.Append("user-id", customer.Id.ToString());
        return customer;
      }
      else
      {
        var customer = await _serviceProvider.New<CustomerService>().GetCustomerById(new Guid(userId));

        return customer;
      }
    }

  }
}
