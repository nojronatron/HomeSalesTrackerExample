using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.Report_Models
{
    public class BuyersReportModel :
		PersonBaseModel,
		IEquatable<BuyersReportModel>, IEqualityComparer<BuyersReportModel>
    {
		private int _buyerID;

		public int BuyerID
		{
			get { return _buyerID; }
			set { _buyerID = value; }
		}

		private int? _creditRating;

		public int? CreditRating
		{
			get { return _creditRating; }
			set { _creditRating = value; }
		}

		private DateTime _saleDate;

		public DateTime SaleDate
		{
			get { return _saleDate.Date; }
			set { _saleDate = value; }
		}

		private Decimal _saleAmount;

		public Decimal SaleAmount
		{
			get { return _saleAmount; }
			set { _saleAmount = value; }
		}

		private string _address;

		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}

		private string _city;

		public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		private string _state;

		public string State
		{
			get { return _state; }
			set { _state = value; }
		}

		private string _zip;

		public string Zip
		{
			get
			{
				return $"{ _zip.Substring(0, 5) }-{ _zip.Substring(5, 4) }";
			}
			set { _zip = value; }
		}

		public override bool Equals(object obj)
		{
			return obj is BuyersReportModel model &&
				   base.Equals(obj) &&
				   BuyerID == model.BuyerID;
		}

		public override int GetHashCode()
		{
			var hashCode = -62914997;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + BuyerID.GetHashCode();
			return hashCode;
		}

		public bool Equals(BuyersReportModel other)
		{
			if (Object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (Object.ReferenceEquals(this, other))
			{
				return true;
			}
			return BuyerID.Equals(other.BuyerID);
		}

		public bool Equals(BuyersReportModel x, BuyersReportModel y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			return x.BuyerID == y.BuyerID;
		}

		public int GetHashCode(BuyersReportModel obj)
		{
			return obj.BuyerID.GetHashCode();
		}

		public static bool operator ==(BuyersReportModel left, BuyersReportModel right)
		{
			return EqualityComparer<BuyersReportModel>.Default.Equals(left, right);
		}

		public static bool operator !=(BuyersReportModel left, BuyersReportModel right)
		{
			return !(left == right);
		}

	}
}
