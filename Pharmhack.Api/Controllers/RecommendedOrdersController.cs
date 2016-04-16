using System;
using System.Web.Http;
using Pharmhack.OData;

namespace Pharmhack.Api.Controllers
{
	public class RecommendedOrdersController : ApiController
	{
		readonly FredClient fred;

		public RecommendedOrdersController(FredClient fred)
		{
			this.fred = fred;
		}

		public IHttpActionResult Get(DateTime from, DateTime to)
		{
			
		}
	}
}
