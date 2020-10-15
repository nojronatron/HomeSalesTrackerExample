using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.Report_Models
{
    /// <summary>
    /// Model Class arranges instances from multiple classes into a single consolidated class for display and UI-interaction.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1?view=netcore-3.1
    /// </summary>
    public class HomesForSaleReportModel : 
        IEquatable<HomesForSaleReportModel>, IEqualityComparer<HomesForSaleReportModel>
    {
        private int _homeID;

        public int HomeID
        {
            get { return _homeID; }
            set
            {
                if (value != this._homeID)
                {
                    this._homeID = value;
                }
            }
        }

        private string _address;
   
        public string Address
        {
            get { return _address; }
            set
            {
                if (value != this._address)
                {
                    this._address = value;
                }
            }
        }
  
        private string _city;
   
        public string City
        {
            get { return _city; }
            set {
                if (value != this._city)
                {
                    this._city = value;
                }
            }
        }
   
        private string _state;
   
        public string State
        {
            get { return _state; }
            set
            {
                if (value != this._state)
                {
                    this._state = value;
                }
            }
        }
   
        private string _zip;
    
        public string Zip
        {
            get
            {
                return $"{ _zip.Substring(0, 5) }-{ _zip.Substring(5, 4) }";
            }
            set
            {
                if (value != this._zip)
                {
                    this._zip = value;
                }
            }
        }

        private string _ownerFullName;

        public string OwnerFullName
        {
            get { return _ownerFullName; }
            set { _ownerFullName = value; }
        }

        private string _ownerPhone;

        public string OwnerPhone
        {
            get { 
                return $"({ _ownerPhone.Substring(0,3) }) { _ownerPhone.Substring(3,3) }-{ _ownerPhone.Substring(6,4) }"; 
            }
            set { _ownerPhone = value; }
        }

        private string _ownerEMail;

        public string OwnerEMail
        {
            get { return _ownerEMail; }
            set { _ownerEMail = value; }
        }
        
        private string _agentFullName;

        public string AgentFullName
        {
            get { return _agentFullName; }
            set { _agentFullName = value; }
        }

        private string _agentPhone;

        public string AgentPhone
        {
            get {
                return $"({ _agentPhone.Substring(0, 3) }) { _agentPhone.Substring(3, 3) }-{ _agentPhone.Substring(6, 4) }";
            }
            set { _agentPhone = value; }
        }

        private string _agentEMail;

        public string AgentEMail
        {
            get { return _agentEMail; }
            set { _agentEMail = value; }
        }

        private string _preferredLender;
     
        public string PreferredLender
        {
            get { return _preferredLender; }
            set
            {
                if (this._preferredLender != value)
                {
                    this._preferredLender = value;
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

        private DateTime _marketDate;

        public DateTime MarketDate
        {
            get { return _marketDate.Date; }
            set
            {
                if (this._marketDate != value)
                {
                    this._marketDate = value;
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

        public bool Equals(HomesForSaleReportModel other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return Address.Equals(other.Address) && Zip.Equals(other.Zip) && 
                SaleAmount.Equals(other.SaleAmount) && MarketDate.Equals(other.MarketDate);
        }

        public bool Equals(HomesForSaleReportModel x, HomesForSaleReportModel y)
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
                x.SaleAmount == y.SaleAmount && x.MarketDate == y.MarketDate; 
        }

        public int GetHashCode(HomesForSaleReportModel obj)
        {
            return (this.Address + this.Zip + this.SaleAmount + this.MarketDate).GetHashCode();
        }

        public override int GetHashCode()
        {
            var hashCode = -63552794;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
            hashCode = hashCode * -1521134295 + MarketDate.GetHashCode();
            hashCode = hashCode * -1521134295 + SaleAmount.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(HomesForSaleReportModel left, HomesForSaleReportModel right)
        {
            return EqualityComparer<HomesForSaleReportModel>.Default.Equals(left, right);
        }

        public static bool operator !=(HomesForSaleReportModel left, HomesForSaleReportModel right)
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
