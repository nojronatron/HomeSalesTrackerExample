using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSalesTrackerApp.SearchResultViewModels
{
    public class SoldHomesDisplayViewModel
    {
        public List<SoldHomeModel> FoundSoldHomes { get; set; }
        private List<string> _formattedSearchTerms;

        public SoldHomesDisplayViewModel(List<string> formattedSearchTerms)
        {
            _formattedSearchTerms = formattedSearchTerms;
            LoadData();
        }

        private void LoadData()
        {
            FoundSoldHomes = HomeSalesSearchHelper.GetSoldHomesSearchResults(_formattedSearchTerms);

        }
    }
}
