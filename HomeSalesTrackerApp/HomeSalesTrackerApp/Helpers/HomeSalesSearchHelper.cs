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

            foundHfsItems = foundHfsItems.Distinct().ToList();

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
            var homesForSaleSearchResults = new List<HomeForSaleModel>();

            foundHfsItems = SearchHomeForSaleItems(searchTerms);
            foundHomeItems = HomeSearchHelper.SearchHomeItems(searchTerms);

            foreach (var homeItem in foundHomeItems)
            {
                foundHfsItems.AddRange(MainWindow.homeSalesCollection.Retreive(homeItem));
            }

            foundHfsItems = foundHfsItems.Distinct().ToList();

            homesForSaleSearchResults = (from hs in foundHfsItems
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

            return homesForSaleSearchResults;
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

        public static string GetHomeForSaleItemDetails(HomeForSaleModel selectedHomeForSale)
        {
            if (selectedHomeForSale != null)
            {
                var homeForSaleDetails = new HomeForSaleDetailModel();
                homeForSaleDetails = (from hfs in MainWindow.homeSalesCollection
                                      where hfs.HomeID == selectedHomeForSale.HomeID &&
                                        hfs.MarketDate == selectedHomeForSale.MarketDate
                                      join h in MainWindow.homesCollection on hfs.HomeID equals h.HomeID
                                      join reco in MainWindow.reCosCollection on hfs.CompanyID equals reco.CompanyID
                                      join o in MainWindow.peopleCollection on h.OwnerID equals o.PersonID
                                      join a in MainWindow.peopleCollection on hfs.AgentID equals a.PersonID
                                      select new HomeForSaleDetailModel
                                      {
                                          HomeID = h.HomeID,
                                          Address = h.Address,
                                          City = h.City,
                                          State = h.State,
                                          Zip = h.Zip,
                                          SaleAmount = hfs.SaleAmount,
                                          MarketDate = hfs.MarketDate,
                                          OwnerFirstName = o.FirstName,
                                          OwnerLastName = o.LastName,
                                          PreferredLender = h.Owner.PreferredLender,
                                          AgentFirstName = a.FirstName,
                                          AgentLastName = a.LastName,
                                          CommissionPercent = hfs.Agent.CommissionPercent,
                                          RecoName = reco.CompanyName,
                                          RecoPhone = reco.Phone,
                                      }).FirstOrDefault();

                if (homeForSaleDetails != null)
                {
                    return homeForSaleDetails.ToStackedString();
                }

            }

            return string.Empty;
        }

        public static string GetSoldHomeItemDetails(SoldHomeModel selectedSoldHome)
        {
            if (selectedSoldHome != null)
            {
                var soldHomeDetails = new SoldHomeDetailModel();
                soldHomeDetails = (from hfs in MainWindow.homeSalesCollection
                                   where hfs.HomeID == selectedSoldHome.HomeID &&
                                   hfs.SoldDate == selectedSoldHome.SoldDate
                                   join h in MainWindow.homesCollection on hfs.HomeID equals h.HomeID
                                   join o in MainWindow.peopleCollection on h.OwnerID equals o.PersonID
                                   join a in MainWindow.peopleCollection on hfs.AgentID equals a.PersonID
                                   join reco in MainWindow.reCosCollection on hfs.CompanyID equals reco.CompanyID
                                   select new SoldHomeDetailModel
                                   {
                                       HomeID = h.HomeID,
                                       Address = h.Address,
                                       City = h.City,
                                       State = h.State,
                                       Zip = h.Zip,
                                       SaleAmount = hfs.SaleAmount,
                                       MarketDate = hfs.MarketDate,
                                       OwnerFirstName = o.FirstName,
                                       OwnerLastName = o.LastName,
                                       PreferredLender = h.Owner.PreferredLender,
                                       AgentFirstName = a.FirstName,
                                       AgentLastName = a.LastName,
                                       CommissionPercent = hfs.Agent.CommissionPercent,
                                       RecoName = reco.CompanyName,
                                       RecoPhone = reco.Phone,
                                       SoldDate = hfs.SoldDate
                                   }).FirstOrDefault();

                if (soldHomeDetails != null)
                {
                    return soldHomeDetails.ToStackedString();
                }

            }

            return string.Empty;
        }

    }
}
