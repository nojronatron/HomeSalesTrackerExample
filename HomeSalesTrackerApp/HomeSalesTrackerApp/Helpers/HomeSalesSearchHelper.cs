using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    public static class HomeSalesSearchHelper
    {
        /// <summary>
        /// Return sold home items matching input search terms. Searches properties of both HomeSales and Homes.
        /// Returns empty List if nothing matches.
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static List<SoldHomeModel> GetSoldHomesSearchResults(List<string> searchTerms)
        {
            var foundHfsItems = new List<HomeSale>();
            var foundHomeItems = new List<Home>();
            var soldHomesaleSearchResults = new List<SoldHomeModel>();

            foundHfsItems = SearchHomeForSaleItems(searchTerms);
            foundHomeItems = HomeSearchHelper.SearchHomeItems(searchTerms);

            foreach (var homeItem in foundHomeItems)
            {
                foundHfsItems.AddRange(MainWindow.homeSalesCollection.Retreive(homeItem));
            }

            soldHomesaleSearchResults = (from hs in foundHfsItems
                                         where hs.SoldDate != null
                                         select new SoldHomeModel()
                                         {
                                             HomeID = hs.HomeID,
                                             HomeSaleID = hs.SaleID,
                                             Address = hs.Home.Address,
                                             City = hs.Home.City,
                                             State = hs.Home.State,
                                             Zip = hs.Home.Zip,
                                             SaleAmount = hs.SaleAmount,
                                             SoldDate = hs.SoldDate
                                         }).ToList();

            return soldHomesaleSearchResults;
        }

        /// <summary>
        /// Return home for sale items matching input search terms. Searches properties of both HomeSales and Homes.
        /// Returns empty List if nothing matches.
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static List<HomeForSaleModel> GetHomesForSaleSearchResults(List<string> searchTerms)
        {
            var foundHfsItems = new List<HomeSale>();
            var foundHomeItems = new List<Home>();
            var soldHomesaleSearchResults = new List<HomeForSaleModel>();

            foundHfsItems = SearchHomeForSaleItems(searchTerms);
            foundHomeItems = HomeSearchHelper.SearchHomeItems(searchTerms);

            foreach (var homeItem in foundHomeItems)
            {
                foundHfsItems.AddRange(MainWindow.homeSalesCollection.Retreive(homeItem));
            }

            soldHomesaleSearchResults = (from hs in foundHfsItems
                                         where hs.Buyer == null
                                         select new HomeForSaleModel()
                                         {
                                             HomeID = hs.HomeID,
                                             HomeForSaleID = hs.SaleID,
                                             Address = hs.Home.Address,
                                             City = hs.Home.City,
                                             State = hs.Home.State,
                                             Zip = hs.Home.Zip,
                                             SaleAmount = hs.SaleAmount,
                                             MarketDate = hs.MarketDate
                                         }).ToList();

            return soldHomesaleSearchResults;
        }

        /// <summary>
        /// Search Home Sales entities and return list of entities who have matching elements.
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static List<HomeSale> SearchHomeForSaleItems(List<string> searchTerms)
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
