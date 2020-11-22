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

    }
}
