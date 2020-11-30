using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.SearchResultViewModels
{
    public class PeopleDisplayViewModel
    {
        public List<PersonModel> FoundPeople { get; set; }
        private List<string> FormattedSearchTerms { get; set; }

        public PeopleDisplayViewModel(List<string> formattedSearchTerms)
        {
            FormattedSearchTerms = formattedSearchTerms;
            Load();
        }

        private void Load()
        {
            var pst = new PeopleSearchTool(FormattedSearchTerms);
            var foundPeople = pst.SearchResults;
            
            if (foundPeople.Count > 0)
            {
                FoundPeople = foundPeople;
            }
            else
            {
                FoundPeople = new List<PersonModel>();
            }

        }

    }
}
