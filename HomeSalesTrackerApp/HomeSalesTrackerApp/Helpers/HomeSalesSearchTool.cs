using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    public static class HomeSalesSearchTool
    {

        public static List<HomeSale> GetMatchingHomesaleItems(List<string> searchTerms)
        {
            var searchHomesalesResults = new List<HomeSale>();

            foreach (var searchTerm in searchTerms)
            {
                string capSearchTerm = searchTerm.ToUpper().Trim();
                searchHomesalesResults.AddRange(MainWindow.homeSalesCollection.OfType<HomeSale>()
                    .Where(hs =>
                        hs.SaleID.ToString().Contains(searchTerm) ||
                        hs.MarketDate.ToString().Contains(searchTerm) ||
                        hs.SaleAmount.ToString().Contains(searchTerm) ||
                        hs.SoldDate.ToString().Contains(searchTerm)
                        ));
            }

            return searchHomesalesResults.Distinct().ToList();
        }

        public static List<HomeForSaleModel> GetHomesOnMarket(string searchTermsText)
        {
            var searchTerms = FormatSearchTerms.FormatTerms(searchTermsText);
            var homesOnMarket = HomeSalesSearchHelper.GetHomeSales(searchTerms, false);

            if (homesOnMarket.Count < 1)
            {
                return new List<HomeForSaleModel>();
            }

            var results = (from h in homesOnMarket
                           from hs in MainWindow.homeSalesCollection
                           where (h.HomeID == hs.HomeID && hs.SoldDate == null)
                           select new HomeForSaleModel
                           {
                               HomeID = h.HomeID,
                               Address = h.Address,
                               City = h.City,
                               State = h.State,
                               Zip = h.Zip,
                               SaleAmount = hs.SaleAmount,
                               MarketDate = hs.MarketDate
                           });

            return results.ToList();
        }

        public static List<SoldHomeModel> GetSoldHomes(string searchTermsText)
        {
            var searchTerms = FormatSearchTerms.FormatTerms(searchTermsText);
            var soldHomes = HomeSalesSearchHelper.GetHomeSales(searchTerms, true);

            if (soldHomes.Count < 1)
            {
                return new List<SoldHomeModel>();
            }

            var results = (from h in soldHomes
                           from hs in MainWindow.homeSalesCollection
                           where (h.HomeID == hs.HomeID && hs.SoldDate != null)
                           select new SoldHomeModel
                           {
                               HomeID = h.HomeID,
                               Address = h.Address,
                               City = h.City,
                               State = h.State,
                               Zip = h.Zip,
                               SaleAmount = hs.SaleAmount,
                               SoldDate = hs.SoldDate
                           });
            
            return results.ToList();
        }

    }
}
