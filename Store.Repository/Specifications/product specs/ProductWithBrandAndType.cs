﻿using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications.product_specs
{
	public class ProductWithBrandAndType : BaseSpecifications<Product>
	{
		public ProductWithBrandAndType(ProductSpecParams specParams) : base
			(
			  p => (!specParams.brandid.HasValue || p.BrandId == specParams.brandid.Value) && (!specParams.categoryid.HasValue || p.CategoryId == specParams.categoryid.Value)
			)
		{
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);

			if (!string.IsNullOrEmpty(specParams.sort))
			{
				switch (specParams.sort)
				{
					case "PriceAsc":
					case "priceasc":
						OrderBy = (P => P.Price);
						break;

					case "PriceDesc":
					case "pricedesc":
						OrderByDesc = (P => P.Price);
						break;

					default:
						OrderBy = (P => P.Name);
						break;
				}
			}
			else
				OrderBy = (P => P.Name);

		}

		public ProductWithBrandAndType(int id) : base(P => P.Id == id)
		{
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);
		}

	}
}







