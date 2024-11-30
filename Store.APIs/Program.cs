using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.APIs.Errors;
using Store.APIs.Extensions;
using Store.APIs.MiddleWares;
using Store.Core.Entities.Identity;
using Store.Repository.Data;
using Store.Repository.Identity;

namespace Store.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var WebApplicationBuilder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			#region Configure Services

			WebApplicationBuilder.Services.AddEndpointsApiExplorer();
			WebApplicationBuilder.Services.AddSwaggerGen();


			WebApplicationBuilder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
				});

			WebApplicationBuilder.Services.AddIdentityServices(WebApplicationBuilder.Configuration);

			WebApplicationBuilder.Services.AddApplicationServices();

			WebApplicationBuilder.Services.AddDbContext<StoreContext>((optionsBuilder) =>
			{
				optionsBuilder.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			WebApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>((OptionsBuilder) =>
			{
				OptionsBuilder.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});

			WebApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var Connection = WebApplicationBuilder.Configuration.GetConnectionString("RedisConnection");

				return ConnectionMultiplexer.Connect(Connection!);
			});

			WebApplicationBuilder.Services.Configure<ApiBehaviorOptions>((Options) =>
			{
				Options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
					.SelectMany(P => P.Value.Errors)
					.Select(E => E.ErrorMessage)
					.ToList();

					var ValidationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(ValidationErrorResponse);
				};
			});

			#endregion

			#region Build The App
			var app = WebApplicationBuilder.Build();
			#endregion

			#region ApplyMigrations/DataSeeding

			var Scope = app.Services.CreateScope();
			var Services = Scope.ServiceProvider;

			var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
			var logger = LoggerFactory.CreateLogger<Program>();


			var _StoreDbContext = Services.GetRequiredService<StoreContext>();

			var _AppIdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();

			var userManager = Services.GetRequiredService<UserManager<AppUser>>();

			//Migrating
			try
			{
				await _StoreDbContext.Database.MigrateAsync();
				await _AppIdentityDbContext.Database.MigrateAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An Error Occured During Apply The Migration");
			}
			//Seeding
			try
			{
				await StoreContextSeed.SeedAsync(_StoreDbContext);
				await AppIdentityDbContextSeeding.SeedUserAsync(userManager, logger);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An Error Occured During Apply The Seeding");
			}
			#endregion

			#region Configure PipeLines
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<ExceptionMiddleWare>();

			app.UseStatusCodePagesWithReExecute("/Errors/{0}");

			app.UseStaticFiles();

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();

			#endregion
		}
	}
}
