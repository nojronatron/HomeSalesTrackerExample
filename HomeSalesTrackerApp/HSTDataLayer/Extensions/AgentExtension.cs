using System;
using System.Collections.Generic;

namespace HSTDataLayer
{
    public partial class Agent : IEquatable<Agent>
    {
        public override bool Equals(object obj)
        {
            return obj is Agent agent &&
                   AgentID == agent.AgentID &&
                   CompanyID == agent.CompanyID &&
                   CommissionPercent == agent.CommissionPercent;
        }

        public override int GetHashCode()
        {
            var hashCode = 129009701;
            hashCode = hashCode * -1521134295 + AgentID.GetHashCode();
            hashCode = hashCode * -1521134295 + CompanyID.GetHashCode();
            hashCode = hashCode * -1521134295 + CommissionPercent.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Agent left, Agent right)
        {
            return EqualityComparer<Agent>.Default.Equals(left, right);
        }

        public static bool operator !=(Agent left, Agent right)
        {
            return !(left == right);
        }

        bool IEquatable<Agent>.Equals(Agent other)
        {
            bool result = false;
            if (other != null)
            {
                if (this.AgentID == other.AgentID &&
                    this.CompanyID == other.CompanyID &&
                    this.CommissionPercent == other.CommissionPercent)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Inherits from Person. A Person can be an Agent, Buyer, and Owner.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string recoID = this.CompanyID == null ? "Agent no longer active." : this.CompanyID.ToString();
            return $"{ base.ToString() } { this.AgentID } { recoID } { this.CommissionPercent }";
        }

    }
}
