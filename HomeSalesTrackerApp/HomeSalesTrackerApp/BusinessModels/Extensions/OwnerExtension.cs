using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Inherits from Person. A Person can be an Agent, Buyer, and a Owner.
    /// </summary>
    public partial class Owner : Person
    {
        public override string ToString()
        {
            return $"{ base.ToString() } { this.PreferredLender }";
        }
    }
}
