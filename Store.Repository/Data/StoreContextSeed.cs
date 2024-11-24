using Store.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Data
{
	public static class StoreContextSeed
	{
		public static async Task SeedAsync(StoreContext DbContext)
		{
			if (DbContext.ProductBrands.Count() == 0)
			{

				var brandsdata = File.ReadAllText("../Store.Repository/Data/DataSeed/brands.json");

				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

				if (brands?.Count > 0)
				{
					foreach (var brand in brands)
					{
						DbContext.Set<ProductBrand>().Add(brand);
					}
					await DbContext.SaveChangesAsync();
				}
			}

			if (DbContext.ProductCategories.Count() == 0)
			{

				var Categoriesdata = File.ReadAllText("../Store.Repository/Data/DataSeed/types.json");

				var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(Categoriesdata);

				if (Categories?.Count > 0)
				{
					foreach (var Category in Categories)
					{
						DbContext.Set<ProductCategory>().Add(Category);
					}
					await DbContext.SaveChangesAsync();
				}
			}

			if (DbContext.Products.Count() == 0)
			{

				var productsdata = File.ReadAllText("../Store.Repository/Data/DataSeed/products.json");

				var products = JsonSerializer.Deserialize<List<Product>>(productsdata);

				if (products?.Count > 0)
				{
					foreach (var product in products)
					{
						DbContext.Set<Product>().Add(product);
					}
					await DbContext.SaveChangesAsync();
				}
			}
		}
	}
}
