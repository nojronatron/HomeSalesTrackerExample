using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSTDataLayer
{
    public partial class Buyer
    {
        /// <summary>
        /// Inherits from Person. A Person can be an Agent, Buyer, or Owner.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ base.ToString() } { this.CreditRating }";
        }
    }
}
