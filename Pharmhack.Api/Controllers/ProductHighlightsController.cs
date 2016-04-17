using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Pharmhack.OData;

namespace Pharmhack.Api.Controllers
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

			var salesforLastTwoPeriods = fred.RetailTransactionSalesTrans
				.WhereBetweenDates(x => x.CreatedDateTime, previousPeriodFrom, lastPeriodTo)
				.Select(x => new
				{
					x.ItemId,
					x.CreatedDateTime,
					x.CostAmount,
					x.Price
				})
				.ToArray();

			return Ok();
		}
	}
}
