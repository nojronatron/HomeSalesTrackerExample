using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.Helpers
{
    /// <summary>
    /// Helper Class used to clean up user input.
    /// </summary>
    public static class FormatSearchTerms
    {
        public static List<string> FormatTerms(string commaSeparatedSearchTerms)
        {
            if (string.IsNullOrEmpty(commaSeparatedSearchTerms) || commaSeparatedSearchTerms.Length < 1)
            {
                return new List<string>();
            }

            string[] searchTermsArr = commaSeparatedSearchTerms.Split(',');
            List<string> searchTermsList = searchTermsArr.Where(st => st.Length > 0 && string.IsNullOrEmpty(st) == false).ToList();
            return searchTermsList;
        }

    }
}
