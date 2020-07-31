namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class Agent
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
    }
}
