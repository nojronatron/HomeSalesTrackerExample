using HomeSalesTrackerApp.CrudWindows;
using HomeSalesTrackerApp.DisplayModels;
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
            if (App.DatabaseLoadCompleted)
            {
                InitializeCollections();
                DisplayStatusMessage("Database data loaded.");
            };
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
        /// Use the Search textbox and UpdateHomeAsSold Menu Item to search for and update a HomeSale, Home, and OwnerID.
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

            //  lots of opportunity for data to be missing or incorrect for this opereration so track error messages for return when canceling out
            StringBuilder statusMessage = new StringBuilder("Ok. ");

            //  take a selected HomeForSale item and use it to pre-load the UpdaterWindow Sold HomeForSale panel
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

                //  Agent must have a CompanyID
                if (hfsAgent != null && hfsAgent.Agent.CompanyID != null)
                {
                    RealEstateCompany hfsReco = new RealEstateCompany();
                    hfsReco = reCosCollection.Where(r => r.CompanyID == hfsAgent.Agent.CompanyID).FirstOrDefault();
                    if (hfsReco != null)
                    {
                        //  pass selected HomeForSale to UpdaterWindow 
                        HomeUpdaterWindow uw = new HomeUpdaterWindow();
                        uw.UpdateType = "HomeSold";
                        uw.UpdateHomeSale = hfsHomesale;
                        uw.UpdateAgent = hfsAgent.Agent;
                        uw.UpdateAgentPerson = hfsAgent;
                        uw.UpdateBuyerPerson = new Person();
                        uw.UpdateBuyer = new Buyer();
                        uw.UpdateHome = hfsHome;
                        uw.UpdateReco = hfsReco;
                        DisplayStatusMessage("Loading update window");
                        uw.Show();

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


        private void MenuDisplaySoldHomes_Click(object sender, RoutedEventArgs e)
        {
            //  Display Sold Homes
            //  HomeID, Address, City, State, Zip
            //  Buyer First & Last
            //  Agent First & Last
            //  Real Estate Co (name)
            //  Sale Amount, Sale Date
            //  Do NOT include Homes without a Sold Date

            var soldHomesQuery = (from hs in homeSalesCollection
                                  where hs.SoldDate != null
                                  select hs).ToList();

            var shResultsList = new List<SoldHomesView>();
            SoldHomesView shv = null;
            foreach(var soldHome in soldHomesQuery)
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

            //var foundSoldHomes = (from hs in homeSalesCollection
            //                      from h in homesCollection
            //                      from a in peopleCollection
            //                      from b in peopleCollection
            //                      from re in reCosCollection
            //                      where hs.SoldDate != null &&
            //                      h.HomeID == hs.HomeID &&
            //                      re.CompanyID == hs.CompanyID &&
            //                      (a.PersonID == hs.AgentID ||
            //                      b.PersonID == hs.BuyerID)
            //                      select new SoldHomesView
            //                      {
            //                          HomeID = h.HomeID,
            //                          Address = h.Address,
            //                          City = h.City,
            //                          State = h.State,
            //                          Zip = h.Zip,
            //                          BuyerFirstLastName = $"{ b.FirstName } { b.LastName }" ?? string.Empty,
            //                          AgentFirstLastName = $"{ a.FirstName } { a.LastName }" ?? string.Empty,
            //                          RealEstateCompanyName = re.CompanyName,
            //                          SaleAmount = hs.SaleAmount,
            //                          SaleDate = hs.SoldDate
            //                      });

            //var foundSoldHomes = shAgentsList;
            //foundSoldHomes.Concat(shBuyersList);

            SoldHomesReport shr = new SoldHomesReport();
            shr.iFoundSoldHomes = shResultsList;
            shr.Show();
            ClearSearchResultsViews();
            DisplayStatusMessage("ReadY");
        }

        private void MenuDisplayBuyers_Click(object sender, RoutedEventArgs e)
        {
            //  Display all Buyers: personid, fname, lname, phone, email, preferred lender
            var foundBuyers = (from p in peopleCollection
                               from b in homeSalesCollection
                               where b.BuyerID == p.PersonID
                               select new BuyerView
                               {
                                    PersonID = p.PersonID,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,
                                    Phone = p.Phone,
                                    Email = p.Email ?? null,
                                    CreditRating = b.Buyer.CreditRating ?? 0
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
            //  Placeholder Window_Closing method. There could be actions to take here
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

            searchResults.Distinct();
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
            //  Home must already exist
            //  Menu -> Search -> Home, highlight Home in results, then Menu -> Update -> Home
            HomeSearchView selectedHome = null;
            selectedHome = FoundHomesView.SelectedItem as HomeSearchView;
            FoundHomesView.SelectedItem = -1;

        }

        private void MenuUpdateHomeForSale_Click(object sender, RoutedEventArgs e)
        {
            //    alternate code:
            /*
            HomeForSaleView selectedHomesaleView = null;
            selectedHomesaleView = FoundHomesForSaleView.SelectedItem as HomeForSaleView;
            FoundHomesForSaleView.SelectedItem = -1;    //  clear the selection on the combobox

            var homeid = selectedHomesaleView.HomeID;
            HomeSale homesaleByID = homeSalesCollection.Where(hs => hs.HomeID == homeid && hs.MarketDate == selectedHomesaleView.MarketDate)
                                                                  .FirstOrDefault();
            Agent homesaleAgent = new Agent()
            {
                AgentID = homesaleByID.AgentID,
                CompanyID = homesaleByID.CompanyID,
                CommissionPercent = homesaleByID.Agent.CommissionPercent
            };

            Person agentPerson = MainWindow.peopleCollection.Where(p => p.PersonID == homesaleByID.AgentID).FirstOrDefault();

            PersonUpdaterWindow personUpdaterWindow = new PersonUpdaterWindow();
            personUpdaterWindow.CalledByUpdateMenuType = "Agent";
            personUpdaterWindow.CalledByUpdateMenu = false;
            personUpdaterWindow.UpdateAgent = homesaleAgent;
            personUpdaterWindow.UpdatePerson = agentPerson;
            personUpdaterWindow.Show();
            */

            //  lots of opportunity for data to be missing or incorrect for this opereration so track error messages for return when canceling out
            StringBuilder statusMessage = new StringBuilder("Ok. ");

            //  take a selected HomeForSale item and 
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
                if (hfsBuyer != null)
                {
                    Person hfsAgent = new Person();
                    hfsAgent = peopleCollection.Where(p => p.Agent.AgentID == hfsHomesale.AgentID).FirstOrDefault();

                    //  Agent must have a CompanyID
                    if (hfsAgent != null && hfsAgent.Agent.CompanyID != null)
                    {
                        RealEstateCompany hfsReco = new RealEstateCompany();
                        hfsReco = reCosCollection.Where(r => r.CompanyID == hfsAgent.Agent.CompanyID).FirstOrDefault();
                        if (hfsReco != null)
                        {
                            //  pass selected HomeForSale to UpdaterWindow 
                            HomeUpdaterWindow uw = new HomeUpdaterWindow();
                            uw.UpdateType = "HomeSale";
                            uw.UpdateHomeSale = hfsHomesale;
                            uw.UpdateAgent = hfsAgent.Agent;
                            uw.UpdateAgentPerson = hfsAgent;
                            uw.UpdateBuyer = hfsBuyer.Buyer;
                            uw.UpdateBuyerPerson = hfsBuyer;
                            uw.UpdateHome = hfsHome;
                            uw.UpdateReco = hfsReco;
                            DisplayStatusMessage("Loading update window");
                            uw.Show();

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
                    statusMessage.Append($"Buyer record not found. ");
                }
            }
            statusMessage.Append($"DB Data problem: No Home found for this Sale record. ");
            if(statusMessage.Length > 4)
            {
                DisplayStatusMessage(statusMessage.ToString());
            }
        }

        private void menuUpdateOwner_Click(object sender, RoutedEventArgs e)
        {
            PersonView selectedperson = FoundPeopleView.SelectedItem as PersonView;
            string statusMessage = "Select a search result prior to using the Update Menu.";
            Person updatePerson = null;
            Owner updateOwner = null;

            if (selectedperson != null)
            {
                updatePerson = peopleCollection.Where(p => p.PersonID == selectedperson.PersonID).FirstOrDefault();
                if (updatePerson != null)
                {
                    updateOwner = updatePerson.Owner;
                    statusMessage = $"{ updatePerson.GetFirstAndLastName() } selected.";
                    var puw = new PersonUpdaterWindow();
                    puw.CalledByUpdateMenu = true;
                    puw.CalledByUpdateMenuType = "Owner";
                    puw.ReceivedPerson = updatePerson;
                    puw.ReceivedOwner = updateOwner;
                    puw.Show();
                }
            }
            DisplayStatusMessage(statusMessage);

        }

        /// <summary>
        /// Take a selected item from Search Results and open a the Person Updater Window to make changes to the selected Person Type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUpdateAgent_Click(object sender, RoutedEventArgs e)
        {
            PersonView selectedPerson = FoundPeopleView.SelectedItem as PersonView;
            string statusMessage = "Select a search result prior to using the Update Menu.";
            Person updatePerson = null;
            Agent updateAgent = null;

            if (selectedPerson != null)
            {
                updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
                if (updatePerson != null)
                {
                    updateAgent = updatePerson.Agent;
                    statusMessage = $"{ updatePerson.GetFirstAndLastName() } selected.";
                    var puw = new PersonUpdaterWindow();
                    puw.CalledByUpdateMenu = true;
                    puw.CalledByUpdateMenuType = "Agent";
                    puw.ReceivedPerson = updatePerson;
                    puw.ReceivedAgent = updateAgent;
                    puw.Show();
                }
            }
            DisplayStatusMessage(statusMessage);

        }

        private void MenuUpdateBuyer_Click(object sender, RoutedEventArgs e)
        {
            PersonView selectedPerson = FoundPeopleView.SelectedItem as PersonView;
            string statusMessage = "Select a search result prior to using the Update Menu.";
            Person updatePerson = null;
            Buyer updateBuyer = null;

            if (selectedPerson != null)
            {
                updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
                if (updatePerson != null)
                {
                    updateBuyer = updatePerson.Buyer;
                    statusMessage = $"{ updatePerson.GetFirstAndLastName() } selected.";
                    var puw = new PersonUpdaterWindow();
                    puw.CalledByUpdateMenu = true;
                    puw.CalledByUpdateMenuType = "Buyer";
                    puw.ReceivedPerson = updatePerson;
                    puw.ReceivedBuyer = updateBuyer;
                    puw.Show();
                }
            }
            DisplayStatusMessage(statusMessage);

        }

        private void MenuSearchSoldHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            FoundSoldHomesView.Visibility = Visibility.Visible;
        }

        private void DisplayPeopleSearchResults()
        {
            ClearSearchResultsViews();
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
                //  deliver results to the screen
                var viewResults = new List<PersonView>();
                List<Person> resultSet = (from p in searchResults
                                          select p).ToList();

                PersonView newPersonView = null;
                foreach (var person in resultSet)
                {
                    newPersonView = new PersonView()
                    {
                        PersonID = person.PersonID,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Phone = person.Phone,
                        Email = person.Email ?? null
                    };

                    if(person.Agent != null)
                    {
                        newPersonView.PersonType = person.Agent.GetType().Name;
                    }
                    if(person.Buyer != null)
                    {
                        newPersonView.PersonType = person.Buyer.GetType().Name;
                    }
                    if (person.Owner != null)
                    {
                        newPersonView.PersonType = person.Owner.GetType().Name;
                    }

                    viewResults.Add(newPersonView);
                }

                //var listResults = results.ToList();
                FoundPeopleView.ItemsSource = viewResults;
                FoundPeopleView.Visibility = Visibility.Visible;
                DisplayStatusMessage($"Found { viewResults.Count } People. Select a result and use Details or Modify to make changes.");
            }

            if (searchTerms.Count < 1 || searchResults.Count < 1)
            {
                DisplayZeroResultsMessage();
            }

        }

        /// <summary>
        /// DO NOT CALL THIS TEMPLATE METHOD.
        /// </summary>
        private void SearchBuyers() 
        { 
            ClearSearchResultsViews();
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
                //  deliver results to the screen
                var results = (from p in searchResults
                               from b in homeSalesCollection
                               where p != null && b.BuyerID == p.PersonID
                               select new BuyerView
                               {
                                   BuyerID = p.PersonID,
                                   FirstName = p.FirstName,
                                   LastName = p.LastName,
                                   Phone = p.Phone,
                                   Email = p.Email,
                                   CreditRating = p.Buyer.CreditRating,
                                   PersonType = "Buyer"
                               });

                var listResults = results.ToList();
                FoundPeopleView.ItemsSource = listResults;
                FoundPeopleView.Visibility = Visibility.Visible;
                DisplayStatusMessage($"Found { listResults.Count } Buyers. Select a result and use Update or Remove menus to make changes.");
            }

            if (searchTerms.Count < 1 || searchResults.Count < 1)
            {
                DisplayZeroResultsMessage();
            }

        }

        private void MenuAddHomesForSale_Click(object sender, RoutedEventArgs e)
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

        private void MenuSearchPeople_Click(object sender, RoutedEventArgs e)
        {
            DisplayPeopleSearchResults();
        }
    }
}
