using Checkout.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkout.Data.Mapping
{
  public class BasketMapping : IEntityTypeConfiguration<Basket>
  {
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).ValueGeneratedOnAdd();

      builder.HasMany(x => x.Items).WithOne(x => x.Basket).HasForeignKey(x => x.BasketId);
    }
  }
}
