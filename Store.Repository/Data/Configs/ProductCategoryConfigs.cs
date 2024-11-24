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
	internal class ProductCategoryConfigs : IEntityTypeConfiguration<ProductCategory>
	{
		public void Configure(EntityTypeBuilder<ProductCategory> builder)
		{
			builder.Property(c => c.Name)
				.IsRequired();
		}
	}
}
