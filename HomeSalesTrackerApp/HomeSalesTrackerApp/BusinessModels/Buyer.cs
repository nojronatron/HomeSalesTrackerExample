namespace HomeSalesTrackerApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Buyer : Person
    {
        public Buyer()
        {
            HomeSales = new HashSet<HomeSale>();
        }

        public int BuyerID { get; set; }

        public int? CreditRating { get; set; }

        public virtual Person Person { get; set; }

        public virtual ICollection<HomeSale> HomeSales { get; set; }

    }
}
