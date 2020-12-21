using HomeSalesTrackerApp.Factory;
using HomeSalesTrackerApp.Report_Models;

using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class BuyersReportViewModel
    {
        public List<BuyersReportModel> BuyersList { get; set; }

        public BuyersReportViewModel()
        {
            Load();
        }

        private void Load()
        {
            var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
            var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
            var fRECOsCollection = CollectionFactory.GetRECosCollectionObject();
            var fHomesCollection = CollectionFactory.GetHomesCollectionObject();

            var query = (from home in fHomesCollection
                         join homesale in fHomeSalesCollection on home.HomeID equals homesale.HomeID
                         join buyer in fPeopleCollection on homesale.BuyerID equals buyer.PersonID
                         where homesale.Buyer != null
                         select new BuyersReportModel
                         {
                             Address = home.Address,
                             City = home.City,
                             State = home.State,
                             Zip = home.Zip,
                             SaleDate = homesale.SoldDate,
                             SaleAmount = homesale.SaleAmount,
                             BuyerID = homesale.BuyerID,
                             FirstName = buyer.FirstName,
                             LastName = buyer.LastName,
                             Phone = buyer.Phone,
                             EMail = buyer.Email ?? string.Empty,
                             CreditRating = buyer.Buyer.CreditRating
                         });

            BuyersList = query.OrderBy(l => l.LastName).ThenBy(f => f.FirstName).ThenBy(sd => sd.SaleDate).ThenBy(sa => sa.SaleAmount).ToList();
        }
    }
}
