using System;
using System.Collections.Generic;

namespace HSTDataLayer
{
    public partial class RealEstateCompany : IEquatable<RealEstateCompany>, IComparable<RealEstateCompany>
    {
        public override string ToString()
        {
            string phoneFormatted = $"{ this.Phone.Substring(0, 3) }-{ this.Phone.Substring(3, 3) }-{ this.Phone.Substring(6, 4) }";
            return $"{ this.CompanyName } { phoneFormatted }";
        }

        int IComparable<RealEstateCompany>.CompareTo(RealEstateCompany other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.CompanyName.CompareTo(other.CompanyName);
        }

        bool IEquatable<RealEstateCompany>.Equals(RealEstateCompany other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.CompanyName == other.CompanyName &&
                this.Phone == other.Phone)
            {
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is RealEstateCompany company &&
                   CompanyName == company.CompanyName &&
                   Phone == company.Phone;
        }

        public override int GetHashCode()
        {
            int hashCode = 1109774783;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CompanyName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Phone);
            return hashCode;
        }

        public static bool operator ==(RealEstateCompany left, RealEstateCompany right)
        {
            return EqualityComparer<RealEstateCompany>.Default.Equals(left, right);
        }

        public static bool operator !=(RealEstateCompany left, RealEstateCompany right)
        {
            return !(left == right);
        }
    }
}
