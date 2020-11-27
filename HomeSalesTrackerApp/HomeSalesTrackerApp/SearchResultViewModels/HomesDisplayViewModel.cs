using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;
using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.SearchResultViewModels
{
    public class HomesDisplayViewModel
    {
        private List<string> _formattedSearchTerms;
        public List<HomeDisplayModel> FoundHomes { get; set; }

        public HomesDisplayViewModel(List<string> formattedSearchTerms)
        {
            _formattedSearchTerms = new List<string>(formattedSearchTerms);
            LoadFoundHomes();
        }

        private void LoadFoundHomes()
        {
            List<Home> listResults = HomeSearchHelper.SearchHomeItems(_formattedSearchTerms);
            FoundHomes = new List<HomeDisplayModel>();

            foreach(Home home in listResults)
            {
                FoundHomes.Add(new HomeDisplayModel()
                {
                    HomeID = home.HomeID,
                    Address = home.Address,
                    City = home.City,
                    State = home.State,
                    Zip = home.Zip
                });
            }

        }

    }
}
