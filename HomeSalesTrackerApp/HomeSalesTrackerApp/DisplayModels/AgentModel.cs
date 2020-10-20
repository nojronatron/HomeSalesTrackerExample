using System.Collections.Generic;
using System.Text;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class AgentModel : PersonModel
    {
        private string _personType = "Agent";
        public decimal CommissionRate { get; set; }
        public decimal RECoID { get; set; }
        public string RECompanyName { get; set; } = "Agent no longer active.";
        public string RECoPhone { get; set; }

        public List<SoldHomeModel> SoldHomes { get; set; }
        public List<HomeForSaleModel> HomesOnMarket { get; set; }

        public AgentModel() { }

        private string FancyCommissionRate()
        {
            decimal bigRate = CommissionRate * 100m;
            return $"{ bigRate}";
        }

        private string FormattedPhone()
        {
            if (RECoPhone.Length > 0)
            {
                return $"({ RECoPhone.Substring(0, 3) }) { RECoPhone.Substring(3, 3) }-{ RECoPhone.Substring(6, 4) }";
            }
            return "-missing-";
        }
        
        public override string PersonType => this._personType;
        public override string GetPersonType()
        {
            return this._personType;
        }

        public override string ToStackedString()
        {
            var result = new StringBuilder();
            result.Append($"{ base.ToStackedString() }\n" +
                $"\n*** { base.PersonType } Info ***\n" +
                $"Commission Rate: { FancyCommissionRate().ToString() }\n" +
                $"RE Company ID: { RECoID }\n" +
                $"RE Company: { RECompanyName }\n" +
                $"RE Co Phone: { FormattedPhone() }\n");
            
            if (HomesOnMarket.Count > 0)
            {
                foreach (var homeForSale in HomesOnMarket)
                {
                    if (homeForSale.MarketDate != null)
                    {
                        result.Append("\n*** Home On Market ***\n");
                        result.Append($"{ homeForSale.ToStackedString() }\n");
                    }
                }
            }

            if (SoldHomes.Count > 0)
            { 
                foreach (var soldHome in SoldHomes)
                {
                    if (soldHome.SoldDate != null)
                    {
                        result.Append("\n*** Home Sale ***\n");
                        result.Append($"{ soldHome.ToStackedString() }\n");
                    }
                }
            }

            return result.ToString();

        }

    }
}
