using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class BuyerView : PersonView
    {
        public int BuyerID { get; set; }
        public string PreferredLender { get; set; }
        public int? CreditRating { get; set; }
        public RealEstateCompany Company { get; set; }
        public Decimal CommissionRate { get; set; }

        public override string ToString()
        {
            //  {this.PreferredLender }     { this.Company.CompanyName }{ this.CommissionRate:c}
            return $"{ base.ToString() }{ this.CreditRating:D0}";
        }
    }

}
