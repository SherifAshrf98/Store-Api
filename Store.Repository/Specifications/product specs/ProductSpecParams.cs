using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specifications.product_specs
{
	public class ProductSpecParams
	{
		private const int maxpagesize = 10;

		private int pagesize;

		public int PageSize
		{
			get { return pagesize; }

			set { pagesize = value > maxpagesize ? maxpagesize : value; }
		}
		public int PageIndex { get; set; } = 1;

		public string? sort { get; set; }

		public int? brandid { get; set; }

		public int? categoryid { get; set; }
	}
}
