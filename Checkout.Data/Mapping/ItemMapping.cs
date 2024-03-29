﻿using Checkout.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkout.Data.Mapping
{
  public class ItemMapping : IEntityTypeConfiguration<Item>
  {
    public void Configure(EntityTypeBuilder<Item> builder)
    {
      builder.HasKey(x => x.Id);

      builder.HasOne(x => x.Product).WithMany(x => x.Items).HasForeignKey(x => x.ProductId);
    }
  }
}
