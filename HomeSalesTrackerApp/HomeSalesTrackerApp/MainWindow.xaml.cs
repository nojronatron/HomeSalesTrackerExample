using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using HSTDataLayer.Helpers;
using HSTDataLayer;
using HomeSalesTrackerApp.CrudWindows;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HomesCollection homesCollection = null;
        public static PeopleCollection<Person> peopleCollection = null;
        public static HomeSalesCollection homeSalesCollection = null;
        public static RealEstateCosCollection reCosCollection = null;
        public static Person NewPersonAddedToCollection { get; set; }

        private int UpdateCollection()
        {
            return 0;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.DatabaseLoadCompleted)
            {
                ////  https://stackoverflow.com/questions/26353919/wpf-listview-binding-itemssource-in-xaml
                //this.DataContext = this;
                peopleCollection = new PeopleCollection<Person>();
                peopleCollection.listOfHandlers += AlertPersonAddedToCollection;
                InitializeCollections();
                DisplayStatusMessage("Database data loaded.");
            };
        }

        public static void AlertPersonAddedToCollection(Person p)
        {
            NewPersonAddedToCollection = p;
        }

        private static void InitializeCollections()
        {
            InitHomeSalesCollection();
            InitPeopleCollection();
            InitHomesCollection();
            InitRealEstateCompaniesCollection();
        }

        public static void InitRealEstateCompaniesCollection()
        {
            reCosCollection = new RealEstateCosCollection();
            List<RealEstateCompany> recos = EntityLists.GetTreeListOfRECompanies();
            foreach (var reco in recos)
            {
                reCosCollection.Add(reco);
            }
        }

        public static void InitHomesCollection()
        {
            homesCollection = new HomesCollection();
            List<Home> homes = EntityLists.GetTreeListOfHomes();
            foreach (var home in homes)
            {
                homesCollection.Add(home);
            }
        }

        public static void InitPeopleCollection()
        {
            peopleCollection = new PeopleCollection<Person>();
            List<Person> people = EntityLists.GetTreeListOfPeople();
            foreach (var person in people)
            {
                peopleCollection.Add(person);
            }
        }

        public static void InitHomeSalesCollection()
        {
            homeSalesCollection = new HomeSalesCollection();
            List<HomeSale> homeSales = EntityLists.GetTreeListOfHomeSales();
            foreach (var homeSale in homeSales)
            {
                homeSalesCollection.Add((HomeSale)homeSale);
            }
        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            //searchResultsListview.ItemsSource = null;
            searchTermsTextbox.Text = string.Empty;
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready.");
        }

        private void ClearSearchResultsViews()
        {
            FoundHomesView.Visibility = Visibility.Hidden;
            FoundHomesForSaleView.Visibility = Visibility.Hidden;
            FoundSoldHomesView.Visibility = Visibility.Hidden;
            FoundPeopleView.Visibility = Visibility.Hidden;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //  App.Exit triggers Application_Exit Method that calls data layer to backup DB to XML
            //this.Close();
            App.Current.Shutdown();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            //  App.Exit triggers Application_Exit Method that calls data layer to backup DB to XML
            App.Current.Shutdown();
            //this.Close();
        }

        /// <summary>
        /// Search for existing Homes using user provided search terms in the Search textbox. Return results to customer ListView with headers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSearchHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            var searchResults = new List<Home>();
            var searchTerms = new List<String>();
            string searchTermsText = searchTermsTextbox.Text;
            string[] searchTermsArr = searchTermsText.Split(',');
            searchTerms = searchTermsArr.Where(st => st.Length > 0 && string.IsNullOrEmpty(st) == false).ToList();
            
            if (searchTerms.Count > 0)
            {
                HomeSearchHelper(ref searchResults, ref searchTerms);
            }

            if (searchResults.Count > 0)
            {
                //  deliver results to the screen
                var results = (from h in searchResults
                               where h != null
                               select new
                               {
                                   HomeID = h.HomeID,
                                   Address = h.Address,
                                   City = h.City,
                                   State = h.State,
                                   Zip = h.Zip,
                               });

                var listResults = results.ToList();
                FoundHomesView.ItemsSource = listResults;
                FoundHomesView.Visibility = Visibility.Visible;
                DisplayStatusMessage($"Displaying { listResults.Count } search results.");
            }

            if (searchTerms.Count < 1 || searchResults.Count < 1)
            {
                DisplayZeroResultsMessage();
            }
        }

        /// <summary>
        /// Uses search terms in Search textbox to find Homes For Sale and returns results to custom Search Results ListView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuSearchHomesForSale_Click(Object sender, RoutedEventArgs args)
        {
            ClearSearchResultsViews();
            var searchResults = new List<Home>();
            var searchTerms = new List<String>();
            string searchTermsText = searchTermsTextbox.Text;
            string[] searchTermsArr = searchTermsText.Split(',');
            searchTerms = searchTermsArr.Where(st => st.Length > 0 && string.IsNullOrEmpty(st) == false).ToList();
            HomeSalesSearchHelper(ref searchResults, ref searchTerms, sold: false);

            if (searchResults.Count > 0)
            {
                //  deliver results to the screen
                var results = (from h in searchResults
                               from hs in homeSalesCollection
                               where h.HomeID == hs.HomeID //&& hs.SoldDate == null
                               select new
                               {
                                   HomeID = h.HomeID,
                                   Address = h.Address,
                                   City = h.City,
                                   State = h.State,
                                   Zip = $"{h.Zip:#####-####}",
                                   SaleAmount = hs.SaleAmount,
                                   MarketDate = hs.MarketDate
                               });

                var listResults = results.ToList();
                FoundHomesForSaleView.ItemsSource = listResults;
                FoundHomesForSaleView.Visibility = Visibility.Visible;
                DisplayStatusMessage($"Displaying { listResults.Count } search results.");
            }

            if (searchTerms.Count < 1 || searchResults.Count < 1)
            {
                DisplayZeroResultsMessage();
            }
        }

        /// <summary>
        /// NEED TO FINISH IMPLEMENTING!! Use the Search textbox and UpdateHomeAsSold Menu Item to search for and update a HomeSale, Home, and OwnerID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUpdateHomeAsSold_Click(object sender, RoutedEventArgs e)
        {
            //  1)  Search for home for sale in the HomeSales collection
            //      1a) Take home info from search box
            //      2a) Store HomeID for use with HomeSales and Homes Collections
            //  2)  Update:
            //          Sale Amount; Buyer ID (add if not in the system); Sold Date
            //  3)  Search for home in the Homes collection
            //  4)  Update:
            //          OwnerID (add if not in the system for same person as buyer)
            //  5)  Propagate update to the Entities (save to DB)
            //  6)  Allow user to abandon this process
            List<HomeSale> homesSalesForSale = homeSalesCollection.Where(hs => hs.SoldDate == null).ToList();

            var searchResultsForSale = new List<Home>();
            var searchTerms = new List<string>();
            string searchTermsText = searchTermsTextbox.Text;
            searchTerms = FormatSearchTerms(searchTermsText);
            
            HomeSalesSearchHelper(ref searchResultsForSale, ref searchTerms, false);

            var searchResult = (from hsForSale in homesSalesForSale
                                from srForSale in searchResultsForSale
                                where srForSale.HomeID == hsForSale.HomeID
                                select hsForSale).ToList();

            int selectedHomeID = -1;

            if (searchResult.Count > 1)
            {
                DisplayStatusMessage("More than one Home For Sale found. Refine your search terms and try again.");
            }
            else if (searchResult.Count == 0)
            {
                DisplayStatusMessage("No Homes For Sale found. Refine your search terms and try again.");
            }

            else
            {
                selectedHomeID = searchResult.First().HomeID;
                HomeSale homeSaleToUpdate = homeSalesCollection.FirstOrDefault(hs => hs.HomeID == selectedHomeID);
                homeSaleToUpdate.SaleAmount = 0m;   //  TODO: get SaleAmount (decimal) from user
                homeSaleToUpdate.BuyerID = 0;   //  TODO: get new or existing BuyerID information
                homeSaleToUpdate.SoldDate = new DateTime(2020, 7, 31);  //  TODO: get new SoldDate from user

                Home homeToUpdate = homesCollection.FirstOrDefault(h => h.HomeID == selectedHomeID);
                homeToUpdate.OwnerID = 0;   //  TODO: acquire the PersonID and insert it here

                //  TODO: Save to Entities

                //  TODO: Update HomeCollection

                //  TODO: Update HomeSalesCollection
            }

        }

        /// <summary>
        /// Search for home on-the-market with user-provided search terms and if found, remove it from market.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuRemoveHomeFromMarket_Click(object sender, RoutedEventArgs e)
        {
            //  Golden Path 4: Remove a home off the market
            //  1)  Search > HomeSale Collection for user-provided search term(s)
            //  2)  Home is sold (has marketdate value) ? StatusMessage tells user cannot be removed
            //      Otherwise REMOVE MARKETDATE from Collection
            //  3)  Update HomeSale Collection and Database Context
            //      Allow the user to abandon the remove/update process

            //List<HomeSale> homeSalesWithMarketDate = homeSalesCollection.Where(hs => hs.MarketDate != null).ToList();

            var searchResults = new List<Home>();
            var searchTerms = new List<String>();
            string searchTermsText = searchTermsTextbox.Text;
            if (searchTermsText.Length < 1)
            {
                DisplayStatusMessage("Enter one or more search terms in the Search textbox, then click the Remove menu.");
                return;
            }
            searchTerms = FormatSearchTerms(searchTermsText);
            HomeSalesSearchHelper(ref searchResults, ref searchTerms, sold: false);

            HomeSale homesale = (from sr in searchResults
                                 from hs in homeSalesCollection
                                 where hs.HomeID == sr.HomeID
                                 select hs).FirstOrDefault();

            StringBuilder statusMessage = new StringBuilder($"About to remove HomeID { homesale.HomeID } off the Market.");

            //  TODO: test allowing user to go-ahead or cancel out.
            MessageBoxResult mbr = MessageBox.Show(statusMessage.ToString(), "Confirm", MessageBoxButton.YesNo);
            statusMessage.Clear();

            if (mbr == MessageBoxResult.Yes)
            {
                if (homesale != null)
                {
                    if (homesale.MarketDate == null)
                    {
                        if (homeSalesCollection.Remove(homesale))
                        {
                            //  TODO: Verify DB updated using LogicBroker.RemoveEntity() method
                            LogicBroker.RemoveEntity<HomeSale>(homesale);
                            statusMessage.Append("Removed Home off the Market.");
                        }
                        else
                        {
                            statusMessage.Append("Unable to remove Home off the Market. Call Support Line for assistance.");
                        }
                    }
                    else
                    {
                        statusMessage.Append("Home is sold. Cannot remove from Market.");
                    }
                }
                else
                {
                    statusMessage.Append("Home Sale not found. Try again using different search terms.");
                }
            }
            else
            {
                statusMessage.Append("Aborting operation. Home Sale will not be removed from the Market.");
            }

            DisplayStatusMessage(statusMessage.ToString());
        }

        /// <summary>
        /// Private helper method that cleans-up user-input prior to executing a search.
        /// </summary>
        /// <param name="searchTermsText"></param>
        /// <returns></returns>
        private List<string> FormatSearchTerms(string searchTermsText)
        {
            string[] searchTermsArr = searchTermsText.Split(',');
            var searchTermsList = new List<string>();
            searchTermsList = searchTermsArr.Where(st => st.Length > 0 && string.IsNullOrEmpty(st) == false).ToList();
            return searchTermsList;
        }

        /// <summary>
        /// private helper method to search HomeSalesCollection for items in Search Terms Textbox.
        /// REF's SearchResults (Home), SearchTerms (string). Can return sold and unsold HomeSales.
        /// </summary>
        /// <param name="searchResults"></param>
        /// <param name="searchTerms"></param>
        /// <param name="sold"></param>
        private static void HomeSalesSearchHelper(ref List<Home> searchResults, ref List<string> searchTerms, bool sold)
        {
            if (searchTerms.Count > 0)
            {
                if (sold == false)
                {
                    var unsoldHomes = (from hs in homeSalesCollection
                                       from h in homesCollection
                                       where hs.SoldDate == null && hs.HomeID == h.HomeID
                                       select h).ToList();

                    HomeSearchHelper(ref searchResults, ref searchTerms);

                    searchResults = (from sr in searchResults
                                     from uh in unsoldHomes
                                     where sr.HomeID == uh.HomeID
                                     select sr).ToList();

                    searchResults.Distinct();
                }
                else
                {
                    var soldHomes = (from sh in homeSalesCollection
                                     from h in homesCollection
                                     where sh.SoldDate != null && sh.HomeID == h.HomeID
                                     select h).ToList();

                    HomeSearchHelper(ref searchResults, ref searchTerms);

                    searchResults = (from sr in searchResults
                                     from sh in soldHomes
                                     where sr.HomeID == sh.HomeID
                                     select sr).ToList();

                    searchResults.Distinct();
                }
            }
        }

        private void MenuAddHome_Click(object sender, RoutedEventArgs e)
        {
            AddHomeWindow ahw = new AddHomeWindow();
            ahw.AddType = "Home";
            ahw.Show();
        }

        private void MenuAddOwner_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Owner";
            apw.Show();
        }

        private void MenuAddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Agent";
            apw.Show();
        }

        private void MenuAddBuyer_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Buyer";
            apw.Show();
        }


        private void menuDisplaySoldHomes_Click(object sender, RoutedEventArgs e)
        {
            SoldHomesReport shr = new SoldHomesReport();
            shr.Show();
        }
        
        private void menuDisplayBuyers_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        
        private void menuDisplayAgents_Click(object sender, RoutedEventArgs e)
        {
            AgentsResultsReport arr = new AgentsResultsReport();
            arr.Show();
        }

        private void menuAboutAppInfo_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxCaption = "About: Home Sales Tracker App";
            string messageBoxText = $"App: Home Sales Tracker\nAuthor: Jon Rumsey\nDeadline: 20-Aug-2020";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, messageBoxCaption, button, icon);
        }

        private void modifyItemButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Modify button not yet implemented");
        }

        private void DisplayStatusMessage(string message)
        {
            statusBarText.Text = message.Trim().ToString();
        }

        private void DisplayZeroResultsMessage()
        {
            ClearSearchResultsViews();
            DisplayStatusMessage($"Search returned 0 results. Try different search term(s).");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  Placeholder Window_Closing method. There could be actions to take here
        }

        private void FullDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Full Details button clicked. Not yet implemented");
        }

        private static void HomeSearchHelper(ref List<Home> searchResults, ref List<string> searchTerms)
        {
            foreach (var searchTerm in searchTerms)
            {
                string capSearchTerm = searchTerm.ToUpper().Trim();
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.Address.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.City.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.State.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.Zip.Contains(searchTerm)));
            }

            searchResults.Distinct();
        }

        private void MenuUpdateHome_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void MenuUpdateHomeForSale_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateOwner_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateAgent_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateBuyer_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void MenuSearchSoldHomes_Click(object sender, RoutedEventArgs e)
        {
            //  SoldDate != null
            ClearSearchResultsViews();
            FoundSoldHomesView.Visibility = Visibility.Visible;
        }

        private void MenuSearchOwners_Click(object sender, RoutedEventArgs e)
        {
            //
            ClearSearchResultsViews();
            FoundPeopleView.Visibility = Visibility.Visible;
        }

        private void MenuSearchAgents_Click(object sender, RoutedEventArgs e)
        {
            //
            ClearSearchResultsViews();
            FoundPeopleView.Visibility = Visibility.Visible;

        }

        private void MenuSeawrchBuyers_Click(object sender, RoutedEventArgs e)
        {
            //
            ClearSearchResultsViews();
            FoundPeopleView.Visibility = Visibility.Visible;

        }

        private void menuAddHomesForSale_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateSoldHome_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuDisplayRECoTotals_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuDisplayHomesForSale_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
