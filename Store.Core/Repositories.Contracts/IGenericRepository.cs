using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories.Contracts
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T?> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();	
		Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
		Task<int> GetCountAsync(ISpecifications<T> spec);
	}
}
