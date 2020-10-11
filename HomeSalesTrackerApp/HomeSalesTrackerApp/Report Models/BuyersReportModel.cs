using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.Report_Models
{
    public class BuyersReportModel :
		IEquatable<BuyersReportModel>, IEqualityComparer<BuyersReportModel>
    {
		private int _buyerID;

		public int BuyerID
		{
			get { return _buyerID; }
			set { _buyerID = value; }
		}

		private string _firstName;

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		private string _lastName;

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		private string _phone;

		public string Phone
		{
			get { return _phone; }
			set { _phone = value; }
		}

		private string _eMail;

		public string EMail
		{
			get { return _eMail; }
			set { _eMail = value; }
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
			get { return _saleDate; }
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
			get { return _zip; }
			set { _zip = value; }
		}

		public string FullName => $"{ this.FirstName } { this.LastName }";

		public override bool Equals(object obj)
		{
			return obj is BuyersReportModel model &&
				   Address == model.Address &&
				   Zip == model.Zip;
		}

		public override int GetHashCode()
		{
			var hashCode = -1748418241;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
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
			return this.Address.Equals(other.Address) && this.Zip.Equals(other.Zip);
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
			return x.Address == y.Address && x.Address == y.Address;
		}

		public int GetHashCode(BuyersReportModel obj)
		{
			return (this.Address + this.Zip).GetHashCode();
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
