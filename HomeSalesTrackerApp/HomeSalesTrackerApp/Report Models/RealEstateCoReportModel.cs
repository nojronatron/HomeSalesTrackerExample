using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeSalesTrackerApp.Report_Models
{
    public class RealEstateCoReportModel : 
        IEquatable<RealEstateCoReportModel>, IEqualityComparer<RealEstateCoReportModel>, INotifyPropertyChanged
    {
        /*  Display data of ID, RECo Name, Ttl Num homes sold, Ttl Sales, 
         *  Ttl Homes currently For Sale, Ttl amount for sale.
        */
        private int _companyID;

        public int CompanyID
        {
            get { return _companyID; }
            set
            {
                if (this._companyID != value)
                {
                    _companyID = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _rECoName;

        public string RECoName
        {
            get { return _rECoName; }
            set
            {
                if (this._rECoName != value)
                {
                    _rECoName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _totalSoldHomesCount;

        public int TotalSoldHomesCount
        {
            get { return _totalSoldHomesCount; }
            set
            {
                if (this._totalSoldHomesCount != value)
                {
                    _totalSoldHomesCount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private decimal _totalSales;

        public decimal TotalSales
        {
            get { return _totalSales; }
            set
            {
                if (this._totalSales != value)
                {
                    _totalSales = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _totalHomesForSale;

        public int TotalHomesForSale
        {
            get { return _totalHomesForSale; }
            set
            {
                if (this._totalHomesForSale != value)
                {
                    _totalHomesForSale = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private decimal _totalAmountForSale;

        public decimal TotalAmountForSale
        {
            get { return _totalAmountForSale; }
            set
            {
                if (this._totalAmountForSale != value)
                {
                    _totalAmountForSale = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Equals(RealEstateCoReportModel other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return CompanyID.Equals(other.CompanyID);
        }

        public bool Equals(RealEstateCoReportModel x, RealEstateCoReportModel y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.CompanyID == y.CompanyID;
        }

        public int GetHashCode(RealEstateCoReportModel obj)
        {
            return obj.CompanyID.GetHashCode();
        }

        public override int GetHashCode()
        {
            return 2112202448 + _companyID.GetHashCode();
        }

        public static bool operator ==(RealEstateCoReportModel left, RealEstateCoReportModel right)
        {
            return EqualityComparer<RealEstateCoReportModel>.Default.Equals(left, right);
        }

        public static bool operator !=(RealEstateCoReportModel left, RealEstateCoReportModel right)
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
