using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
		public BasketRepository(IConnectionMultiplexer Redis)
		{
			_database = Redis.GetDatabase();
		}

		public async Task<CustomerBasket?> GetBasketAsync(string BasketID)
		{
			var JsonBasket = await _database.StringGetAsync(BasketID);

			return JsonBasket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(JsonBasket);
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
		{
			var JsonBasket = JsonSerializer.Serialize(Basket);

			var Success = await _database.StringSetAsync(Basket.Id, JsonBasket, TimeSpan.FromDays(1));

			if (!Success) return null;

			return await GetBasketAsync(Basket.Id);
		}

		public async Task<bool> DeleteBasketAsync(string BasketID)
		{
			return await _database.KeyDeleteAsync(BasketID);
		}
	}
}
