using Checkout.Data.Mapping;
using Checkout.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Checkout.Data
{
  public class CheckoutContext : DbContext
  {
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Product> Products { get; set; }

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

      modelBuilder.ApplyConfiguration(new CustomerMapping());
      modelBuilder.ApplyConfiguration(new BasketMapping());
      modelBuilder.ApplyConfiguration(new ItemMapping());
      modelBuilder.ApplyConfiguration(new ProductMapping());

      modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Tin of beans", Description = "A tin of beans", Price = 0.99M });
      modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Wrench", Description = "A tool", Price = 8.99M });
      modelBuilder.Entity<Product>().HasData(new Product { Id = Guid.NewGuid(), Name = "Shampoo", Description = "A bottle of shampoo", Price = 3.99M });
    }

  }
}
