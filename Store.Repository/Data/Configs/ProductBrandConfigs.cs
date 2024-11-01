﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Data.Configs
{
	internal class ProductBrandConfigs : IEntityTypeConfiguration<ProductBrand>
	{
		public void Configure(EntityTypeBuilder<ProductBrand> builder)
		{
			builder.Property(b => b.Name)
				.IsRequired();
		}
	}
}