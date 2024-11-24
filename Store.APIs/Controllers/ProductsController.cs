using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.APIs.Helpers;
using Store.Core.Entities;
using Store.Core.Repositories.Contracts;
using Store.Repository.Specifications.product_specs;

namespace Store.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsRepo;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IGenericRepository<ProductCategory> _categoryRepo;
		private readonly IMapper _mapper;

		public ProductsController(
			IGenericRepository<Product> ProductsRepo,
			IGenericRepository<ProductBrand> BrandRepo,
			IGenericRepository<ProductCategory> CategoryRepo,
			IMapper mapper)
		{
			_productsRepo = ProductsRepo;
			_brandRepo = BrandRepo;
			_categoryRepo = CategoryRepo;
			_mapper = mapper;
		}


		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetAllProducts([FromQuery]ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndType(specParams);

			var products = await _productsRepo.GetAllWithSpecAsync(spec);

			var CountSpec = new ProductFiltrationForCountSpecs(specParams);

			var count = await _productsRepo.GetCountAsync(CountSpec);

			var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageSize, specParams.PageIndex,count, mappedProduct));
		}



		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProductsById(int id)
		{
			var spec = new ProductWithBrandAndType(id);

			var product = await _productsRepo.GetByIdWithSpecAsync(spec);

			if (product == null)
			{

				return NotFound(new ApiResponse(404));
			}

			var mappedProduct = _mapper.Map<Product, ProductToReturnDto>(product);

			return Ok(mappedProduct);
		}



		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetBrands()
		{
			var Brands = await _brandRepo.GetAllAsync();

			return Ok(Brands);
		}



		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var Categories = await _categoryRepo.GetAllAsync();

			return Ok(Categories);
		}
	}
}
