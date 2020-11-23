using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class HomeDisplayDetailModel : HomeDisplayModel
    {
        public string PersonType => "Owner";
        public DateTime? MarketDate { get; set; }
        public OwnerModel owner { get; set; }

        public override string ToStackedString()
        {
            StringBuilder result = new StringBuilder();

            var marketDate = "Home not for sale";

            if (MarketDate != null)
            {
                marketDate = $"Market Date: {MarketDate.ToString() }";
            }

            result.AppendLine($"HomeID: { this.HomeID }");
            result.AppendLine($"Address: { this.Address }");
            result.AppendLine($"City: { this.City }");
            result.AppendLine($"State: { this.State }");
            result.AppendLine($"Zip Code: { this.Zip.Substring(0, 5) }-{ this.Zip.Substring(5, 4) }");
            result.AppendLine( marketDate );
            result.AppendLine($"*** { PersonType } Info ***");
            result.AppendLine($"Name: { owner.FullName }");
            result.AppendLine($"Phone: { owner.PhoneNumber }");
            result.AppendLine($"EMail: { owner.Email }");
            result.AppendLine($"Preferred Lender: { owner.PreferredLender }");

            return result.ToString();
        }

    }
}
