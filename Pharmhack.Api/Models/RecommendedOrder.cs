using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmhack.Api.Models
{
	public class RecommendedOrder
	{
		public int CurrentStockOnHand { get; internal set; }
		public decimal ExpectedCost { get; set; }
		public decimal ExpectedProfit
		{
			get { return LastPeriodSalesValue - ExpectedCost; }
		}

		public int LastPeriodSales { get; internal set; }
		public decimal LastPeriodSalesValue { get; internal set; }
		public Product Product { get; set; }
		public int SuggestedQuantity { get; set; }
	}
}