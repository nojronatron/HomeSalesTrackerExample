using HomeSalesTrackerApp.Report_Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class BuyersReportViewModel
    {
        public ObservableCollection<BuyersReportModel> BuyersList { get; set; }

        public BuyersReportViewModel()
        {
            Load();
        }

        private void Load()
        {
            var buyersList = new ObservableCollection<BuyersReportModel>();

            var soldHomes = (from hs in MainWindow.homeSalesCollection
                             where hs.Buyer != null
                             select hs).Distinct().ToList();

            var buyerPeople = (from hs in soldHomes
                               from p in MainWindow.peopleCollection
                               where p.PersonID == hs.BuyerID
                               select p).ToList();

            var buyers = (from hs in soldHomes
                          select hs.Buyer).ToList();

            var homes = (from hs in soldHomes
                         from h in MainWindow.homesCollection
                         where h.HomeID == hs.HomeID
                         select h).ToList();

            BuyersReportModel buyer = null;
            foreach (var soldHome in soldHomes)
            {
                buyer = new BuyersReportModel();

                buyer.BuyerID = int.Parse(soldHome.BuyerID.ToString()); //  TODO: find out if this is good or ganky
                buyer.FirstName = buyerPeople.Where(p => p.PersonID == soldHome.BuyerID).FirstOrDefault().FirstName;
                buyer.LastName = buyerPeople.Where(p => p.PersonID == soldHome.BuyerID).FirstOrDefault().LastName;
                buyer.Phone = buyerPeople.Where(p => p.PersonID == soldHome.BuyerID).FirstOrDefault().Phone;
                buyer.EMail = buyerPeople.Where(p => p.PersonID == soldHome.BuyerID).FirstOrDefault().Email;
                buyer.CreditRating = buyers.Where(b => b.BuyerID == soldHome.BuyerID).FirstOrDefault().CreditRating;

                buyer.SaleDate = (DateTime)soldHome.SoldDate;
                buyer.SaleAmount = (Decimal)soldHome.SaleAmount;
                buyer.Address = homes.Where(h => h.HomeID == soldHome.HomeID).FirstOrDefault().Address;
                buyer.City = homes.Where(h => h.HomeID == soldHome.HomeID).FirstOrDefault().City;
                buyer.State = homes.Where(h => h.HomeID == soldHome.HomeID).FirstOrDefault().State;
                buyer.Zip = homes.Where(h => h.HomeID == soldHome.HomeID).FirstOrDefault().Zip;

                buyersList.Add(buyer);
            }

            BuyersList = buyersList;
        }
    }
}
