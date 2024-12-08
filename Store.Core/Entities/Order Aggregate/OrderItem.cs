using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{
		public OrderItem()
		{

		}
		public OrderItem(ProductInOrderItem product, decimal price, int quantity)
		{
			Product = product;
			Price = price;
			Quantity = quantity;
		}

		public ProductInOrderItem Product { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }
	}
}
