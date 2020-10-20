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
            result.Append($"Preferred Lender: { this.PreferredLender }\n");
            result.Append("\n*** Owned Homes ***\n");

            foreach (HomeDisplayModel ownedHome in OwnedHomes)
            {
                result.Append($"{ ownedHome.ToStackedString() }");
            }
            return result.ToString();
        }

    }
}
