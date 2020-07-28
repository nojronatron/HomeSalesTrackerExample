namespace HSTDataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HomeSale
    {
        [Key]
        public int SaleID { get; set; }

        public int HomeID { get; set; }

        public DateTime? SoldDate { get; set; }

        public int AgentID { get; set; }

        public decimal SaleAmount { get; set; }

        public int? BuyerID { get; set; }

        public DateTime MarketDate { get; set; }

        public int CompanyID { get; set; }

        public virtual Agent Agent { get; set; }

        public virtual Buyer Buyer { get; set; }

        public virtual Home Home { get; set; }

        public virtual RealEstateCompany RealEstateCompany { get; set; }
    }
}
