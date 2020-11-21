using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    public static class HomeSalesSearchHelper
    {
        public static List<SoldHomeModel> GetSoldHomesSearchResults(List<string> searchTerms)
        {
            var soldHomesaleSearchResults = new List<SoldHomeModel>();

            if (searchTerms.Count > 0)
            {
                var matchingHomesaleItems = HomeSalesSearchHelper.GetMatchingHomesaleItems(searchTerms);

                soldHomesaleSearchResults = (from hs in matchingHomesaleItems
                                             from h in MainWindow.homesCollection
                                             where hs.SoldDate != null && hs.HomeID == h.HomeID
                                             select new SoldHomeModel()
                                             {
                                                 HomeID = hs.HomeID,
                                                 HomeSaleID = hs.SaleID,
                                                 Address = h.Address,
                                                 City = h.City,
                                                 State = h.State,
                                                 Zip = h.Zip,
                                                 SaleAmount = hs.SaleAmount,
                                                 SoldDate = hs.SoldDate
                                             }).ToList();
            }

            return soldHomesaleSearchResults;
        }

        public static List<HomeForSaleModel> GetHomesForSaleSearchResults(List<string> searchTerms)
        {
            //  purpose: return a list of HomeForSaleModel items that match search criteria and are home sales On Market
            var homesaleSearchResults = new List<HomeForSaleModel>();
            
            if (searchTerms.Count > 0)
            {
                var matchingHomesaleItems = HomeSalesSearchHelper.GetMatchingHomesaleItems(searchTerms);

                homesaleSearchResults = (from hs in matchingHomesaleItems
                                       from h in MainWindow.homesCollection
                                       where hs.SoldDate == null && hs.HomeID == h.HomeID
                                       select new HomeForSaleModel()
                                       {
                                           HomeID = hs.HomeID,
                                           HomeForSaleID = hs.SaleID,
                                           Address = h.Address,
                                           City = h.City,
                                           State = h.State,
                                           Zip = h.Zip,
                                           MarketDate = hs.MarketDate,
                                           SaleAmount = hs.SaleAmount
                                       }).ToList();

            }

            return homesaleSearchResults;
        }

        /// <summary>
        /// Search Home Sales entities and return list of entities who have matching elements.
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
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

    }
}
