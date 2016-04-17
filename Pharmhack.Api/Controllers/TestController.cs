using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Pharmhack.OData;
using Pharmhack.OData.Default;

namespace Pharmhack.Api.Penises
{
	public class TestController : ApiController
	{
		readonly FredClient fred;

		public TestController(FredClient fred)
		{
			this.fred = fred;
		}

		public IHttpActionResult Get()
		{
			var things = fred.RetailTransactionSalesTrans.Where(x => x.CostAmount > 0)
				.Take(10)
				.Select(x => new
				{
					x.CostAmount,
					x.Price,
					Difference = x.CostAmount - x.Price
				});
			return Ok(things);
		}
	}
}
