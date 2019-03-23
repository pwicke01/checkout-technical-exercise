using Checkout.Data;
using Checkout.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Checkout.Domain
{
  public class CustomerService
  {
    private readonly CheckoutContext _checkoutContext;

    public CustomerService(CheckoutContext checkoutContext)
    {
      _checkoutContext = checkoutContext;
    }

    public async Task AddCustomer(Customer customer)
    {
      await _checkoutContext.Customers.AddAsync(customer);
      await _checkoutContext.SaveChangesAsync();
    }

    public async Task<Customer> GetCustomerById(Guid id)
    {
      return await _checkoutContext.Customers.FindAsync(id);
    }
  }
}
