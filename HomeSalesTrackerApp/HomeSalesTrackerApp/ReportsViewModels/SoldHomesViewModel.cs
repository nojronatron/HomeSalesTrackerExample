using HomeSalesTrackerApp.Factory;
using HomeSalesTrackerApp.Report_Models;

using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class SoldHomesViewModel
    {
        public List<SoldHomesReportModel> SoldHomes { get; set; }
        public SoldHomesViewModel()
        {
            LoadSoldHomes();
        }

        private void LoadSoldHomes()
        {
            var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
            var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
            var fRECOsCollection = CollectionFactory.GetRECosCollectionObject();
            var fHomesCollection = CollectionFactory.GetHomesCollectionObject();

            var query = (from hfs in fHomeSalesCollection
                         where hfs.SoldDate != null
                         join h in fHomesCollection on hfs.HomeID equals h.HomeID
                         join a in fPeopleCollection on hfs.AgentID equals a.PersonID
                         join b in fPeopleCollection on hfs.BuyerID equals b.PersonID
                         join re in fRECOsCollection on hfs.CompanyID equals re.CompanyID
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
                         });

            SoldHomes = query.OrderBy(re => re.CompanyName).ThenBy(a => a.AgentLastName)
                             .ThenBy(a => a.AgentFirstName).ThenBy(hfs => hfs.SoldDate)
                             .ToList();

        }
    }
}
