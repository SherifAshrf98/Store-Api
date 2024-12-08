using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Core.Entities.Order_Aggregate;

namespace Store.APIs.Helpers.Mapping
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
				.ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
				.ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureURLReSolver>());

			CreateMap<BasketItemDto, BasketItem>();

			CreateMap<CustomerBasketDto, CustomerBasket>();

			CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

			CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();

			CreateMap<Order, OrderToReturnDto>()
				.ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
				.ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
				.ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
				.ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
				.ForMember(d => d.PictureUrl, o => o.MapFrom<OrderPictureUrlReSolver>());
		}
	}
}
