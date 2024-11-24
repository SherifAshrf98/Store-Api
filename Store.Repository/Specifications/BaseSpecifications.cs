using Store.Core;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; }
		public Expression<Func<T, object>> OrderByDesc { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; }
		public bool IsPaginationEnabled { get; set; }

		public BaseSpecifications()
		{

		}

		public BaseSpecifications(Expression<Func<T, bool>>? criteria)
		{
			Criteria = criteria;
		}

		public void ApplyPagination(int skip, int take)
		{
			Skip = skip;
			Take = take;
			IsPaginationEnabled = true;
		}

	}
}
