using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;

namespace Store.APIs.Controllers
{
	[Route("Errors/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ControllerBase
	{
		public ActionResult Errors(int code)
		{
			return code switch
			{
				401 => Unauthorized(new ApiResponse(401)),
				404 => NotFound(new ApiResponse(404)),
				_ => StatusCode(code)
			};
		}
	}
}
