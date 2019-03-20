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

    /// <summary>
    /// Adds the specified number of Products to the Customer's Basket, provided that the Product exists and that the specified quantity is more than 0.
    /// If no Basket exists for the customer, a new one is created.
    /// </summary>
    /// <param name="request">An object containing the Product Id and the quantity of said Product to add.</param>
    /// <returns>An HTTP OK status if the item is added successfully or an HTTP Bad Request status if the Product does not exist or the request body is malformed.</returns>
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

    /// <summary>
    /// Deletes the specified number of Products from the Customer's Basket, or all instances of the given Product if the quantity is greater than that of Products in the Basket.
    /// Alternatively, to remove all items without knowing the quantity to remove, providing the value 'true' to the argument 'removeAll' will also delete all instances of the given Product.
    /// </summary>
    /// <param name="request">An object containing the Product Id, the quantity to delete (optional) and a flag to remove all items (optional)</param>
    /// <returns>An HTTP OK status or an HTTP Bad Request status if the request body is malformed.</returns>
    [HttpDelete("items")]
    public async Task<IActionResult> RemoveItems([FromBody] RemoveItemsRequest request)
    {
      if (request == null)
        return BadRequest();

      var customer = await GetCustomer();

      await _serviceProvider.New<BasketService>().RemoveItemsFromBasket(customer.Id, request);

      return Ok();
    }

    /// <summary>
    /// Retrieves the Customer's Basket and all of its contents, including the Product details for each Item.
    /// </summary>
    /// <returns>The Basket object, or an HTTP No Content status if no basket exists</returns>
    [HttpGet]
    public async Task<Basket> GetCustomerBasket()
    {
      var customer = await GetCustomer();

      return await _serviceProvider.New<BasketService>().GetCustomerBasket(customer.Id);
    }

    /// <summary>
    /// Removes all Items from a Customer's Basket. Does not delete the Basket itself.
    /// </summary>
    [HttpDelete("clear")]
    public async Task ClearCustomerBasket()
    {
      var customer = await GetCustomer();

      await _serviceProvider.New<BasketService>().ClearCustomerBasket(customer.Id);
    }

    /// <summary>
    /// Attempts to retrieve a Customer from the Database by the user ID in stored in the Cookies.
    /// If the Cookies do not contain a user ID, a new Customer is created and its ID is added to the Cookies.
    /// If the Database does not contain a Customer for a given user ID, one will be added.
    /// </summary>
    /// <returns>A Customer from the Database</returns>
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
