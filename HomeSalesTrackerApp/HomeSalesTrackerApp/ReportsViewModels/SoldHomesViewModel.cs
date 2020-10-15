using HomeSalesTrackerApp.Report_Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class SoldHomesViewModel
    {
        public ObservableCollection<SoldHomesReportModel> SoldHomes { get; set; }
        public SoldHomesViewModel()
        {
            LoadSoldHomes();
        }

        private void LoadSoldHomes()
        {
            ObservableCollection<SoldHomesReportModel> soldHomes = new ObservableCollection<SoldHomesReportModel>();

            var vHfsHomesSold = (from hfs in MainWindow.homeSalesCollection
                                 where hfs.SoldDate != null
                                 select hfs).ToList();

            var vHomes = (from h in MainWindow.homesCollection
                          from hfs in vHfsHomesSold
                          where h.HomeID == hfs.HomeID
                          select h).ToList();

            var vBuyers = (from p in MainWindow.peopleCollection
                           from hfs in vHfsHomesSold
                           where p.PersonID == hfs.BuyerID
                           select p).ToList();

            var vAgents = (from p in MainWindow.peopleCollection
                           from hfs in vHfsHomesSold
                           where p.PersonID == hfs.AgentID
                           select p).ToList();

            var vRECos = (from re in MainWindow.reCosCollection
                          from hfs in vHfsHomesSold
                          where re.CompanyID == hfs.CompanyID
                          select re).ToList();

            var vSoldHomes = (from hfs in vHfsHomesSold
                              from h in vHomes
                              from b in vBuyers
                              from a in vAgents
                              from re in vRECos
                              where h.HomeID == hfs.HomeID &&
                              b.Buyer.BuyerID == hfs.BuyerID &&
                              a.Agent.AgentID == hfs.AgentID &&
                              re.CompanyID == hfs.CompanyID
                              select new SoldHomesReportModel
                              {
                                  HomeID = hfs.HomeID,
                                  Address = h.Address,
                                  City = h.City,
                                  State = h.State,
                                  Zip = h.Zip,
                                  BuyerFirstName = b.FirstName,
                                  BuyerLastName = b.LastName,
                                  AgentFirstName = a.FirstName,
                                  AgentLastName = a.LastName,
                                  CompanyName = re.CompanyName,
                                  SaleAmount = hfs.SaleAmount,
                                  SoldDate = hfs.SoldDate
                              }).ToList();

            vSoldHomes = vSoldHomes.Distinct().ToList();

            foreach (SoldHomesReportModel home in vSoldHomes)
            {
                soldHomes.Add(home);
            }

            SoldHomes = soldHomes;
        }
    }
}
