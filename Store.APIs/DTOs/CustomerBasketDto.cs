using Store.Core.Entities;

namespace Store.APIs.DTOs
{
	public class CustomerBasketDto
	{
		public string Id { get; set; }
		public List<BasketItemDto> Items { get; set; }

	}
}
