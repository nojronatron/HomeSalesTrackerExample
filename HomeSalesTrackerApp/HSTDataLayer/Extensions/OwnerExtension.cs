using System;
using System.Collections.Generic;

namespace HSTDataLayer
{
    /// <summary>
    /// Inherits from Person. A Person can be an Agent, Buyer, and a Owner.
    /// </summary>
    public partial class Owner : IEquatable<Owner>
    {
        public override bool Equals(object obj)
        {
            return obj is Owner owner &&
                   OwnerID == owner.OwnerID &&
                   PreferredLender == owner.PreferredLender;
        }

        public override int GetHashCode()
        {
            var hashCode = 1023787409;
            hashCode = hashCode * -1521134295 + OwnerID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PreferredLender);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{ base.ToString() } { this.PreferredLender }";
        }

        bool IEquatable<Owner>.Equals(Owner other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.OwnerID == other.OwnerID &&
                this.PreferredLender == other.PreferredLender)
            {
                return true;
            }

            return false;
        }
    }
}
