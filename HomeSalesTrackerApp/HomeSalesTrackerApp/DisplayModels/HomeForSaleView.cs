using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeForSaleView : HomeView
    {
        public Decimal SaleAmount { get; set;}
        public DateTime MarketDate { get; set; }
        public HomeForSaleView() { }

        public override string ToString()
        {
            return $"{ base.ToString() }{ this.SaleAmount:C2}{ this.MarketDate }";
        }
    }

}
