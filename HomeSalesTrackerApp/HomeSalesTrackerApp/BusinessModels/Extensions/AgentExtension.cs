using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp
{
    public partial class Agent : Person, IEquatable<Agent>, IComparable<Agent>
    {
        public override bool Equals(object obj)
        {
            return obj is Agent agent &&
                   AgentID == agent.AgentID &&
                   CommissionPercent == agent.CommissionPercent;
        }

        public override int GetHashCode()
        {
            int hashCode = 726386804;
            hashCode = hashCode * -1521134295 + AgentID.GetHashCode();
            hashCode = hashCode * -1521134295 + CommissionPercent.GetHashCode();
            return hashCode;
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

        int IComparable<Agent>.CompareTo(Agent other)
        {
            int result = 0;
            if (this.AgentID < other.AgentID)
            {
                result = -1;
            }
            if (this.AgentID > other.AgentID)
            {
                result = 1;
            }
            return result;
        }

        bool IEquatable<Agent>.Equals(Agent other)
        {
            bool result = false;
            if (other != null)
            {
                if (this.AgentID == other.AgentID)
                {
                    result = true;
                }
            }
            return result;
        }

        public static bool operator ==(Agent left, Agent right)
        {
            return EqualityComparer<Agent>.Default.Equals(left, right);
        }

        public static bool operator !=(Agent left, Agent right)
        {
            return !(left == right);
        }
    }
}
