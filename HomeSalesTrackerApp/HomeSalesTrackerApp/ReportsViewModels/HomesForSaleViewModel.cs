using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.Report_Models
{
    public class HomesForSaleViewModel
    {
        public ObservableCollection<HomesForSaleReportModel> HomesForSale { get; set; }

        public HomesForSaleViewModel()
        {
            LoadHomesForSale();

        }

        public void LoadHomesForSale()
        {
            ObservableCollection<HomesForSaleReportModel> homesForSale = new ObservableCollection<HomesForSaleReportModel>();

            var vHfsNotSold = MainWindow.homeSalesCollection.Where(hfs => hfs.SoldDate == null).ToList();
            
            var vHomesNotSold = (from h in MainWindow.homesCollection
                                 join hfs in vHfsNotSold on h.HomeID equals hfs.HomeID
                                 select h).ToList();
            
            var vOwners = (from p in MainWindow.peopleCollection
                           join h in vHomesNotSold on p.PersonID equals h.OwnerID
                           select p).ToList();

            var vAgents = (from p in MainWindow.peopleCollection
                           join hfs in vHfsNotSold on p.PersonID equals hfs.AgentID
                           select p).ToList();

            var vRECos = (from reco in MainWindow.reCosCollection
                          join hfs in vHfsNotSold on reco.CompanyID equals hfs.CompanyID
                          select reco).ToList();

            var vHomesForSale = (from hfs in vHfsNotSold
                                 from h in vHomesNotSold
                                 from o in vOwners
                                 from a in vAgents
                                 from re in vRECos
                                 where (
                                 h.HomeID == hfs.HomeID &&
                                 o.PersonID == h.OwnerID &&
                                 a.PersonID == hfs.AgentID &&
                                 re.CompanyID == hfs.CompanyID
                                 )
                                 select new HomesForSaleReportModel
                                 {
                                     HomeID = hfs.HomeID,
                                     Address = h.Address,
                                     City = h.City,
                                     State = h.State,
                                     Zip = h.Zip,
                                     OwnerFullName = $"{ o.FirstName } { o.LastName }",
                                     OwnerPhone = o.Phone,
                                     OwnerEMail = o.Email ?? "-none-",
                                     PreferredLender = o.Owner.PreferredLender ?? "-none-",
                                     AgentFullName = $"{ a.FirstName } { a.LastName }",
                                     AgentPhone = a.Phone,
                                     AgentEMail = a.Email ?? "-none-",
                                     CompanyName = re.CompanyName,
                                     MarketDate = hfs.MarketDate,
                                     SaleAmount = hfs.SaleAmount
                                 }).ToList();

            vHomesForSale = vHomesForSale.Distinct().ToList();

            foreach (HomesForSaleReportModel hfsrm in vHomesForSale)
            {
                homesForSale.Add(hfsrm);
            }

            HomesForSale = homesForSale;
        }
    }
}
