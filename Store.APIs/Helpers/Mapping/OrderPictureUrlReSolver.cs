using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Entities.Order_Aggregate;

namespace Store.APIs.Helpers.Mapping
{
	public class OrderPictureUrlReSolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderPictureUrlReSolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{_configuration["APiBaseUrl"]}{source.Product.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
