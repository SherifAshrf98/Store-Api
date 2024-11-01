using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.APIs.Errors;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Store.APIs.MiddleWares
{
	public class ExceptionMiddleWare
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleWare> _logger;
		private readonly IHostEnvironment _env;
		public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}

			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				context.Response.ContentType = "Application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var Response =
					_env.IsDevelopment()
					? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
					: new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				var JsonRsponse = JsonSerializer.Serialize(Response, options);

				await context.Response.WriteAsync(JsonRsponse);
			}
		}

	}
}
