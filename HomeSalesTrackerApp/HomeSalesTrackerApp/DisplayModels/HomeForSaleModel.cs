using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeForSaleModel : HomeDisplayModel
    {
        public int HomeForSaleID { get; set; }
        public Decimal SaleAmount { get; set; }
        public DateTime MarketDate { get; set; }

        public HomeForSaleModel() { }

        private string FormatZip()
        {
            if (base.Zip.Length > 0)
            {
                return $"{ base.Zip.Substring(0, 5) }-{ base.Zip.Substring(5, 4) }";
            }
            return $"! { base.Zip } !";
        }

        public override string ToString()
        {
            return $"{ base.ToString() }{ this.SaleAmount:C0}{ this.MarketDate:D}";
        }

        public override string ToStackedString()
        {
            return $"HomeID: { base.HomeID }\n" +
                $"Address: { base.Address }\n" +
                $"City: { base.City }\n" +
                $"State: { base.State }\n" +
                $"Zip: { FormatZip() }\n" +
                $"Market Date: { this.MarketDate:D}\n" +
                $"Sale Amount: { this.SaleAmount:C0}";
        }

    }
}
