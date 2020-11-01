using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    class HomeSearchTool
    {
        private List<string> SearchTerms { get; set; }

        public List<HomeSearchModel> SearchResults { get; set; }

        public HomeSearchTool(List<string> searchTerms)
        {
            SearchTerms = searchTerms;
            HomeSearch();
        }

        private void HomeSearch()
        {
            if (SearchTerms.Count < 1)
            {
                SearchResults = new List<HomeSearchModel>();
                return;
            }

            var searchResults = new List<Home>();

            foreach (var searchTerm in SearchTerms)
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

            if (searchResults.Count > 0)
            {
                var preliminaryResults = (from h in searchResults
                                          where h != null
                                          select new HomeSearchModel
                                          {
                                              HomeID = h.HomeID,
                                              Address = h.Address,
                                              City = h.City,
                                              State = h.State,
                                              Zip = h.Zip,
                                          });

                SearchResults = preliminaryResults.ToList();

            }
        }

    }
}
