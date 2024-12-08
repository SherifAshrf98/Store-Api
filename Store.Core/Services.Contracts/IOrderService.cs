using Store.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contracts
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
		Task<IReadOnlyList<Order>> GetAllOrdersForAUserAsync(string BuyerEmail);
		Task<Order?> GetOrderByIdForAUserAsync(string BuyerEmail,int OrderID);
	}
}
