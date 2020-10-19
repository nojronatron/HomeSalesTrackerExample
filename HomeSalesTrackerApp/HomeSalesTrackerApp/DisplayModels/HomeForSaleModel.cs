using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeForSaleModel : HomeDisplayModel
    {
        public int HomeForSaleID { get; set; }
        public Decimal SaleAmount { get; set; }
        public DateTime MarketDate { get; set; }
        private RealEstateCompanyView _reco = new RealEstateCompanyView();

        public HomeForSaleModel() { }

        public override string ToString()
        {
            return $"{ base.ToString() }{ this.SaleAmount:C0}{ this.MarketDate:D}";
        }

        public override string ToStackedString()
        {
            return $"{ base.ToStackedString() }\n" +
                $"Sale Amount: { this.SaleAmount:C0}\n" +
                $"Market Date: { this.MarketDate:D}";
        }
    }

}
