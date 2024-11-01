
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.APIs.Errors;
using Store.APIs.Helpers;
using Store.APIs.MiddleWares;
using Store.Core.Repositories.Contracts;
using Store.Repository.Data;
using Store.Repository.Repositories;

namespace Store.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var WebApplicationBuilder = WebApplication.CreateBuilder(args);

			var ConnectionString = WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection");

			// Add services to the container.
			#region Configure Services

			WebApplicationBuilder.Services.AddControllers();

			WebApplicationBuilder.Services.AddEndpointsApiExplorer();

			WebApplicationBuilder.Services.AddSwaggerGen();

			WebApplicationBuilder.Services.AddDbContext<StoreContext>(optionsBuilder =>
			{
				optionsBuilder.UseSqlServer(ConnectionString);
			});

			WebApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			WebApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));



			WebApplicationBuilder.Services.Configure<ApiBehaviorOptions>(Options =>
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

			#region DataSeeding/ApplyMigrations

			var Scope = app.Services.CreateScope();

			var Services = Scope.ServiceProvider;

			var _DbContext = Services.GetRequiredService<StoreContext>();

			var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _DbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(_DbContext);
			}
			catch (Exception ex)
			{
				var logger = LoggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error Occured During Apply The Migration");
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

			app.MapControllers();

			app.Run();

			#endregion
		}
	}
}
