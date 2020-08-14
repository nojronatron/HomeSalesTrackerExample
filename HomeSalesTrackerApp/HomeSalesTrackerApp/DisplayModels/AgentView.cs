using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class AgentView : PersonView
    {
        public decimal CommissionRate { get; set; }
        public RealEstateCompany RECo { get; set; }
        public HomeSale homeSale { get; set; }
        public override string PersonType => this.GetType().Name;
        public override string GetPersonType()
        {
            return this.GetPersonType();
        }

    }
}
