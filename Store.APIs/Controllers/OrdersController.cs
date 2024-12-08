using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Store.APIs.DTOs;
using Store.APIs.Errors;
using Store.Core;
using Store.Core.Entities.Order_Aggregate;
using Store.Core.Services.Contracts;
using System.Security.Claims;
using Order = Store.Core.Entities.Order_Aggregate.Order;

namespace Store.APIs.Controllers
{
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_orderService = orderService;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var MappedAddress = _mapper.Map<Address>(orderDto.ShippingAddress);

			var order = await _orderService.CreateOrderAsync(email, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);

			if (order == null) return BadRequest(new ApiResponse(400, "something wrong with creating your order please try again"));

			return Ok(order);
		}

		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
		[HttpGet]
		[Authorize]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrders()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var orders = await _orderService.GetAllOrdersForAUserAsync(email);

			if (orders == null) return NotFound(new ApiResponse(404, "No orders Found For This User"));

			var mappedOrder = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

			return Ok(mappedOrder);
		}

		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[HttpGet("{Id}")]
		[Authorize]
		public async Task<ActionResult<OrderToReturnDto>> GetOrder(int id)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var order = await _orderService.GetOrderByIdForAUserAsync(email, id);

			if (order == null) return NotFound(new ApiResponse(404, "No orders Found With This Id"));

			var mappedOrder = _mapper.Map<OrderToReturnDto>(order);

			return Ok(mappedOrder);
		}

		[HttpGet("DeliveryMethods")]
		[Authorize]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

			if (DeliveryMethods == null) return NotFound(new ApiResponse(404, "No Delivery Methods Found"));

			return Ok(DeliveryMethods);
		}
	}
}
