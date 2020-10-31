using System.Collections.Generic;
using System.Text;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class AgentModel : PersonModel
    {
        private string _personType = "Agent";
        public decimal CommissionRate { get; set; }
        public int? RECoID { get; set; }
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
            result.Append($"{ base.ToStackedString() }\n");
            result.AppendLine($"*** { PersonType } Info ***");
            result.AppendLine($"Commission Rate: { FancyCommissionRate().ToString() }");
            result.AppendLine($"RE Company ID: { RECoID }");
            result.AppendLine($"RE Company: { RECompanyName }");
            result.AppendLine($"RE Co Phone: { FormattedPhone() }");
            
            if (HomesOnMarket.Count > 0)
            {
                foreach (var homeForSale in HomesOnMarket)
                {
                    if (homeForSale.MarketDate != null)
                    {
                        result.AppendLine("*** Home On Market ***");
                        result.AppendLine($"{ homeForSale.ToStackedString() }");
                    }
                }
            }

            if (SoldHomes.Count > 0)
            { 
                foreach (var soldHome in SoldHomes)
                {
                    if (soldHome.SoldDate != null)
                    {
                        result.AppendLine("*** Home Sale ***");
                        result.AppendLine($"{ soldHome.ToStackedString() }");
                    }
                }
            }

            return result.ToString();
        }

    }
}
