using Microsoft.Extensions.DependencyInjection;
using Store.Core;
using Store.Core.Entities;
using Store.Core.Repositories.Contracts;
using Store.Repository.Data;
using Store.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _storeContext;
		private Hashtable _Repositories;
		public UnitOfWork(StoreContext storeContext)
		{
			_storeContext = storeContext;
			_Repositories = new Hashtable();
		}
		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var Type = typeof(TEntity).Name;

			if (!_Repositories.ContainsKey(Type))
			{
				var repository = new GenericRepository<TEntity>(_storeContext);

				_Repositories.Add(Type, repository);
			}

			return _Repositories[Type] as IGenericRepository<TEntity>;
		}
		public async Task<int> CompleteAsync() => await _storeContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
		{
			await _storeContext.DisposeAsync();
		}

	}
}
