using System;
using System.Collections.Generic;

namespace HSTDataLayer
{
    public partial class Home : IEquatable<Home>
    {
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
