namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class Home : IEquatable<Home>, IComparable<Home>
    {
        public Home()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        public int HomeID { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public int OwnerID { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual ICollection<HomeSale> HomeSales { get; set; }

        /// <summary>
        /// Override ToString() to control a basic string-output of a Home instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string zipcode = this.Zip.Substring(0, 5);
            string plusFour = this.Zip.Substring(5, 4);
            return $"{ this.Address }\n{ this.City }, { this.State }\t{ zipcode }-{ plusFour }";
        }

        /// <summary>
        /// Required IComparable method implementation. Reference for multi-field sorting: https://stackoverflow.com/questions/4501501/custom-sorting-icomparer-on-three-fields
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable<Home>.CompareTo(Home other)
        {
            if (other == null) return 1;
            //  sort() items in a list by LastName then FirstName
            int result = this.Zip.CompareTo(other.Zip);
            if (result == 0)
            {
                result = this.Address.CompareTo(other.Address);
            }
            return result;
        }

        /// <summary>
        /// Required IEquatable method implementation. Home uniqueness is defined only by Zip and Address.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool IEquatable<Home>.Equals(Home other)
        {
            if (this.Zip == other.Zip &&
                this.Address == other.Address)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Required Equals() override when implementing IEquitable T (IDE generated code). Uniqueness is defined only by Zip and Address.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Home home &&
                   Zip == home.Zip &&
                   Address == home.Address;
        }

        /// <summary>
        /// Recommended GetHashCode() override when overriding Equals() (IDE generated code]
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = -339536537;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Address);
            return hashCode;
        }

        /// <summary>
        /// Recommended Equal Operator override. Supports IEquatable implementation and Equals() override methods.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Home left, Home right)
        {
            return EqualityComparer<Home>.Default.Equals(left, right);
        }

        /// <summary>
        /// Recommended Not Equal Operator override. Supports IEquatable implementation and Equals() override methods.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Home left, Home right)
        {
            return !(left == right);
        }
    }
}
