using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeSearchView : HomeDisplayModel
    {
        public DateTime? MarketDate { get; set; }

        public override string ToString()
        {
            return $"{ this.HomeID }{ this.Address }{ this.City }{ this.State }{ this.Zip }";
        }

        public override string ToStackedString()
        {
            if (MarketDate != null)
            {
                return $"{ base.ToStackedString() }\n" +
                    $"Market Date: { this.MarketDate:D}";
            }
            return $"{ base.ToStackedString() }\n" +
                $"Market Date: (Home not for sale)";

        }

    }

}
