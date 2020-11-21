using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class SoldHomeModel : HomeDisplayModel
    {
        public int HomeSaleID { get; set; }
        public string BuyerFirstLastName { get; set; }
        public string AgentFirstLastName { get; set; }
        public string RealEstateCompanyName { get; set; }
        public string RealEstateCompanyPhone { get; set; }
        public RealEstateCoModel RECo { get; set; } = new RealEstateCoModel();
        public decimal SaleAmount { get; set; }
        public DateTime? SoldDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public SoldHomeModel() { }

        private string FormatZip()
        {
            if (base.Zip.Length > 0)
            {
                return $"{ base.Zip.Substring(0, 5) }-{ base.Zip.Substring(5, 4) }";
            }
            return $"! { base.Zip } !";
        }

        public override string ToString()
        {
            return $"{ base.ToString()} { BuyerFirstLastName } { AgentFirstLastName }" +
                $" { RealEstateCompanyName } { RealEstateCompanyPhone } { SaleAmount:C0} { SoldDate:D}";
        }

        public override string ToStackedString()
        {
            return $"HomeID: { base.HomeID }\n" +
                $"Address: { base.Address }\n" +
                $"City: { base.City }\n" +
                $"State: { base.State }\n" +
                $"Zip: { FormatZip() }\n" +
                $"Sold Date: { this.SoldDate:D}\n" +
                $"Sale Amount: { this.SaleAmount:C0}";
        }

    }
}
