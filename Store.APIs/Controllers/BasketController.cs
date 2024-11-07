using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.Entities;
using Store.Core.Repositories.Contracts;

namespace Store.APIs.Controllers
{
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;

		public BasketController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository;
		}

		[HttpGet("{Id}")]
		public async Task<ActionResult<CustomerBasket?>> GetBasket(string Id)
		{
			var Basket = await _basketRepository.GetBasketAsync(Id);

			return Basket is null ? new CustomerBasket(Id) : Ok(Basket);
		}

		[HttpPost("Update")]
		public async Task<ActionResult<CustomerBasket?>> UpdateBasket([FromBody] CustomerBasket? customerBasket)
		{
			if (customerBasket is null) return BadRequest(new ApiResponse(400));

			var UpdatedOrCreatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

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
