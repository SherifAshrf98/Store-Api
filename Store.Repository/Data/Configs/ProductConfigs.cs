using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Configs
{
	internal class ProductConfigs : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(p => p.Description)
				.IsRequired();

			builder.Property(p => p.PictureUrl)
				.IsRequired();

			builder.Property(p => p.Price)
				.HasColumnType("decimal(18,2)");

			builder.HasOne(p => p.Brand)
				.WithMany()
				.HasForeignKey(P => P.BrandId);

			builder.HasOne(p => p.Category)
				.WithMany()
				.HasForeignKey(p => p.CategoryId);

		}
	}
}
