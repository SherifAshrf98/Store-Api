using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Contracts
{
	public interface IBasketRepository
	{
		Task<CustomerBasket?> GetBasketAsync(string BasketID);
		Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
		Task<bool> DeleteBasketAsync(string BasketID);
	}
}
