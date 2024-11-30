using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order_Aggregate;

namespace Store.Repository.Data.Configs
{
	internal class OrderConfigs : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{

			builder.OwnsOne(o => o.ShippingAddress);

			builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");

			builder.Property(o => o.Status).HasConversion((status) => status.ToString(), (status) => (OrderStatus)Enum.Parse(typeof(OrderStatus), status));

			builder.HasOne(o => o.DeliveryMethod)
				.WithMany()
				.HasForeignKey("DeliveryMethodId")
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(o => o.Items)
				.WithOne()
				.HasForeignKey("OrderId")
				.OnDelete(DeleteBehavior.NoAction);

		}
	}
}
