using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Configs
{
	internal class OrderItemConfigs : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
		
			builder.OwnsOne(oi => oi.Product);
			builder.Property(oi => oi.Price).HasColumnType("decimal(18,2)");
		}
	}
}
