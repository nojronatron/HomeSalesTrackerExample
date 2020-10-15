using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.Report_Models
{
    public class SoldHomesReportModel :
        IEquatable<SoldHomesReportModel>, IEqualityComparer<SoldHomesReportModel>
    {
        private int _homeID;

        public int HomeID
        {
            get { return _homeID; }
            set
            {
                if (this._homeID != value)
                {
                    _homeID = value;
                }
            }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                if (this._address != value)
                {
                    _address = value;
                }
            }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set
            {
                if (this._city != value)
                {
                    _city = value;
                }
            }
        }

        private string _state;

        public string State
        {
            get { return _state; }
            set
            {
                if (this._state != value)
                {
                    _state = value;
                }
            }
        }

        private string _zip;

        public string Zip
        {
            get
            {
                return $"{ _zip.Substring(0, 5)}-{ _zip.Substring(5, 4) }";
            }
            set
            {
                if (this._zip != value)
                {
                    _zip = value;
                }
            }
        }


        private string _buyerFirstName;

        public string BuyerFirstName
        {
            get { return _buyerFirstName; }
            set
            {
                if (this._buyerFirstName != value)
                {
                    _buyerFirstName = value;
                }
            }
        }

        private string _buyerLastName;

        public string BuyerLastName
        {
            get { return _buyerLastName; }
            set
            {
                if (this._buyerLastName != value)
                {
                    _buyerLastName = value;
                }
            }
        }

        private string _agentFirstName;

        public string AgentFirstName
        {
            get { return _agentFirstName; }
            set
            {
                if (this._agentFirstName != value)
                {
                    _agentFirstName = value;
                }
            }
        }

        private string _agentLastName;

        public string AgentLastName
        {
            get { return _agentLastName; }
            set
            {
                if (this._agentLastName != value)
                {
                    _agentLastName = value;
                }
            }
        }

        private string _companyName;

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (this._companyName != value)
                {
                    _companyName = value;
                }
            }
        }

        private decimal _saleAmount;

        public decimal SaleAmount
        {
            get { return _saleAmount; }
            set
            {
                if (this._saleAmount != value)
                {
                    this._saleAmount = value;
                }
            }
        }

        private DateTime? _soldDate;

        public DateTime? SoldDate
        {
            get { return _soldDate; }
            set
            {
                if (this._soldDate != value)
                {
                    this._soldDate = value;
                }
            }
        }

        public string BuyerFullName
        {
            get
            {
                return $"{ this._buyerFirstName } { this._buyerLastName }";
            }
        }

        public string AgentFullName
        {
            get
            {
                return $"{ this._agentFirstName } { this._agentLastName }";
            }
        }

        public bool Equals(SoldHomesReportModel other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return this.Address.Equals(other.Address) && this.Zip.Equals(other.Zip) &&
                this.SaleAmount.Equals(other.SaleAmount) && this.SoldDate.Equals(other.SoldDate);
        }

        public bool Equals(SoldHomesReportModel x, SoldHomesReportModel y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.Address == y.Address && x.Zip == y.Zip && 
                x.SaleAmount == y.SaleAmount && x.SoldDate == y.SoldDate;
        }

        public int GetHashCode(SoldHomesReportModel obj)
        {
            return (this.Address + this.Zip + this.SaleAmount + this.SoldDate).GetHashCode();
        }

        public override int GetHashCode()
        {
            var hashCode = 903327883;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
            hashCode = hashCode * -1521134295 + SaleAmount.GetHashCode();
            hashCode = hashCode * -1521134295 + SoldDate.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SoldHomesReportModel left, SoldHomesReportModel right)
        {
            return EqualityComparer<SoldHomesReportModel>.Default.Equals(left, right);
        }

        public static bool operator !=(SoldHomesReportModel left, SoldHomesReportModel right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return this.GetType() == obj.GetType();
        }
    }
}
