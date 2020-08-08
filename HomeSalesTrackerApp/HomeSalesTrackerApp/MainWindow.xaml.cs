using HomeSalesTrackerApp.CrudWindows;

using HSTDataLayer;
using HSTDataLayer.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public Person APerson { get; set; }
        public Owner AnOwner { get; set; }
        public Home AHome { get; set; }
        public RealEstateCompany RECo { get; set; }
        public Agent AnAgent { get; set; }
        public Buyer ABuyer { get; set; }
        public HomeSale AHomeSale { get; set; }

        public static Person NewPersonAddedToCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.DatabaseLoadCompleted)
            {
                //peopleCollection = new PeopleCollection<Person>();
                //peopleCollection.listOfHandlers += AlertPersonAddedToCollection;
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
            List<Person> people = EntityLists.GetListOfPeople();
            foreach (var person in people)
            {
                peopleCollection.Add(person);
            }
        }

        public static void InitHomeSalesCollection()
        {
            homeSalesCollection = new HomeSalesCollection();
            List<HomeSale> homeSales = EntityLists.GetListOfHomeSales();
            foreach (var homeSale in homeSales)
            {
                homeSalesCollection.Add(homeSale);
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
            App.Current.Shutdown();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            //  App.Exit triggers Application_Exit Method that calls data layer to backup DB to XML
            App.Current.Shutdown();
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
                               select new HomeSearchView
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
                DisplayStatusMessage($"Found { listResults.Count } Homes. Select a result and use Update or Remove menus to make changes.");
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
                               where (h.HomeID == hs.HomeID && hs.SoldDate == null)
                               select new HomeForSaleView
                               {
                                   HomeID = h.HomeID,
                                   Address = h.Address,
                                   City = h.City,
                                   State = h.State,
                                   Zip = h.Zip,
                                   SaleAmount = hs.SaleAmount,
                                   MarketDate = hs.MarketDate
                               });

                var listResults = results.ToList();
                FoundHomesForSaleView.ItemsSource = listResults;
                FoundHomesForSaleView.Visibility = Visibility.Visible;
                DisplayStatusMessage($"Found { listResults.Count } Homes For Sale. Select a result and use Update or Remove menus to make changes.");
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

            //  TODO: Complete MenuUpdateHomeAsSold_Click method

            //  TODO: Save to Entities
            LogicBroker.SaveEntity<HomeSale>(new HomeSale()); //  context.Save() will update db with changes only according to text in "Pro C# 7" book

            //  Update collections
            InitializeCollections();
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

            HomeForSaleView selectedHomeForSale = (HomeForSaleView)this.FoundHomesForSaleView.SelectedItem;
            if (selectedHomeForSale != null)
            {
                int homeID = selectedHomeForSale.HomeID;
                var homeSaleToRemove = (from h in homesCollection
                                        from hs in homeSalesCollection
                                        where h.HomeID == hs.HomeID 
                                            && hs.MarketDate != null 
                                            && hs.SoldDate == null 
                                        select hs).FirstOrDefault();

                if (homeSaleToRemove != null)
                {
                    LogicBroker.RemoveEntity<HomeSale>(homeSaleToRemove);
                    InitializeCollections();

                    DisplayStatusMessage($"Removing { selectedHomeForSale.Address }, SaleID { homeSaleToRemove.SaleID } from For Sale Market.");
                    ClearSearchResultsViews();  //  user doesn't need to see remaining results right?
                }
                else
                {
                    DisplayStatusMessage("Home For Sale not found.");
                }

            }
            else
            {
                DisplayStatusMessage("Select an item in the search results before choosing to remove it from the Market.");
            }

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
            var homeSearchResults = new List<Home>();
            var soldOrUnsoldHomes = new List<Home>();
            if (searchTerms.Count > 0)
            {
                if (sold == false)
                {
                    soldOrUnsoldHomes = (from hs in homeSalesCollection
                                         from h in homesCollection
                                         where hs.SoldDate == null && hs.HomeID == h.HomeID
                                         select h).ToList();
                }
                else
                {
                    soldOrUnsoldHomes = (from sh in homeSalesCollection
                                         from h in homesCollection
                                         where sh.SoldDate != null && sh.HomeID == h.HomeID
                                         select h).ToList();
                }

                HomeSearchHelper(ref homeSearchResults, ref searchTerms);

                searchResults = (from sr in homeSearchResults
                                 from sh in soldOrUnsoldHomes
                                 where sr.HomeID == sh.HomeID
                                 select sr).ToList();

                searchResults.Distinct();
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
            //  take a selected HomeForSale item and pass it to UpdaterWindow with arguments to update HomeSale
            UpdaterWindow uw = new UpdaterWindow();
            uw.UpdateType = "HomeSale";
            uw.Show();
        }

        private void menuUpdateOwner_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateAgent_Click(object sender, RoutedEventArgs e)
        {
            //  when a HomeSale is selected in the Search Results window
            //  this menu item can be used to change the Agent (new or existing)
            var selectedHomesaleView = FoundHomesForSaleView.SelectedItem as HomeForSaleView;

            Agent hsAgent = (from h in homesCollection
                             from hs in homeSalesCollection
                             from a in peopleCollection
                             where h.HomeID == selectedHomesaleView.HomeID
                             where hs.HomeID == selectedHomesaleView.HomeID
                             where a.PersonID == hs.AgentID
                             select a.Agent).FirstOrDefault();

            HomeSale homesale = (from hs in homeSalesCollection
                                 from h in homesCollection
                                 where selectedHomesaleView.Address == h.Address &&
                                 selectedHomesaleView.Zip == h.Zip
                                 where hs.HomeID == h.HomeID
                                 select hs).FirstOrDefault();

            UpdaterWindow uw = new UpdaterWindow();
            uw.UpdateType = "Agent";
            uw.UpdateAgent = hsAgent;
            uw.UpdateHomeSale = homesale;
            uw.Show();
        }

        private void menuUpdateBuyer_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void MenuSearchSoldHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            FoundSoldHomesView.Visibility = Visibility.Visible;
        }

        private void MenuSearchOwners_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            FoundPeopleView.Visibility = Visibility.Visible;
        }

        private void MenuSearchAgents_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            FoundPeopleView.Visibility = Visibility.Visible;

        }

        private void MenuSearchBuyers_Click(object sender, RoutedEventArgs e)
        {
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
            //
        }

        private void RemoveHomeSaleAndUpdateCollection(HomeSale homesale)
        {
            if (LogicBroker.RemoveEntity<HomeSale>(homesale))
            {
                InitHomesCollection();
                InitHomeSalesCollection();
                InitRealEstateCompaniesCollection();
            }
            else
            {
                DisplayStatusMessage("Unable to update database with this home.");
            }
        }

        class HomeSearchView
        {
            public int HomeID { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; } 
            public override string ToString()
            {
                return $"{ this.HomeID }{ this.Address }{ this.City }{ this.State }{ this.Zip }";
            }
        }

        class HomeForSaleView : HomeSearchView
        {
            public Decimal SaleAmount { get; set; }
            public DateTime MarketDate { get; set; }
            public HomeForSaleView() { }

            public override string ToString()
            {
                return $"{ base.ToString() }{this.SaleAmount:C2}{ this.MarketDate }";
            }
        }

    }
}
