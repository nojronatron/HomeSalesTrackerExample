using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    public static class HomeSalesSearchHelper
    {
        public static List<Home> GetHomeSales(List<string> searchTerms, bool sold)
        {
            var homeSearchResults = new List<Home>();
            var soldOrUnsoldHomes = new List<Home>();
            if (searchTerms.Count > 0)
            {
                if (sold == false)
                {
                    soldOrUnsoldHomes = (from hs in MainWindow.homeSalesCollection
                                         from h in MainWindow.homesCollection
                                         where hs.SoldDate == null && hs.HomeID == h.HomeID
                                         select h).ToList();
                }
                else
                {
                    soldOrUnsoldHomes = (from sh in MainWindow.homeSalesCollection
                                         from h in MainWindow.homesCollection
                                         where sh.SoldDate != null && sh.HomeID == h.HomeID
                                         select h).ToList();
                }

                homeSearchResults = HomeSearchHelper.SearchHomes(searchTerms);

                var searchResults = (from sr in homeSearchResults
                                     from sh in soldOrUnsoldHomes
                                     where sr.HomeID == sh.HomeID
                                     select sr).ToList();

                homeSearchResults = searchResults.Distinct().ToList();
            }

            return homeSearchResults;
        }

    }
}
