using Store.APIs.Helpers;
using Store.Core.Repositories.Contracts;
using Store.Repository.Repositories;

namespace Store.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
	
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
			
			services.AddAutoMapper(typeof(MappingProfiles));

			return services;
		}
	}
}
