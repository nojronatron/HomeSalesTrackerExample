using System.Text;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class OwnerModel : PersonModel
    {
        private string _personType = "Owner";
        public int OwnerID { get; set; }
        public string PreferredLender { get; set; } = "Not identified.";
        public List<HomeDisplayModel> OwnedHomes { get; set; } = new List<HomeDisplayModel>();

        public OwnerModel() { }

        public override string PersonType => this._personType;
        public override string GetPersonType()
        {
            return this._personType;
        }

        public override string ToStackedString()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"{ base.ToStackedString() }\n");
            result.AppendLine($"*** { PersonType } Info ***");
            result.AppendLine($"Preferred Lender: { this.PreferredLender }");

            foreach (HomeDisplayModel ownedHome in OwnedHomes)
            {
                result.AppendLine("*** Owned Home ***");
                result.AppendLine($"{ ownedHome.ToStackedString() }");
            }

            return result.ToString();
        }

    }
}
