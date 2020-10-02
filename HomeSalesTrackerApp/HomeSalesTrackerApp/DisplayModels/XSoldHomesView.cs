using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class XSoldHomesView : HomeView
    {
        public string BuyerFirstLastName { get; set; }
        public string AgentFirstLastName { get; set; }

        public string RealEstateCompanyName { get; set; }
        public decimal SaleAmount { get; set; }
        public DateTime? SoldDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{ base.ToString()} { BuyerFirstLastName } { AgentFirstLastName } { RealEstateCompanyName } { SaleAmount:C2} { SoldDate:MM/dd/yyyy}";
        }

    }
}
