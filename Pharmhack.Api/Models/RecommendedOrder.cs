using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmhack.Api.Models
{
	public class RecommendedOrder
	{
		public Product Product { get; set; }
		public int SuggestedQuantity { get; set; }
	}
}