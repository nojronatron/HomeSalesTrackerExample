using HomeSalesTrackerApp.CrudWindows;
using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;
using HSTDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private Logger logger = null;
        public static HomesCollection homesCollection = null;
        public static PeopleCollection<Person> peopleCollection = null;
        public static HomeSalesCollection homeSalesCollection = null;
        public static RealEstateCosCollection reCosCollection = null;

        public static Person NewPersonAddedToCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logger = new Logger();
            if (App.DatabaseInitLoaded)
            {
                InitializeCollections();
                logger.Data("MainWindow Loaded", "Database data loaded.");
                DisplayStatusMessage("Database data loaded.");
            }
            else
            {
                logger.Data("MainWindow Loaded", "Database data NOT loaded.");
            }
            logger.Flush();
        }

        public static void AlertPersonAddedToCollection(Person p)
        {
            NewPersonAddedToCollection = p;
        }

        public static void InitializeCollections()
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
            ClearSearchTermsTextbox();
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready.");
        }

        private void ClearSearchTermsTextbox()
        {
            searchTermsTextbox.Text = string.Empty;
        }

        private void ClearSearchResultsViews()
        {
            FoundHomesView.ItemsSource = null;
            FoundHomesView.Visibility = Visibility.Hidden;
            FoundHomesForSaleView.ItemsSource = null;
            FoundHomesForSaleView.Visibility = Visibility.Hidden;
            FoundSoldHomesView.ItemsSource = null;
            FoundSoldHomesView.Visibility = Visibility.Hidden;
            FoundPeopleView.ItemsSource = null;
            FoundPeopleView.Visibility = Visibility.Hidden;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
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
        /// Use the Search textbox and UpdateHomeAsSold Menu Item to search for and update a HomeSale, Home, and OwnerID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUpdateHomeAsSold_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder statusMessage = new StringBuilder("Ok. ");

            HomeForSaleView selectedHomesaleView = null;
            selectedHomesaleView = FoundHomesForSaleView.SelectedItem as HomeForSaleView;
            FoundHomesForSaleView.SelectedIndex = -1;

            var homeid = selectedHomesaleView.HomeID;
            HomeSale hfsHomesale = homeSalesCollection.Where(hs => hs.HomeID == homeid && hs.MarketDate == selectedHomesaleView.MarketDate)
                                                                  .FirstOrDefault();

            Home hfsHome = new Home();
            hfsHome = homesCollection.Where(h => h.HomeID == hfsHomesale.HomeID).FirstOrDefault();
            if (hfsHome != null)
            {
                Person hfsAgent = new Person();
                hfsAgent = peopleCollection.Where(p => p.PersonID == hfsHomesale.AgentID).FirstOrDefault();

                if (hfsAgent != null && hfsAgent.Agent.CompanyID != null)
                {
                    RealEstateCompany hfsReco = new RealEstateCompany();
                    hfsReco = reCosCollection.Where(r => r.CompanyID == hfsAgent.Agent.CompanyID).FirstOrDefault();
                    if (hfsReco != null)
                    {
                        var homeUpdaterWindow = new HomeUpdaterWindow();
                        homeUpdaterWindow.UpdateType = "HomeSold";
                        homeUpdaterWindow.UpdatePerson = hfsAgent;
                        homeUpdaterWindow.UpdateAgent = hfsAgent.Agent;
                        homeUpdaterWindow.UpdateHome = hfsHome;
                        homeUpdaterWindow.UpdateHomeSale = hfsHomesale;
                        homeUpdaterWindow.UpdateReco = hfsReco;
                        DisplayStatusMessage("Loading update window");
                        homeUpdaterWindow.Show();

                    }
                    else
                    {
                        statusMessage.Append($"DB Data problem: Real Estate Co not found. ");
                    }
                }
                else
                {
                    statusMessage.Append($"Agent not associated with a Real Estate Co. ");
                }
            }
            else
            {
                statusMessage.Append($"DB Data problem: No Home found for this Sale record. ");
            }
            if (statusMessage.Length > 4)
            {
                DisplayStatusMessage(statusMessage.ToString());
            }

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
            HomeForSaleView selectedHomeForSale = (HomeForSaleView)this.FoundHomesForSaleView.SelectedItem;
            if (selectedHomeForSale != null)
            {
                int homeID = selectedHomeForSale.HomeID;
                var homeSaleToRemove = (from h in homesCollection
                                        from hs in homeSalesCollection
                                        where h.HomeID == homeID
                                            && hs.HomeID == homeID
                                            && hs.MarketDate != null
                                            && hs.SoldDate == null
                                        select hs).FirstOrDefault();

                if (homeSaleToRemove != null)
                {
                    LogicBroker.RemoveEntity<HomeSale>(homeSaleToRemove);
                    InitializeCollections();

                    DisplayStatusMessage($"Removing { selectedHomeForSale.Address }, SaleID { homeSaleToRemove.SaleID } from For Sale Market.");
                    ClearSearchResultsViews();
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
            //InitializeCollections();
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

                searchResults = searchResults.Distinct().ToList();
            }
        }

        /// <summary>
        /// Add a new Home instance to the DB and a attach it to new or existing Agent or Owner instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddHome_Click(object sender, RoutedEventArgs e)
        {
            AddHomeWindow ahw = new AddHomeWindow();
            ahw.AddType = "Home";
            ahw.Show();
        }

        /// <summary>
        /// Add a new Owner instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddOwner_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Owner";
            apw.Show();
        }

        /// <summary>
        /// Add a new Agent instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Agent";
            apw.Show();
        }

        /// <summary>
        /// Add a new Buyer instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddBuyer_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Buyer";
            apw.Show();
            ClearSearchResultsViews();
        }


        /// <summary>
        /// Display a summary of Sold Homes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuDisplaySoldHomes_Click(object sender, RoutedEventArgs e)
        {
            var soldHomesQuery = (from hs in homeSalesCollection
                                  where hs.SoldDate != null
                                  select hs).ToList();

            var shResultsList = new List<SoldHomesView>();
            SoldHomesView shv = null;
            foreach (var soldHome in soldHomesQuery)
            {
                var homeInstance = homesCollection.Where(h => h.HomeID == soldHome.HomeID).FirstOrDefault();
                var recoInstance = reCosCollection.Where(reco => reco.CompanyID == soldHome.CompanyID).FirstOrDefault();
                shv = new SoldHomesView()
                {
                    HomeID = soldHome.HomeID,
                    Address = homeInstance.Address,
                    City = homeInstance.City,
                    State = homeInstance.State,
                    Zip = homeInstance.Zip,
                    RealEstateCompanyName = recoInstance.CompanyName,
                    SaleAmount = soldHome.SaleAmount,
                    SoldDate = soldHome.SoldDate
                };

                if (soldHome.Agent != null)
                {
                    var personAgent = peopleCollection.Where(p => p.PersonID == soldHome.AgentID).FirstOrDefault();
                    string firstLastName = $"{ personAgent.FirstName } { personAgent.LastName }";
                    shv.AgentFirstLastName = firstLastName;
                    shv.FirstName = personAgent.FirstName;
                    shv.LastName = personAgent.LastName;
                    shv.Phone = personAgent.Phone;
                    shv.Email = personAgent.Email ?? null;
                }
                if (soldHome.Buyer != null)
                {
                    var personBuyer = peopleCollection.Where(p => p.PersonID == soldHome.BuyerID).FirstOrDefault();
                    string firstLastName = $"{ personBuyer.FirstName } { personBuyer.LastName }";
                    shv.BuyerFirstLastName = firstLastName;
                    shv.FirstName = personBuyer.FirstName;
                    shv.LastName = personBuyer.LastName;
                    shv.Phone = personBuyer.Phone;
                    shv.Email = personBuyer.Email ?? null;
                }
                shResultsList.Add(shv);
            }

            SoldHomesReport shr = new SoldHomesReport();
            shr.iFoundSoldHomes = shResultsList;
            shr.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("ReadY");
        }

        private void MenuDisplayBuyers_Click(object sender, RoutedEventArgs e)
        {
            var foundBuyers = (from p in peopleCollection
                               where p.Buyer != null
                               select new BuyerView
                               {
                                   PersonID = p.PersonID,
                                   FirstName = p.FirstName,
                                   LastName = p.LastName,
                                   Phone = p.Phone,
                                   Email = p.Email ?? null,
                                   CreditRating = p.Buyer.CreditRating ?? 0
                               });
            BuyersResultsReport brr = new BuyersResultsReport();
            brr.iFoundBuyers = foundBuyers;
            brr.Show();
            foundBuyers = null;
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready");
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

        private void getItemDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Detail button not yet implemented");
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
            logger.Data("Main Window Closing", "Closing.");
            logger.Flush();
        }

        private void GetItemDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Get Item Details button clicked. Not yet implemented.");
        }

        private void ModifyItemButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Modify Item Details button clicked. Not yet implemented.");
        }

        private static void HomeSearchHelper(ref List<Home> searchResults, ref List<string> searchTerms)
        {
            foreach (var searchTerm in searchTerms)
            {
                string capSearchTerm = searchTerm.ToUpper().Trim();
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.HomeID.ToString().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.Address.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.City.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.State.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(homesCollection.OfType<Home>().Where(hc => hc.Zip.Contains(searchTerm)));
            }

            searchResults = searchResults.Distinct().ToList();
        }

        private static void PersonSearchHelper(ref List<Person> searchResults, ref List<string> searchTerms)
        {
            foreach (var searchTerm in searchTerms)
            {
                string capSearchTerm = searchTerm.ToUpper().Trim();
                searchResults.AddRange(peopleCollection.OfType<Person>().Where(p => p.PersonID.ToString().Contains(capSearchTerm)));
                searchResults.AddRange(peopleCollection.OfType<Person>().Where(p => p.FirstName.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(peopleCollection.OfType<Person>().Where(p => p.LastName.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(peopleCollection.OfType<Person>().Where(p => p.Phone.ToUpper().Contains(capSearchTerm)));
                searchResults.AddRange(peopleCollection.OfType<Person>().Where(p => !string.IsNullOrEmpty(p.Email) && p.Email.ToUpper().Contains(capSearchTerm)));
            }

            searchResults.Distinct();
        }

        private void MenuUpdateHome_Click(object sender, RoutedEventArgs e)
        {
            //  TODO: Complete Upate Home workflow.
            //  Home must already exist
            //  Menu -> Search -> Home, highlight Home in results, then Menu -> Update -> Home
            HomeSearchView selectedHome = null;
            selectedHome = FoundHomesView.SelectedItem as HomeSearchView;
            FoundHomesView.SelectedItem = -1;

            DisplayStatusMessage("NOT IMPLEMENTED");
            ClearSearchResultsViews();
        }

        /// <summary>
        /// Update selected HomeForSale. Requires: HomeForSale instance as a Collection of a Home, a RE Company, and of an Agent, at a minimum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUpdateHomeForSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder statusMessage = new StringBuilder("Ok. ");
                HomeForSaleView selectedHomesaleView = null;
                selectedHomesaleView = FoundHomesForSaleView.SelectedItem as HomeForSaleView;
                FoundHomesForSaleView.SelectedIndex = -1;
                var homeid = selectedHomesaleView.HomeID;
                HomeSale hfsHomesale = homeSalesCollection.Where(hs => hs.HomeID == homeid && hs.MarketDate == selectedHomesaleView.MarketDate)
                                                                   .FirstOrDefault();

                Home hfsHome = new Home();
                hfsHome = homesCollection.Where(h => h.HomeID == hfsHomesale.HomeID).FirstOrDefault();
                if (hfsHome != null)
                {
                    Person hfsBuyer = new Person();
                    hfsBuyer = peopleCollection.Where(p => p.PersonID == hfsHomesale.BuyerID).FirstOrDefault();
                    Person hfsAgent = new Person();
                    hfsAgent = peopleCollection.Where(p => p.Agent.AgentID == hfsHomesale.AgentID).FirstOrDefault();
                    if (hfsAgent != null)
                    {
                        RealEstateCompany hfsReco = new RealEstateCompany();
                        hfsReco = reCosCollection.Where(r => r.CompanyID == hfsAgent.Agent.CompanyID).FirstOrDefault();
                        if (hfsReco != null)
                        {
                            HomeUpdaterWindow huw = new HomeUpdaterWindow();
                            huw.UpdateType = "HomeSale";
                            huw.UpdateHomeSale = hfsHomesale;
                            huw.UpdateAgent = hfsAgent.Agent;
                            huw.UpdateBuyer = hfsBuyer.Buyer;
                            huw.UpdateHome = hfsHome;
                            huw.UpdateReco = hfsReco;
                            DisplayStatusMessage("Loading update window");
                            huw.Show();
                        }
                        else
                        {
                            statusMessage.Append("RE Company not associated with this Home For Sale record. ");
                        }

                    }
                    else
                    {
                        statusMessage.Append($"Agent not associated with this Home For Sale record. ");
                    }

                }
                else
                {
                    statusMessage.Append($"DB Data problem: No Home found for this Home For Sale record. ");
                }

                if (statusMessage.Length > 4)
                {
                    DisplayStatusMessage(statusMessage.ToString());
                }
            }
            catch
            {
                logger.Data("Main Window MenuUpdateHomeForSale", "You must select a Home For Sale in order to Update it. Did you mean to Add a Home For Sale instead?");
                logger.Flush();
                DisplayStatusMessage("You must select a Home For Sale in order to Update it. Did you mean to Add a Home For Sale instead?");
            }
        }

        /// <summary>
        /// Take a selected item from Search Results and open a the Person Updater Window to make changes to the selected Person Type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUpdateAgent_Click(object sender, RoutedEventArgs e)
        {
            var updatePerson = new Person();
            var updateAgent = new Agent();
            PersonView selectedPerson = FoundPeopleView.SelectedItem as PersonView;
            updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
            try
            {
                if (selectedPerson != null)
                {
                    if (updatePerson != null)
                    {
                        updateAgent = updatePerson.Agent;
                        var puw = new PersonUpdaterWindow();
                        puw.CalledByUpdateMenu = true;
                        puw.CalledByUpdateMenuType = "Agent";
                        puw.ReceivedPerson = updatePerson;
                        puw.ReceivedAgent = updateAgent;
                        puw.Show();
                    }
                }
            }
            catch
            {
                DisplayStatusMessage("Unable to load Buyer Update Window. Refresh, search again, and select a Person in the results.");
            }
        }

        private void MenuUpdateBuyer_Click(object sender, RoutedEventArgs e)
        {
            var updatePerson = new Person();
            var updateBuyer = new Buyer();
            PersonView selectedPerson = FoundPeopleView.SelectedItem as PersonView;
            updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
            try
            {
                if (updatePerson.Buyer != null)
                {
                    updateBuyer = updatePerson.Buyer;
                }

                if (selectedPerson != null)
                {
                    if (updatePerson != null)
                    {
                        var puw = new PersonUpdaterWindow();
                        puw.CalledByUpdateMenu = true;
                        puw.CalledByUpdateMenuType = "Buyer";
                        puw.ReceivedPerson = updatePerson;
                        puw.ReceivedBuyer = updateBuyer;
                        puw.Show();
                    }
                }
            }
            catch
            {
                DisplayStatusMessage("Unable to load Buyer Update Window. Refresh, search again, and select a Person in the results.");
            }

        }

        private void menuUpdateOwner_Click(object sender, RoutedEventArgs e)
        {
            var updatePerson = new Person();
            var updateOwner = new Owner();
            var updateAgent = new Agent();
            var updateBuyer = new Buyer();
            PersonView selectedPerson = FoundPeopleView.SelectedItem as PersonView;
            updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
            try
            {
                if (updatePerson.Owner != null)
                {
                    updateOwner = updatePerson.Owner;
                }

                if (selectedPerson != null)
                    {
                    if (updatePerson != null)
                    { 
                        var puw = new PersonUpdaterWindow();
                        puw.CalledByUpdateMenu = true;
                        puw.CalledByUpdateMenuType = "Owner";
                        puw.ReceivedPerson = updatePerson;
                        puw.ReceivedOwner = updateOwner;
                        puw.Show();
                    }
                }
            }
            catch
            {
                DisplayStatusMessage("Unable to load Owner Update Window. Refresh, search again, and then select a Person in the results.");
            }
        }

        private void MenuSearchSoldHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            FoundSoldHomesView.Visibility = Visibility.Visible;
        }

        private void DisplayPeopleSearchResults()
        {
            var searchResults = new List<Person>();
            var searchTerms = new List<String>();
            string searchTermsText = searchTermsTextbox.Text;
            string[] searchTermsArr = searchTermsText.Split(',');
            searchTerms = searchTermsArr.Where(st => st.Length > 0 && string.IsNullOrWhiteSpace(st) == false).ToList();

            if (searchTerms.Count > 0)
            {
                PersonSearchHelper(ref searchResults, ref searchTerms);
            }

            if (searchResults.Count > 0)
            {
                var viewResults = new List<PersonView>();
                searchResults = searchResults.Distinct<Person>().ToList();
                
                PersonView newPersonView = null;
                foreach (var person in searchResults)
                {
                    if (person.Agent == null && person.Buyer == null && person.Owner == null)
                    {
                        newPersonView = new PersonView()
                        {
                            PersonID = person.PersonID,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            Phone = person.Phone,
                            Email = person.Email ?? null,
                            PersonType = string.Empty
                        };
                        viewResults.Add(newPersonView);
                    }
                    else
                    {
                        if (person.Agent != null)
                        {
                            newPersonView = new PersonView()
                            {
                                PersonID = person.PersonID,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null,
                                PersonType = person.Agent.GetType().Name
                            };
                            viewResults.Add(newPersonView);
                        }
                        if (person.Buyer != null)
                        {
                            newPersonView = new PersonView()
                            {
                                PersonID = person.PersonID,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null,
                                PersonType = person.Buyer.GetType().Name
                            };
                            viewResults.Add(newPersonView);
                        }
                        if (person.Owner != null)
                        {
                            newPersonView = new PersonView()
                            {
                                PersonID = person.PersonID,
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                Phone = person.Phone,
                                Email = person.Email ?? null,
                                PersonType = person.Owner.GetType().Name
                            };
                            viewResults.Add(newPersonView);
                        }
                    }
                }

                FoundPeopleView.ItemsSource = viewResults;
                FoundPeopleView.Visibility = Visibility.Visible;
                DisplayStatusMessage($"Found { viewResults.Count } People. Select a result and use Details or Modify to make changes.");
            }

            if (searchTerms.Count < 1 || searchResults.Count < 1)
            {
                DisplayZeroResultsMessage();
                ClearSearchResultsViews();
            }

        }

        /// <summary>
        /// Opens Home Updater Window to edit a selected Home to put it up for sale or change other For Sale properties.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MenuAddHomesForSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedHome = FoundHomesView.SelectedItem as HomeSearchView;
                Home hfsHome = homesCollection.Where(h => h.HomeID == selectedHome.HomeID).FirstOrDefault();
                List<HomeSale> hfsHomesales = homeSalesCollection.Where(hs => hs.HomeID == hfsHome.HomeID).ToList();
                hfsHome.HomeSales = hfsHomesales;

                var huw = new HomeUpdaterWindow();
                huw.UpdateType = "HomeSale";
                huw.UpdateAgent = new Agent();
                huw.UpdatePerson = new Person();
                huw.UpdateHome = hfsHome;
                huw.UpdateHomeSale = new HomeSale();
                huw.UpdateReco = new RealEstateCompany();
                huw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home that is not already for Sale then click Menu Add Home For Sale.");
                logger.Data("MenuAddHomesForSale Exception", ex.Message);
                logger.Flush();
            }
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

        private void MenuSearchPeople_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            DisplayPeopleSearchResults();
        }

        /// <summary>
        /// Throws an error for testing Exception handling and logging.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuThrowError_Click(object sender, RoutedEventArgs e)
        {
            int zero = 0;
            int number = 2 / zero;
        }
    }
}
