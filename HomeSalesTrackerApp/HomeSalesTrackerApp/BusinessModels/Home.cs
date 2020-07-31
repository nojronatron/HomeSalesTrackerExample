namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;

    public partial class Home
    {
        public Home()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        public int HomeID { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public int OwnerID { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual ICollection<HomeSale> HomeSales { get; set; }
    }
}
