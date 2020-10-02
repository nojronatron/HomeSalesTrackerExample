using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeSalesTrackerApp.Report_Models
{
    /// <summary>
    /// Model Class arranges instances from multiple classes into a single consolidated class for display and UI-interaction.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netcore-3.1
    /// https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1?view=netcore-3.1
    /// </summary>
    public class HomesForSaleReportModel : 
        INotifyPropertyChanged, IEquatable<HomesForSaleReportModel>, IEqualityComparer<HomesForSaleReportModel>
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }
   
        private string _zip;
    
        public string Zip
        {
            get { return _zip; }
            set
            {
                if (value != this._zip)
                {
                    this._zip = value;
                    NotifyPropertyChanged();
                }
            }
        }
  
        private string _ownerFirstName;
    
        public string OwnerFirstName
        {
            get { return _ownerFirstName; }
            set
            {
                if (value != this._ownerFirstName)
                {
                    this._ownerFirstName = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(OwnerFullName));
                }
            }
        }
     
        private string _ownerLastName;
    
        public string OwnerLastName
        {
            get { return _ownerLastName; }
            set
            {
                if (this._ownerLastName != value)
                {
                    this._ownerLastName = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(OwnerFullName));
                }
            }
        }
     
        private string _ownerPhone;
    
        public string OwnerPhone
        {
            get { return _ownerPhone; }
            set
            {
                if (this._ownerPhone != value)
                {
                    this._ownerPhone = value;
                    NotifyPropertyChanged();
                }
            }
        }
     
        private string _ownerEmail;
    
        public string OwnerEmail
        {
            get { return _ownerEmail; }
            set
            {
                if (this._ownerEmail != value)
                {
                    this._ownerEmail = value;
                    NotifyPropertyChanged();
                }
            }
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
                    NotifyPropertyChanged();
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
                    this._agentFirstName = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(AgentFullName));
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
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(AgentFullName));
                }
            }
        }
     
        private string _agentPhone;
    
        public string AgentPhone
        {
            get { return _agentPhone; }
            set
            {
                if (this._agentPhone != value)
                {
                    _agentPhone = value;
                    NotifyPropertyChanged();
                }
            }
        }
    
        private string _agentEmail;
     
        public string AgentEmail
        {
            get { return _agentEmail; }
            set
            {
                if (this._agentEmail != value)
                {
                    _agentEmail = value;
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }

        private DateTime _marketDate;

        public DateTime MarketDate
        {
            get { return _marketDate; }
            set
            {
                if (this._marketDate != value)
                {
                    this._marketDate = value;
                    NotifyPropertyChanged();
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
                    NotifyPropertyChanged();
                }
            }
        }

        public string AgentFullName => $"{ AgentFirstName } { AgentLastName }";

        public string OwnerFullName => $"{ OwnerFirstName } { OwnerLastName }";

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
