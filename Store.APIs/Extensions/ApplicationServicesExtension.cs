using Store.APIs.Helpers.Mapping;
using Store.Core;
using Store.Core.Repositories.Contracts;
using Store.Core.Services.Contracts;
using Store.Repository;
using Store.Repository.Repositories;
using Store.Services;

namespace Store.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

			services.AddScoped(typeof(ITokenService), typeof(TokenService));

			services.AddScoped(typeof(IOrderService), typeof(OrderService));

			services.AddAutoMapper(typeof(MappingProfiles));

			return services;
		}
	}
}
