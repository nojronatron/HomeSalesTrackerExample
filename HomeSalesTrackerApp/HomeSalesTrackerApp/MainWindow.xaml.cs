using HomeSalesTrackerApp.CrudWindows;
using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using HomeSalesTrackerApp.ReportWindows;
using HSTDataLayer;
using HSTDataLayer.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

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

        private void MenuSearchSoldHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            var soldHomes = (from hs in homeSalesCollection
                             where hs.SoldDate != null
                             select hs).ToList();

            var searchHomesalesResults = new List<HomeSale>();
            var searchHomesResults = new List<Home>();
            var searchTerms = new List<String>();
            string searchTermsText = searchTermsTextbox.Text;
            searchTerms = FormatSearchTerms(searchTermsText);
            var shvResults = new List<SoldHomesView>();

            if (searchTerms.Count > 0)
            {
                HomesalesSearchHelper(ref searchHomesalesResults, ref searchTerms);
                searchHomesalesResults = searchHomesalesResults.Distinct().ToList();
                shvResults = (from hs in homeSalesCollection
                              from shsr in searchHomesalesResults
                              from h in homesCollection
                              from sh in soldHomes
                              where hs.SaleID == sh.SaleID
                              && hs.SaleID == shsr.SaleID
                              && h.HomeID == shsr.HomeID
                              && shsr.SoldDate != null
                              select new SoldHomesView
                              {
                                  HomeID = hs.HomeID,
                                  Address = h.Address,
                                  City = h.City,
                                  State = h.State,
                                  Zip = h.Zip,
                                  SaleAmount = hs.SaleAmount,
                                  SoldDate = hs.SoldDate
                              })
                              .ToList();

                HomeSearchHelper(ref searchHomesResults, ref searchTerms);
                searchHomesResults = searchHomesResults.Distinct().ToList();
                shvResults.AddRange((from hs in homeSalesCollection
                                     from shr in searchHomesResults
                                     from h in homesCollection
                                     from sh in soldHomes
                                     where h.HomeID == sh.HomeID
                                     && h.HomeID == shr.HomeID
                                     && h.HomeID == hs.HomeID
                                     && hs.SoldDate != null
                                     select new SoldHomesView
                                     {
                                         HomeID = hs.HomeID,
                                         Address = h.Address,
                                         City = h.City,
                                         State = h.State,
                                         Zip = h.Zip,
                                         SaleAmount = hs.SaleAmount,
                                         SoldDate = hs.SoldDate
                                     })
                                     .ToList());
            }

            if (shvResults.Count > 0)
            {
                FoundSoldHomesView.Visibility = Visibility.Visible;
                FoundSoldHomesView.ItemsSource = shvResults;
                DisplayStatusMessage($"Found { searchHomesalesResults.Count } Sold Homes. Select a result and use Update or Remove menus to make changes.");
            }

            if (searchTerms.Count < 1 || shvResults.Count < 1)
            {
                DisplayZeroResultsMessage();
            }

        }

        private void HomesalesSearchHelper(ref List<HomeSale> searchHomesalesResults, ref List<String> searchTerms)
        {
            foreach (var searchTerm in searchTerms)
            {
                string capSearchTerms = searchTerm.ToUpper().Trim();
                searchHomesalesResults.AddRange(homeSalesCollection.OfType<HomeSale>().Where(hs => hs.SaleID.ToString().Contains(searchTerm)));
                searchHomesalesResults.AddRange(homeSalesCollection.OfType<HomeSale>().Where(hs => hs.MarketDate.ToString().Contains(searchTerm)));
                searchHomesalesResults.AddRange(homeSalesCollection.OfType<HomeSale>().Where(hs => hs.SaleAmount.ToString().Contains(searchTerm)));
                searchHomesalesResults.AddRange(homeSalesCollection.OfType<HomeSale>().Where(hs => hs.SoldDate.ToString().Contains(searchTerm)));
            }

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
            searchTerms = FormatSearchTerms(searchTermsText);
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
            searchTerms = FormatSearchTerms(searchTermsText);
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

            InitializeCollections();
        }

        /// <summary>
        /// Selected Home For Sale is taken off the Market.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuRemoveHomeFromMarket_Click(object sender, RoutedEventArgs e)
        {
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
            var ahw = new AddHomeWindow();
            ahw.AddType = "Home";
            ahw.Title = "Add New Home";
            ahw.Show();
            ClearSearchResultsViews();
        }

        /// <summary>
        /// Add a new Owner instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddOwner_Click(object sender, RoutedEventArgs e)
        {
            var apw = new AddPersonWindow();
            apw.AddType = "Owner";
            apw.Title = "Add New Owner Person";
            apw.Show();
            ClearSearchResultsViews();
        }

        /// <summary>
        /// Add a new Agent instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddAgent_Click(object sender, RoutedEventArgs e)
        {
            var apw = new AddPersonWindow();
            apw.AddType = "Agent";
            apw.Title = "Add New Agent Person";
            apw.Show();
            ClearSearchResultsViews();
        }

        /// <summary>
        /// Add a new Buyer instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddBuyer_Click(object sender, RoutedEventArgs e)
        {
            var apw = new AddPersonWindow();
            apw.AddType = "Buyer";
            apw.Title = "Add New Buyer Person";
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
            var soldHomesReport = new SoldHomesReport();
            soldHomesReport.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready");
        }

        private void MenuDisplayBuyers_Click(object sender, RoutedEventArgs e)
        {
            var buyersResultsReport = new BuyersResultsReport();
            buyersResultsReport.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready");        
        }

        private void menuDisplayAgents_Click(object sender, RoutedEventArgs e)
        {
            var agentsResultsReport = new AgentsResultsReport();
            agentsResultsReport.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready");
        }

        private void menuDisplayRECoTotals_Click(object sender, RoutedEventArgs e)
        {
            var realEstateCoReport = new RealEstateCoReport();
            realEstateCoReport.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready");
        }

        private void menuDisplayHomesForSale_Click(object sender, RoutedEventArgs e)
        {
            var homesForSaleReport = new HomesForSaleReport();
            homesForSaleReport.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("Ready");
        }

        private void MenuAboutAppInfo_Click(object sender, RoutedEventArgs e)
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
            try
            {
                HomeSearchView selectedHome = FoundHomesView.SelectedItem as HomeSearchView;
                Home home = homesCollection.Where(h => h.HomeID == selectedHome.HomeID).FirstOrDefault();
                home.HomeSales = homeSalesCollection.Where(hs => hs.HomeID == home.HomeID).ToList();
                home.Owner = peopleCollection.Where(o => o.PersonID == home.OwnerID).FirstOrDefault().Owner;
                var ahw = new AddHomeWindow();
                ahw.NewHome = home;
                ahw.AnOwner = home.Owner;
                ahw.APerson = peopleCollection.Where(p => p.PersonID == home.Owner.OwnerID).FirstOrDefault();
                ahw.UpdateInsteadOfAdd = true;
                ahw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home first, then click Menu, Update Home.");
                logger.Data("MenuUpdateHome Exception", ex.Message);
                logger.Flush();
            }
            DisplayStatusMessage("!!TESTING!! Menu -> Update Home !!TESTING!!");
            ClearSearchResultsViews();
        }

        /// <summary>
        /// Opens Home Updater Window to edit a selected Home to put it up for sale or change other For Sale properties.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MenuAddHomeForSale_Click(object sender, RoutedEventArgs e)
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
                huw.Title = "Add Home: For Sale";
                huw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home that is not already for Sale then click Menu Add Home For Sale.");
                logger.Data("MenuAddHomesForSale Exception", ex.Message);
                logger.Flush();
            }
        }

        /// <summary>
        /// Take a selected item from Search Results and open a the Person Updater Window to make changes to the selected Person Type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUpdateAgent_Click(object sender, RoutedEventArgs e)
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
                        puw.Title = "Update Person's Agent Info";
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
                        puw.Title = "Update Person's Buyer Info";
                        puw.Show();
                    }
                }
            }
            catch
            {
                DisplayStatusMessage("Unable to load Buyer Update Window. Refresh, search again, and select a Person in the results.");
            }

        }

        private void MenuUpdateOwner_Click(object sender, RoutedEventArgs e)
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
                        puw.Title = "Update Person's Owner Info";
                        puw.Show();
                    }
                }
            }
            catch
            {
                DisplayStatusMessage("Unable to load Owner Update Window. Refresh, search again, and then select a Person in the results.");
            }
        }

        private void DisplayPeopleSearchResults()
        {
            var searchResults = new List<Person>();
            var searchTerms = new List<string>();
            string searchTermsText = searchTermsTextbox.Text;
            searchTerms = FormatSearchTerms(searchTermsText);
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
