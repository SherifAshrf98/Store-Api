using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Core;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications
{
	internal static class SpecificationEvaluator<T> where T : BaseEntity
	{
		public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> spec)
		{
			var Query = InputQuery;

			if (spec.Criteria is not null)
				Query = Query.Where(spec.Criteria);

			if (spec.OrderBy is not null)
				Query = Query.OrderBy(spec.OrderBy);

			else if (spec.OrderByDesc is not null)
				Query = Query.OrderByDescending(spec.OrderByDesc);

			if (spec.IsPaginationEnabled)
				Query = Query.Skip(spec.Skip).Take(spec.Take);

			Query = spec.Includes.Aggregate(Query, (CurrentExpresion, IncludeExpression) => CurrentExpresion.Include(IncludeExpression));

			return Query;
		}
	}
}
