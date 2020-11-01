using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    public static class HomeSearchHelper
    {
        public static List<Home> SearchHomes(List<string> searchTerms)
        {
            var searchResults = new List<Home>();
            
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

            return searchResults;
        }

    }
}
