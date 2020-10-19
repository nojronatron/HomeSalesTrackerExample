using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class SoldHomeDetailModel : HomeForSaleDetailModel
    {
        public DateTime? SoldDate { get; set; }

        private string FormattedSoldDate
		{
			get
			{
				return $"{ this.SoldDate:D}";
			}
		}

		public override string ToString()
		{
			return $"{ base.ToString() } { FormattedSoldDate }";
		}

		public override string ToStackedString()
		{
			return $"{ base.ToStackedString()}\n" +
				$"Sold Date: { FormattedSoldDate }";
		}
	}
}
