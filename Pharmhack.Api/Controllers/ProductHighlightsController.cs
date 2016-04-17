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

        public IHttpActionResult Get(DateTime from, DateTime to)
        {
            var salesInPeriod = fred.RetailTransactionSalesTrans.Where(x => x.CostAmount > 0)
                                    .GroupBy(x => x.Barcode)
                                    .Select(g => new {g.Key, Count = g.Count()})
                                    .OrderBy(f => f.Count);

            salesInPeriod.First();
            //var highestSales = new SalesProductHighlight {Product = '3'};

		    var result = new ProductHighlight()
		        {
		        };

			return Ok(result);
		}
	}
}
