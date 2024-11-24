using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.Core.Entities;
using Store.Core.Repositories.Contracts;

namespace Store.APIs.Controllers
{
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository, IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}

		[HttpGet("{Id}")]
		public async Task<ActionResult<CustomerBasket?>> GetBasket(string Id)
		{
			var Basket = await _basketRepository.GetBasketAsync(Id);

			return Basket is null ? new CustomerBasket(Id) : Ok(Basket);
		}

		[HttpPost("Update")]
		public async Task<ActionResult<CustomerBasket?>> UpdateBasket([FromBody] CustomerBasketDto? customerBasketdto)
		{
			if (customerBasketdto is null) return BadRequest(new ApiResponse(400));

			var MappedCustomerBasket = _mapper.Map<CustomerBasket>(customerBasketdto);

			var UpdatedOrCreatedBasket = await _basketRepository.UpdateBasketAsync(MappedCustomerBasket);

			if (UpdatedOrCreatedBasket is null) return BadRequest(new ApiResponse(400));

			return Ok(UpdatedOrCreatedBasket);
		}

		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteBasket(string Id)
		{
			return await _basketRepository.DeleteBasketAsync(Id);
		}
	}
}
