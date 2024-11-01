using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Entities;

namespace Store.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
				.ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
				.ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureURLReSolver>());

		}
	}
}
