using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pharmhack.Api.Models;

namespace Pharmhack.Api.Penises
{
    public class TrendProductHighlight
    {
        public Product Product { get; set; }
        public SalesByPeriod[] SalesOverTime { get; set; }
    }
}