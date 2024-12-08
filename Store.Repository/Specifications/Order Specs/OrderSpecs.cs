using Store.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications.Order_Specs
{
	public class OrderSpecs : BaseSpecifications<Order>
	{
		public OrderSpecs(string email) : base(O => O.BuyerEmail == email)
		{
			Includes.Add(o => o.DeliveryMethod);
			Includes.Add(o => o.Items);
			OrderByDesc = o => o.OrderDate;
		}

		public OrderSpecs(string email, int id) : base(O => O.BuyerEmail == email && O.Id == id)
		{
			Includes.Add(o => o.DeliveryMethod);
			Includes.Add(o => o.Items);
		}
	}
}