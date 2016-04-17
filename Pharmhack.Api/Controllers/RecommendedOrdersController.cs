using System;
using System.Linq;
using System.Web.Http;
using Pharmhack.Api.Models;
using Pharmhack.OData;
using Pharmhack.OData.FredNXT.Web.Model;

namespace Pharmhack.Api.Controllers
{
	public class RecommendedOrdersController : ApiController
	{
		readonly FredClient fred;

		public RecommendedOrdersController(FredClient fred)
		{
			this.fred = fred;
		}

		public IHttpActionResult Get(DateTime dateFrom, DateTime dateTo)
		{
			var orderPeriodDuration = dateTo - dateFrom;
			var lastPeriodFrom = dateFrom - orderPeriodDuration;
			var lastPeriodTo = dateFrom;

			var potentialRestocks = fred.RetailTransactionSalesTrans
				.WhereBetweenDates(x => x.CreatedDateTime, lastPeriodFrom, lastPeriodTo)
				.Select(x => new
				{
					x.ItemId,
					x.Barcode,
					x.CostAmount,
					x.Price
				})
				.ToArray()
				.GroupBy(x => new { x.ItemId, x.Barcode })
				.Select(x => new
				{
					ItemId = x.Key.ItemId,
					Barcode = x.Key.Barcode,
					TotalCost = x.Sum(g => g.CostAmount),
					TotalSalesValue = x.Sum(g => g.Price),
					TotalSales = x.Count()
				});

			var inventorySummaries = fred.InventSums
				.WhereBetweenDates(x => x.ModifiedDateTime, lastPeriodFrom, lastPeriodTo)
				.Select(x => new
				{
					x.ItemId,
					x.AvailPhysical,
					x.AvailOrdered,
					x.PhysicalInvent
				})
				.ToArray();

			var productIds = potentialRestocks.Select(x => x.ItemId).ToArray();

			productIds = productIds
				.Concat(inventorySummaries.Select(x => x.ItemId))
				.Distinct()
				.ToArray();

			var products = fred.EcoresProducts
				.Select(x => new
				{
					x.DisplayProductNumber,
					x.SearchName
				})
				.ToArray();

			var recommendedRestocks = potentialRestocks
				.GroupJoin(inventorySummaries,
					r => r.ItemId, i => i.ItemId,
					(restock, summaries) => new
					{
						restock.ItemId,
						restock.Barcode,
						LastPeriodSales = restock.TotalSales,
						LastPeriodSalesValue = restock.TotalSalesValue,
						LastPeriodCost = restock.TotalCost,
						CurrentStockOnHand = (int)summaries.Sum(x => x.AvailPhysical + x.AvailOrdered)
					})
				.Where(x => x.CurrentStockOnHand <= x.LastPeriodSales)
				.Select(x => new
				{
					x.ItemId,
					x.Barcode,
					SuggestedQuantity = x.LastPeriodSales - x.CurrentStockOnHand,
					x.LastPeriodSales,
					x.LastPeriodSalesValue,
					x.LastPeriodCost,
					x.CurrentStockOnHand
				})
				.ToArray();

			var result = recommendedRestocks
				.Join(products,
					r => r.ItemId, p => p.DisplayProductNumber,
					(r, p) => new RecommendedOrder
					{
						Product = new Product { Name = p.SearchName, Sku = r.Barcode },
						SuggestedQuantity = r.SuggestedQuantity,
						ExpectedCost = (r.LastPeriodCost / r.LastPeriodSales) * r.SuggestedQuantity,
						LastPeriodSales = r.LastPeriodSales,
						LastPeriodSalesValue = r.LastPeriodSalesValue,
						CurrentStockOnHand = r.CurrentStockOnHand
					})
				.Where(x => x.ExpectedProfit > 0 && x.ExpectedCost > 0)
				.OrderByDescending(x => x.SuggestedQuantity)
				.ThenByDescending(x => x.ExpectedProfit);

			return Ok(result);
		}
	}
}
