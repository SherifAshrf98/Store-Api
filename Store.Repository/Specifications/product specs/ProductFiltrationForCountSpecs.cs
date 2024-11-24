using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications.product_specs
{
	public class ProductFiltrationForCountSpecs : BaseSpecifications<Product>
	{
		public ProductFiltrationForCountSpecs(ProductSpecParams specParams) : base
			(
				 p =>
				 (string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search.ToLower())) &&
				 (!specParams.brandid.HasValue || p.BrandId == specParams.brandid.Value) &&
				 (!specParams.categoryid.HasValue || p.CategoryId == specParams.categoryid.Value)
			)
		{

		}

	}
}
