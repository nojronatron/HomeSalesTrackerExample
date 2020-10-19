using System;

namespace HomeSalesTrackerApp.DisplayModels
{
    public class SoldHomeView : HomeDisplayModel
    {
        public string BuyerFirstLastName { get; set; }
        public string AgentFirstLastName { get; set; }

        ////  TODO: Fix this class and Menu > Search > SoldHomes so that Real Estate Co details are not included (they are not needed)
        public string RealEstateCompanyName { get; set; }
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
        //public int RecoID { get; set; }
        //public string RecoName { get; set; }
        //public string RecoPhone { get; set; }
        //private string RecoPhoneFormatted
        //{
        //    get
        //    {
        //        return $"({ RecoPhone.Substring(0,3) }) { RecoPhone.Substring(3,3) }-{ RecoPhone.Substring(6,4) }";
        //    }
        //}

        //public override string ToString()
        //{
        //    return $"{ base.ToString()} { BuyerFirstLastName } { AgentFirstLastName }" +
        //        $" { RecoID } { RecoName } { RecoPhoneFormatted }" +
        //        $" { SaleAmount:C2} { SoldDate:MM/dd/yyyy}";
        //}

        //public override string ToStackedString()
        //{
        //    return $"{ base.ToStackedString() }\n" +
        //        $"Real Estate Co ID: { RecoID }\n" + 
        //        $"Real Estate Co Name: { RecoName }\n" +
        //        $"Real Estate Co Phone: { RecoPhoneFormatted }";
        //}


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
