using System;
using System.Collections.Generic;

namespace HSTDataLayer
{
    public partial class Buyer: IEquatable<Buyer>
    {
        public override bool Equals(object obj)
        {
            return obj is Buyer buyer &&
                   BuyerID == buyer.BuyerID &&
                   CreditRating == buyer.CreditRating;
        }

        public override int GetHashCode()
        {
            var hashCode = 486394590;
            hashCode = hashCode * -1521134295 + BuyerID.GetHashCode();
            hashCode = hashCode * -1521134295 + CreditRating.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Inherits from Person. A Person can be an Agent, Buyer, or Owner.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ base.ToString() } { this.CreditRating }";
        }

        bool IEquatable<Buyer>.Equals(Buyer other)
        {
            return (this.CreditRating == other.CreditRating && this.BuyerID == other.BuyerID);
        }

        public static bool operator ==(Buyer left, Buyer right)
        {
            return EqualityComparer<Buyer>.Default.Equals(left, right);
        }

        public static bool operator !=(Buyer left, Buyer right)
        {
            return !(left == right);
        }

    }
}
