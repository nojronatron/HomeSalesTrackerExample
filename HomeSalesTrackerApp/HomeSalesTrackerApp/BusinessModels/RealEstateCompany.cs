namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class RealEstateCompany
    {
        public RealEstateCompany()
        {
            Agents = new HashSet<Agent>();
            HomeSales = new HashSet<HomeSale>();
        }

        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Agent> Agents { get; set; }

        public virtual ICollection<HomeSale> HomeSales { get; set; }
    }
}
