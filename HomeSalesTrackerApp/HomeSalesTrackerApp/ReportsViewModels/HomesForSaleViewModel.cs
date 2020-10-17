using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var query = (from homesale in MainWindow.homeSalesCollection where homesale.SoldDate == null
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

            //var vHfsNotSold = MainWindow.homeSalesCollection.Where(hfs => hfs.SoldDate == null).ToList();
            
            //var vHomesNotSold = (from h in MainWindow.homesCollection
            //                     join hfs in vHfsNotSold on h.HomeID equals hfs.HomeID
            //                     select h).ToList();
            
            //var vOwners = (from p in MainWindow.peopleCollection
            //               join h in vHomesNotSold on p.PersonID equals h.OwnerID
            //               select p).ToList();

            //var vAgents = (from p in MainWindow.peopleCollection
            //               join hfs in vHfsNotSold on p.PersonID equals hfs.AgentID
            //               select p).ToList();

            //var vRECos = (from reco in MainWindow.reCosCollection
            //              join hfs in vHfsNotSold on reco.CompanyID equals hfs.CompanyID
            //              select reco).ToList();

            //var vHomesForSale = (from hfs in vHfsNotSold
            //                     from h in vHomesNotSold
            //                     from o in vOwners
            //                     from a in vAgents
            //                     from re in vRECos
            //                     where (
            //                     h.HomeID == hfs.HomeID &&
            //                     o.PersonID == h.OwnerID &&
            //                     a.PersonID == hfs.AgentID &&
            //                     re.CompanyID == hfs.CompanyID
            //                     )
            //                     select new HomesForSaleReportModel
            //                     {
            //                         HomeID = hfs.HomeID,
            //                         Address = h.Address,
            //                         City = h.City,
            //                         State = h.State,
            //                         Zip = h.Zip,
            //                         OwnerFullName = $"{ o.FirstName } { o.LastName }",
            //                         OwnerPhone = o.Phone,
            //                         OwnerEMail = o.Email ?? "-none-",
            //                         PreferredLender = o.Owner.PreferredLender ?? "-none-",
            //                         AgentFullName = $"{ a.FirstName } { a.LastName }",
            //                         AgentPhone = a.Phone,
            //                         AgentEMail = a.Email ?? "-none-",
            //                         CompanyName = re.CompanyName,
            //                         MarketDate = hfs.MarketDate,
            //                         SaleAmount = hfs.SaleAmount
            //                     }).ToList();

            //vHomesForSale = vHomesForSale.Distinct().ToList();

            //foreach (HomesForSaleReportModel hfsrm in vHomesForSale)
            //{
            //    homesForSale.Add(hfsrm);
            //}
        }
    }
}
