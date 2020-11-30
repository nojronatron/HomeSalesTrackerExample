using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    public static class HomeSearchHelper
    {
        /// <summary>
        /// Search Home instances in the collection for matching criteria on HomeID, Address, City, State, and Zip.
        /// Returns a List of matchin Home entities; empty if none found.
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        public static List<Home> SearchHomeItems(List<string> searchTerms)
        {
            var searchResults = new List<Home>();

            if (searchTerms.Count > 0)
            {

                foreach (var searchTerm in searchTerms)
                {
                    string capSearchTerm = searchTerm.ToUpper().Trim();
                    searchResults.AddRange(MainWindow.homesCollection.OfType<Home>().Where(
                        hc =>
                            hc.HomeID.ToString().Contains(capSearchTerm) ||
                            hc.Address.ToUpper().Contains(capSearchTerm) ||
                            hc.City.ToUpper().Contains(capSearchTerm) ||
                            hc.State.ToUpper().Contains(capSearchTerm) ||
                            hc.Zip.Contains(searchTerm)));
                }

                searchResults = searchResults.Distinct().ToList();
            }

            return searchResults;
        }

        public static String GetHomeItemDetails(HomeDisplayModel selectedHome)
        {
            if (selectedHome != null)
            {
                DateTime? placeholder = null;
                HomeDisplayDetailModel SelectedHomeDetail = null;
                SelectedHomeDetail = (from hfs in MainWindow.homeSalesCollection
                                      where hfs.HomeID == selectedHome.HomeID &&
                                        hfs.MarketDate != null
                                      join h in MainWindow.homesCollection on hfs.HomeID equals h.HomeID
                                      join p in MainWindow.peopleCollection on h.OwnerID equals p.PersonID
                                      select new HomeDisplayDetailModel
                                      {
                                          HomeID = h.HomeID,
                                          Address = h.Address,
                                          City = h.City,
                                          State = h.State,
                                          Zip = h.Zip,
                                          MarketDate = (hfs.Buyer != null) ? placeholder : hfs.MarketDate,
                                          owner = new OwnerModel
                                          {
                                              OwnerID = p.PersonID,
                                              FirstName = p.FirstName,
                                              LastName = p.LastName,
                                              Phone = p.Phone,
                                              Email = p.Email,
                                              PreferredLender = p.Owner.PreferredLender
                                          }
                                      }).FirstOrDefault();

                if (SelectedHomeDetail != null)
                {
                    return SelectedHomeDetail.ToStackedString();
                }
            }

            return string.Empty;
        }

    }

}
