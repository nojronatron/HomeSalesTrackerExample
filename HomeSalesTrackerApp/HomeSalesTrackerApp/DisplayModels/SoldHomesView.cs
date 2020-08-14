using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class SoldHomesView : HomeView
    {
        public string BuyerFirstLastName { get; set; }
        public string AgentFirstLastName { get; set; }

        public string RealEstateCompanyName { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime? SoldDate { get; set; }

        public override string ToString()
        {
            return $"{ base.ToString()} { BuyerFirstLastName } { AgentFirstLastName } { RealEstateCompanyName } { SaleAmount:C2} { SoldDate:MM/dd/yyyy}";
        }

    }
}
