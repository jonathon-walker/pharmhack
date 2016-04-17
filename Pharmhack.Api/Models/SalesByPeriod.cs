using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmhack.Api.Models
{
    public class SalesByPeriod
    {
        public int Period { get; set; }
        public int TotalSales { get; set; }
    }
}