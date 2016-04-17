using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Pharmhack.Api.Models;
using Pharmhack.OData;

namespace Pharmhack.Api.Penises
{
	public class ProductHighlightsController : ApiController
	{
		readonly FredClient fred;

		public ProductHighlightsController(FredClient fred)
		{
			this.fred = fred;
		}

		public IHttpActionResult Get(DateTime dateFrom, DateTime dateTo)
		{
			var orderPeriodDuration = dateTo - dateFrom;
			var lastPeriodFrom = dateFrom - orderPeriodDuration;
			var lastPeriodTo = dateFrom;

			var previousPeriodFrom = lastPeriodFrom - orderPeriodDuration;
			var previousPeriodTo = lastPeriodTo - orderPeriodDuration;

			var salesForLastPeriod = fred.RetailTransactionSalesTrans
				.WhereBetweenDates(x => x.CreatedDateTime, lastPeriodFrom, lastPeriodTo)
				.Select(x => new
				{
					x.ItemId,
					x.Barcode
				})
				.ToArray()
				.GroupBy(x => new { x.ItemId, x.Barcode })
				.Select(x => new
				{
					ItemId = x.Key.ItemId,
					Barcode = x.Key.Barcode,
					TotalSales = x.Count()
				});
			
			var products = fred.EcoresProducts
				.Select(x => new
				{
					x.DisplayProductNumber,
					x.SearchName
				})
				.ToArray();

			var salesRanked = salesForLastPeriod
				.OrderBy(s => s.TotalSales)
				.Join(products, s => s.ItemId, p => p.DisplayProductNumber,
				      (s, p) => new SalesProductHighlight()
					      {
						      Product = new Product() {Name = p.SearchName, Sku = s.Barcode},
						      TotalSales = s.TotalSales
					      });

			/*
				.GroupBy(x => x.Sku)
				.Select(g => new
					{
						g.Key, 
						salesProductHighlight = new SalesProductHighlight
							{
								Product = g.First(), 
								TotalSales = g.Count()
							}
					})
				.OrderBy(f => f.salesProductHighlight.TotalSales)
				.ToArray();
			*/
			var highestSales = salesRanked.Last();
			var lowestSales = salesRanked.First();



			//var salesforLastTwoPeriods = fred.RetailTransactionSalesTrans
			//	.WhereBetweenDates(x => x.CreatedDateTime, previousPeriodFrom, lastPeriodTo)
			//	.Select(x => new
			//	{
			//		x.ItemId,
			//		x.CreatedDateTime,
			//		x.CostAmount,
			//		x.Price
			//	})
			//	.GroupBy(x => x.ItemId)
			//	.Select(g => new { g.Key, Count = g.Count() })
			//	.OrderBy(f => f.Count)
			//	.ToArray();
			//var salesInPeriod = fred.RetailTransactionSalesTrans.Where(x => x.CostAmount > 0);
			//salesInPeriod.First();
			////var highestSales = new SalesProductHighlight {Product = '3'};

			var result = new ProductHighlight() {HighestSellingProduct  = highestSales,
			LowestSellingProduct = lowestSales};

			return Ok(result);
		}
	}
}
