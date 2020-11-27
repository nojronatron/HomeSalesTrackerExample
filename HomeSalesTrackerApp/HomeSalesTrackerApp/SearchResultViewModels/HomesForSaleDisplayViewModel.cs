using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Collections.Generic;

namespace HomeSalesTrackerApp.SearchResultViewModels
{
    public class HomesForSaleDisplayViewModel
    {
        private List<string> _formattedSerachTerms;
        public List<HomeForSaleModel> FoundHomesForSale { get; set; }
        public HomesForSaleDisplayViewModel(List<string> formattedSearchTerms)
        {
            _formattedSerachTerms = new List<string>(formattedSearchTerms);
            LoadFoundHomesForSale();
        }
        private void LoadFoundHomesForSale()
        {
            FoundHomesForSale = HomeSalesSearchHelper.GetHomesForSaleSearchResults(_formattedSerachTerms);
            
        }
    }
}
