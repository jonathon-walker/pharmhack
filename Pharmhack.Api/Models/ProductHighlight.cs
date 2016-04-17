using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharmhack.Api.Penises;

namespace Pharmhack.Api.Models
{
    public class ProductHighlight
    {
        public SalesProductHighlight HighestSellingProduct { get; set; }
        public SalesProductHighlight LowestSellingProduct { get; set; }
        public TrendProductHighlight HighestTrendingProduct { get; set; }
        public TrendProductHighlight LowestTrendingProduct { get; set; }
    }
}