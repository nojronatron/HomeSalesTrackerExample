using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Report_Models
{
    public class HomesForSaleViewModel
    {
        public List<HomesForSaleReportModel> HomesForSale { get; set; }

        public HomesForSaleViewModel()
        {
            LoadHomesForSale();

        }

        public void LoadHomesForSale()
        {
            var query = (from homesale in MainWindow.homeSalesCollection
                         where homesale.SoldDate == null
                         join home in MainWindow.homesCollection on homesale.HomeID equals home.HomeID
                         join ownerPerson in MainWindow.peopleCollection on home.OwnerID equals ownerPerson.PersonID
                         join agentPerson in MainWindow.peopleCollection on homesale.AgentID equals agentPerson.PersonID
                         join reco in MainWindow.reCosCollection on agentPerson.Agent.CompanyID equals reco.CompanyID
                         select new HomesForSaleReportModel
                         {
                             HomeID = home.HomeID,
                             Address = home.Address,
                             City = home.City,
                             State = home.State,
                             Zip = home.Zip,
                             OwnerFirstName = ownerPerson.FirstName,
                             OwnerLastName = ownerPerson.LastName,
                             OwnerPhone = ownerPerson.Phone,
                             OwnerEMail = ownerPerson.Email ?? "-none-",
                             PreferredLender = home.Owner.PreferredLender ?? "-none-",
                             AgentFirstName = agentPerson.FirstName,
                             AgentLastName = agentPerson.LastName,
                             AgentPhone = agentPerson.Phone,
                             AgentEMail = agentPerson.Email ?? "-none-",
                             CompanyName = reco.CompanyName,
                             MarketDate = homesale.MarketDate,
                             SaleAmount = homesale.SaleAmount
                         });

            var homesForSale = query.OrderBy(m => m.MarketDate).ThenBy(z => z.Zip).ThenBy(a => a.Address);

            HomesForSale = homesForSale.ToList();

        }
    }
}
