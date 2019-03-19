using Checkout.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Checkout.Data
{
  public class CheckoutContext : DbContext
  {
    public DbSet<Customer> Customers { get; set; }

    public CheckoutContext(DbContextOptions<CheckoutContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      
    }

  }
}
