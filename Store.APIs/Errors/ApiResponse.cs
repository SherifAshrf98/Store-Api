
namespace Store.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { set; get; }
		public string? Message { set; get; }

		public ApiResponse(int statusCode, string? message = null)
		{
			StatusCode = statusCode;

			Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
		}

		private string? GetDefaultMessageForStatusCode(int? statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request",
				401 => "your are not Authorized",
				404 => "Source not Found",
				500 => "Internal Server Error",
				_ => null
			};
		}
	}
}