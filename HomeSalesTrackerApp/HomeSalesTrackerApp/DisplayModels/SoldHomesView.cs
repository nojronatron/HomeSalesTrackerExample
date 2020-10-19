using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class SoldHomesView : HomeDisplayModel
    {
        public string BuyerFirstLastName { get; set; }
        public string AgentFirstLastName { get; set; }

        //  TODO: Determine if removing this prop causes bugs
        //  public string RealEstateCompanyName { get; set; }
        private RealEstateCompanyView _reco = new RealEstateCompanyView();
        public RealEstateCompanyView RECo
        {
            get
            {
                return _reco;
            }
            set
            {
                _reco = value;
            }
        }
        public decimal SaleAmount { get; set; }
        public DateTime? SoldDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{ base.ToString()} { BuyerFirstLastName } { AgentFirstLastName }" +
                $" { RECo.CompanyName } { SaleAmount:C2} { SoldDate:MM/dd/yyyy}";
        }

        public override string ToStackedString()
        {
            string phone = "";
            if (RECo.Phone == null || RECo.Phone.Length < 10)
            {
                phone = "No phone number supplied.";
            }
            else
            {
                phone = $"({ RECo.Phone.Substring(0, 3) })" +
                    $" { RECo.Phone.Substring(3, 3) }-{ RECo.Phone.Substring(6, 4) }";
            }

            return $"{ base.ToStackedString() }\n" +
                $"Real Estate Company: { RECo.CompanyName }\n" +
                $"Real Estate Co Phone: { phone }";
        }

    }
}
