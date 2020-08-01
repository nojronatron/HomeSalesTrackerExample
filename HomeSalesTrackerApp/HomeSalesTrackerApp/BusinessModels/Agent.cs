namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class Agent : Person
    {
        public Agent()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        public int AgentID { get; set; }

        public int? CompanyID { get; set; }

        public decimal CommissionPercent { get; set; }

        public virtual Person Person { get; set; }

        public virtual RealEstateCompany RealEstateCompany { get; set; }

        public virtual ICollection<HomeSale> HomeSales { get; set; }

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
