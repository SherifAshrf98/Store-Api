using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Entities;
using Store.Core.Entities.Identity;

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


			CreateMap<Address, AddressDto>().ReverseMap();

			CreateMap<CustomerBasketDto, CustomerBasket>();

			CreateMap<BasketItemDto, BasketItem>();
		}
	}
}
