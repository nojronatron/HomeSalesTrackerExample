using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.SearchResultViewModels
{
    public class HomesForSaleDisplayViewModel
    {
        private List<string> _formattedSearchTerms;
        public List<HomeForSaleModel> FoundHomesForSale { get; set; }
        public HomesForSaleDisplayViewModel(List<string> formattedSearchTerms)
        {
            _formattedSearchTerms = new List<string>(formattedSearchTerms);
            LoadFoundHomesForSale();
        }
        private void LoadFoundHomesForSale()
        {
            FoundHomesForSale = HomeSalesSearchHelper.GetHomesForSaleSearchResults(_formattedSearchTerms);
            
        }
    }
}
