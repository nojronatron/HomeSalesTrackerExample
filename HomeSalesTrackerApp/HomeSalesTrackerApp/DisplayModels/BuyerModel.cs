using HSTDataLayer;

using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class BuyerModel : PersonModel
    {
        private string _personType = "Buyer";
        public int BuyerID { get; set; }
        public string PreferredLender { get; set; }
        public int? CreditRating { get; set; }
        public RealEstateCompany Company { get; set; }
        public Decimal CommissionRate { get; set; }
        public List<SoldHomeModel> PurchasedHomes { get; set; } = new List<SoldHomeModel>();

        public BuyerModel() { }

        public override string PersonType => this._personType;
        public override string GetPersonType()
        {
            return this._personType;
        }

        public override string ToString()
        {
            return $"{ base.ToString() }{ this.CreditRating:D0}";
        }

        public override string ToStackedString()
        {
            string creditRating = "-not in database-";
            if (this.CreditRating != null && this.CreditRating > 299 && this.CreditRating < 800)
            {
                creditRating = this.CreditRating.ToString();
            }

            var result = new StringBuilder();
            result.Append($"{ base.ToStackedString() }\n" +
                $"\n*** { base.PersonType } Info ***\n" +
                $"Credit Rating: { creditRating }\n");

            if (PurchasedHomes.Count > 0)
            {
                foreach (var purchasedHome in PurchasedHomes)
                {
                    result.Append("\n*** Purchased Homes ***\n");
                    result.Append($"{ purchasedHome.ToStackedString() }\n");
                }
            }

            return result.ToString();
        }

    }

}
