using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities.Identity;
using Store.Repository.Identity;
using System.Text;

namespace Store.APIs.Extensions
{
	public static class IdentityServicesExtension
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddIdentity<AppUser, IdentityRole>()
							  .AddEntityFrameworkStores<AppIdentityDbContext>();

			services.AddAuthentication((AuthOptions) =>
			{
				AuthOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				AuthOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer((JwtBearerOptions) =>
			{
				JwtBearerOptions.RequireHttpsMetadata = false;
				JwtBearerOptions.SaveToken = false;
				JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["Jwt:Issuer"],
					ValidAudience = configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
				};
			});

			return services;
		}
	}
}
