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
                GetItemDetailsButton.IsEnabled = false;
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

        private static void InitializeCollections()
        {
            InitHomeSalesCollection();
            InitPeopleCollection();
            InitHomesCollection();
            InitRealEstateCompaniesCollection();
        }

        private static void InitRealEstateCompaniesCollection()
        {
            List<RealEstateCompany> recos = EntityLists.GetTreeListOfRECompanies();
            reCosCollection = new RealEstateCosCollection(recos);

        }

        private static void InitHomesCollection()
        {
            List<Home> homes = EntityLists.GetTreeListOfHomes();
            homesCollection = new HomesCollection(homes);

        }

        private static void InitPeopleCollection()
        {
            List<Person> people = EntityLists.GetListOfPeople();
            peopleCollection = new PeopleCollection<Person>(people);

        }

        private static void InitHomeSalesCollection()
        {
            List<HomeSale> homeSales = EntityLists.GetListOfHomeSales();
            homeSalesCollection = new HomeSalesCollection(homeSales);

        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchTermsTextbox();
            ClearSearchResultsViews();
            GetItemDetailsButton.IsEnabled = false;
            DisplayStatusMessage("Ready.");
        }

        private void ClearSearchTermsTextbox()
        {
            searchTermsTextbox.Text = string.Empty;
            GetItemDetailsButton.IsEnabled = false;
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
            GetItemDetailsButton.IsEnabled = false;
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
            var foundSoldHomes = HomeSalesSearchTool.GetSoldHomes(searchTermsTextbox.Text);

            if (foundSoldHomes.Count < 1)
            {
                DisplayZeroResultsMessage();
            }

            FoundSoldHomesView.ItemsSource = foundSoldHomes;
            FoundSoldHomesView.Visibility = Visibility.Visible;
            GetItemDetailsButton.IsEnabled = true;
            DisplayStatusMessage($"Found { foundSoldHomes.Count } Sold Homes. Select an item to Menu > Update or Get Item Details.");

        }

        /// <summary>
        /// Search for existing Homes using user provided search terms in the Search textbox. Return results to customer ListView with headers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSearchHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            string searchTermsText = searchTermsTextbox.Text;
            var searchTerms = FormatSearchTerms.FormatTerms(searchTermsText);

            if (searchTerms.Count < 1)
            {
                DisplayZeroResultsMessage();
                return;
            }

            var homeSearchTool = new HomeSearchTool(searchTerms);
            var listResults = homeSearchTool.SearchResults;

            if (listResults == null)
            {
                DisplayZeroResultsMessage();
                return;
            }

            FoundHomesView.ItemsSource = listResults;
            FoundHomesView.Visibility = Visibility.Visible;
            GetItemDetailsButton.IsEnabled = true;
            DisplayStatusMessage($"Found { listResults.Count } Homes. Select an entry and click Get Item Details button for more information.");

        }

        /// <summary>
        /// Uses search terms in Search textbox to find Homes For Sale and returns results to custom Search Results ListView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuSearchHomesForSale_Click(Object sender, RoutedEventArgs args)
        {
            ClearSearchResultsViews();
            var foundHomesForSale = HomeSalesSearchTool.GetHomesOnMarket(searchTermsTextbox.Text);

            if (foundHomesForSale.Count < 1)
            {
                DisplayZeroResultsMessage();
                return;
            }

            FoundHomesForSaleView.ItemsSource = foundHomesForSale;
            FoundHomesForSaleView.Visibility = Visibility.Visible;
            GetItemDetailsButton.IsEnabled = true;
            DisplayStatusMessage($"Found { foundHomesForSale.Count } Homes For Sale. Select an entry and click Get Item Details button for more information.");

        }

        /// <summary>
        /// Use the Search textbox and UpdateHomeAsSold Menu Item to search for and update a HomeSale, Home, and OwnerID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUpdateHomeAsSold_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder statusMessage = new StringBuilder("Ok. ");

            int homeID = -1;

            if (FoundHomesForSaleView.IsVisible)
            {
                HomeForSaleModel selectedHomesaleView = FoundHomesForSaleView.SelectedItem as HomeForSaleModel;
                homeID = selectedHomesaleView.HomeID;
            }
            else if (FoundHomesView.IsVisible)
            {
                HomeSearchModel selectedHomeSearchModel = FoundHomesView.SelectedItem as HomeSearchModel;
                homeID = selectedHomeSearchModel.HomeID;
            }
            else
            {
                DisplayStatusMessage("Select an item from the results and try again.");
                return;
            }

            HomeSale hfsHomesale = homeSalesCollection.Where(hs => hs.HomeID == homeID &&
                                                                   hs.MarketDate != null &&
                                                                   hs.SoldDate == null).FirstOrDefault();

            Home hfsHome = homesCollection.Where(h => h.HomeID == homeID).FirstOrDefault();

            if (hfsHome != null && hfsHomesale != null)
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
                        homeUpdaterWindow.UpdateType = "HOMESOLD";
                        homeUpdaterWindow.UpdatePerson = hfsAgent;
                        homeUpdaterWindow.UpdateAgent = hfsAgent.Agent;
                        homeUpdaterWindow.UpdateHome = hfsHome;
                        homeUpdaterWindow.UpdateHomeSale = hfsHomesale;
                        homeUpdaterWindow.UpdateReco = hfsReco;
                        DisplayStatusMessage("Loading update window");
                        homeUpdaterWindow.Show();
                        ClearSearchResultsViews();
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

            //InitializeCollections();
        }

        /// <summary>
        /// Selected Home For Sale is taken off the Market.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuRemoveHomeFromMarket_Click(object sender, RoutedEventArgs e)
        {
            HomeForSaleModel selectedHomeForSale = (HomeForSaleModel)this.FoundHomesForSaleView.SelectedItem;
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
                    //InitializeCollections();

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
            //  testing forced disposal/GC
            using (AgentsResultsReport agentsResultsReport = new AgentsResultsReport())
            {
                agentsResultsReport.Show();
            };
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
            string messageBoxText = $"App: Home Sales Tracker\nAuthor: Jon Rumsey\nSummer, Fall 2020";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, messageBoxCaption, button, icon);
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
            var objectType = new StringBuilder();
            var objectContents = new StringBuilder();

            try
            {
                if (FoundHomesView.Visibility == Visibility.Visible)
                {
                    HomeSearchModel selectedHome = FoundHomesView.SelectedItem as HomeSearchModel;

                    if (selectedHome == null)
                    {
                        DisplayStatusMessage("Select an item in the results before clicking the button.");
                        logger.Data("Get Item Details button", "No item selected in UI before clicking GID button.");
                        logger.Flush();
                        return;
                    }

                    objectType.Append(selectedHome.GetType().ToString());
                    objectContents.Append(selectedHome.ToString());

                    var homeMarketDate = (from hfs in homeSalesCollection
                                          where hfs.HomeID == selectedHome.HomeID &&
                                          hfs.SoldDate == null
                                          select hfs).FirstOrDefault();

                    if (homeMarketDate != null) //  devnote: skip adding a date if there is not one to avoid setting a null
                    {
                        selectedHome.MarketDate = homeMarketDate.MarketDate;
                    }

                    MessageBox.Show($"{ selectedHome.ToStackedString() }");
                    FoundHomesView.SelectedIndex = -1;
                }

                if (FoundHomesForSaleView.Visibility == Visibility.Visible)
                {
                    HomeForSaleModel selectedHfs = FoundHomesForSaleView.SelectedItem as HomeForSaleModel;

                    if (selectedHfs == null)
                    {
                        DisplayStatusMessage("Select an item in the results before clicking the button.");
                        logger.Data("Get Item Details button", "No item selected in UI before clicking GID button.");
                        logger.Flush();
                        return;
                    }

                    objectType.Append(selectedHfs.GetType().ToString());
                    objectContents.Append(selectedHfs.ToString());

                    var homeForSaleDetails = new HomeForSaleDetailModel();
                    homeForSaleDetails = (from hfs in homeSalesCollection
                                          where hfs.HomeID == selectedHfs.HomeID
                                          join h in homesCollection on hfs.HomeID equals h.HomeID
                                          join reco in reCosCollection on hfs.CompanyID equals reco.CompanyID
                                          join o in peopleCollection on h.OwnerID equals o.PersonID
                                          join a in peopleCollection on hfs.AgentID equals a.PersonID
                                          select new HomeForSaleDetailModel
                                          {
                                              HomeID = h.HomeID,
                                              Address = h.Address,
                                              City = h.City,
                                              State = h.State,
                                              Zip = h.Zip,
                                              SaleAmount = hfs.SaleAmount,
                                              MarketDate = hfs.MarketDate,
                                              OwnerFirstName = o.FirstName,
                                              OwnerLastName = o.LastName,
                                              PreferredLender = h.Owner.PreferredLender,
                                              AgentFirstName = a.FirstName,
                                              AgentLastName = a.LastName,
                                              CommissionPercent = hfs.Agent.CommissionPercent,
                                              RecoName = reco.CompanyName,
                                              RecoPhone = reco.Phone,
                                          }).FirstOrDefault();

                    MessageBox.Show($"{ homeForSaleDetails.ToStackedString() }");
                    FoundHomesForSaleView.SelectedIndex = -1;
                }

                if (FoundSoldHomesView.Visibility == Visibility.Visible)
                {
                    SoldHomeModel selectedSh = FoundSoldHomesView.SelectedItem as SoldHomeModel;

                    if (selectedSh == null)
                    {
                        DisplayStatusMessage("Select an item in the results before clicking the button.");
                        logger.Data("Get Item Details button", "No item selected in UI before clicking GID button.");
                        logger.Flush();
                        return;
                    }

                    objectType.Append(selectedSh.GetType().ToString());
                    objectContents.Append(selectedSh.ToString());

                    var soldHomeDetails = new SoldHomeDetailModel();
                    soldHomeDetails = (from hfs in homeSalesCollection
                                       where hfs.HomeID == selectedSh.HomeID
                                       join h in homesCollection on hfs.HomeID equals h.HomeID
                                       join reco in reCosCollection on hfs.CompanyID equals reco.CompanyID
                                       join o in peopleCollection on h.OwnerID equals o.PersonID
                                       join a in peopleCollection on hfs.AgentID equals a.PersonID
                                       select new SoldHomeDetailModel
                                       {
                                           HomeID = h.HomeID,
                                           Address = h.Address,
                                           City = h.City,
                                           State = h.State,
                                           Zip = h.Zip,
                                           SaleAmount = hfs.SaleAmount,
                                           MarketDate = hfs.MarketDate,
                                           OwnerFirstName = o.FirstName,
                                           OwnerLastName = o.LastName,
                                           PreferredLender = h.Owner.PreferredLender,
                                           AgentFirstName = a.FirstName,
                                           AgentLastName = a.LastName,
                                           CommissionPercent = hfs.Agent.CommissionPercent,
                                           RecoName = reco.CompanyName,
                                           RecoPhone = reco.Phone,
                                           SoldDate = hfs.SoldDate
                                       }).FirstOrDefault();

                    MessageBox.Show($"{ soldHomeDetails.ToStackedString() }");
                    FoundSoldHomesView.SelectedIndex = -1;
                }

                if (FoundPeopleView.Visibility == Visibility.Visible)
                {
                    PersonModel foundPerson = FoundPeopleView.SelectedItem as PersonModel;
                    Person foundPersonFull = peopleCollection.FirstOrDefault(p => p.PersonID == foundPerson.PersonID);

                    if (foundPerson == null)
                    {
                        DisplayStatusMessage("Select an item in the results before clicking the button.");
                        logger.Data("Get Item Details button", "No item selected in UI before clicking GID button.");
                        logger.Flush();
                        return;
                    }

                    objectType.Append(foundPerson.GetType().ToString());
                    objectContents.Append(foundPerson.ToString());

                    if (foundPerson.PersonType == new BuyerModel().PersonType)
                    {
                        List<SoldHomeModel> purchasedHomes = new List<SoldHomeModel>();
                        purchasedHomes = (from hfs in homeSalesCollection
                                          where hfs.BuyerID == foundPerson.PersonID
                                          join h in homesCollection on hfs.HomeID equals h.HomeID
                                          select new SoldHomeModel
                                          {
                                              HomeID = h.HomeID,
                                              Address = h.Address,
                                              City = h.City,
                                              State = h.State,
                                              Zip = h.Zip,
                                              SaleAmount = hfs.SaleAmount,
                                              SoldDate = hfs.SoldDate
                                          }).ToList();

                        var buyerPerson = (from hfs in homeSalesCollection
                                           where hfs.BuyerID == foundPerson.PersonID
                                           select new BuyerModel
                                           {
                                               PersonID = foundPersonFull.PersonID,
                                               FirstName = foundPersonFull.FirstName,
                                               LastName = foundPersonFull.LastName,
                                               Phone = foundPersonFull.Phone,
                                               Email = foundPersonFull.Email ?? "- not supplied -",
                                               PersonType = foundPerson.PersonType,
                                               CreditRating = hfs.Buyer.CreditRating,
                                               PurchasedHomes = purchasedHomes
                                           }).FirstOrDefault();

                        MessageBox.Show(buyerPerson.ToStackedString());
                    }

                    if (foundPerson.PersonType == new OwnerModel().PersonType)
                    {
                        List<HomeDisplayModel> ownedHomes = new List<HomeDisplayModel>();
                        ownedHomes = (from h in homesCollection
                                      where h.OwnerID == foundPerson.PersonID
                                      select new HomeDisplayModel
                                      {
                                          HomeID = h.HomeID,
                                          Address = h.Address,
                                          City = h.City,
                                          State = h.State,
                                          Zip = h.Zip
                                      }).ToList();

                        //  TODO: resolve the bug where null reference is thrown when a new user is GetItemDetail'd
                        OwnerModel ownerPerson = (from p in peopleCollection
                                           join h in homesCollection on p.PersonID equals h.OwnerID
                                           where p.PersonID == foundPerson.PersonID
                                           select new OwnerModel
                                           {
                                               PreferredLender = p.Owner.PreferredLender,
                                               PersonID = foundPersonFull.PersonID,
                                               FirstName = foundPersonFull.FirstName,
                                               LastName = foundPersonFull.LastName,
                                               Phone = foundPersonFull.Phone,
                                               Email = foundPersonFull.Email ?? "- not supplied -",
                                               PersonType = foundPerson.PersonType,
                                               OwnerID = foundPersonFull.PersonID,
                                               OwnedHomes = ownedHomes
                                           }).FirstOrDefault();

                        MessageBox.Show(ownerPerson.ToStackedString());
                    }

                    if (foundPerson.PersonType == new AgentModel().PersonType)
                    {
                        List<SoldHomeModel> soldHomes = new List<SoldHomeModel>();
                        soldHomes = (from hfs in homeSalesCollection
                                     where hfs.AgentID == foundPerson.PersonID &&
                                     hfs.SoldDate != null
                                     join h in homesCollection on hfs.HomeID equals h.HomeID
                                     select new SoldHomeModel
                                     {
                                         HomeID = h.HomeID,
                                         Address = h.Address,
                                         City = h.City,
                                         State = h.State,
                                         Zip = h.Zip,
                                         SaleAmount = hfs.SaleAmount,
                                         SoldDate = hfs.SoldDate
                                     }).ToList();

                        List<HomeForSaleModel> homesForSale = new List<HomeForSaleModel>();
                        homesForSale = (from hfs in homeSalesCollection
                                        where hfs.AgentID == foundPersonFull.PersonID &&
                                        hfs.SoldDate == null
                                        join h in homesCollection on hfs.HomeID equals h.HomeID
                                        select new HomeForSaleModel
                                        {
                                            HomeID = hfs.HomeID,
                                            Address = h.Address,
                                            City = h.City,
                                            State = h.State,
                                            Zip = h.Zip,
                                            MarketDate = hfs.MarketDate,
                                            SaleAmount = hfs.SaleAmount
                                        }).ToList();

                        var agentPerson = (from hfs in homeSalesCollection
                                           join re in reCosCollection on hfs.CompanyID equals re.CompanyID
                                           select new AgentModel
                                           {
                                               PersonID = foundPersonFull.PersonID,
                                               FirstName = foundPersonFull.FirstName,
                                               LastName = foundPersonFull.LastName,
                                               Phone = foundPersonFull.Phone,
                                               Email = foundPersonFull.Email ?? "- not supplied -",
                                               PersonType = foundPerson.PersonType,
                                               CommissionRate = hfs.Agent.CommissionPercent,
                                               HomesOnMarket = homesForSale,
                                               SoldHomes = soldHomes,
                                               RECoID = re.CompanyID,
                                               RECompanyName = re.CompanyName,
                                               RECoPhone = re.Phone
                                           }).FirstOrDefault();

                        MessageBox.Show($"{ agentPerson.ToStackedString() }");

                    }

                    FoundPeopleView.SelectedIndex = -1;
                }
            }
            catch
            {
                DisplayStatusMessage($"Selected item did not have details to show.");
                logger.Data($"GetItemDetails", "Problem with selected object (next entry will have details)...");
                logger.Data(objectType.ToString(), objectContents.ToString());
                logger.Flush();
                //throw;
            }

        }

        private void MenuUpdateHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HomeSearchModel selectedHome = FoundHomesView.SelectedItem as HomeSearchModel;
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

            DisplayStatusMessage("Update Home Menu.");
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
                var selectedHome = FoundHomesView.SelectedItem as HomeSearchModel;
                Home hfsHome = homesCollection.Where(h => h.HomeID == selectedHome.HomeID).FirstOrDefault();
                List<HomeSale> hfsHomesales = homeSalesCollection.Where(hs => hs.HomeID == hfsHome.HomeID).ToList();
                hfsHome.HomeSales = hfsHomesales;

                var homeUpdaterWindow = new HomeUpdaterWindow();
                homeUpdaterWindow.UpdateType = "PUTONMARKET";
                homeUpdaterWindow.UpdateAgent = new Agent();
                homeUpdaterWindow.UpdatePerson = new Person();
                homeUpdaterWindow.UpdateHome = hfsHome;
                homeUpdaterWindow.UpdateHomeSale = new HomeSale();
                homeUpdaterWindow.UpdateReco = new RealEstateCompany();
                homeUpdaterWindow.Title = "Add Home: For Sale";
                homeUpdaterWindow.Show();
                ClearSearchResultsViews();
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
            PersonModel selectedPerson = FoundPeopleView.SelectedItem as PersonModel;
            try
            {
                updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
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
                        ClearSearchResultsViews();
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
            PersonModel selectedPerson = FoundPeopleView.SelectedItem as PersonModel;
            try
            {
                updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
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
                        ClearSearchResultsViews();
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
            PersonModel selectedPerson = FoundPeopleView.SelectedItem as PersonModel;
            try
            {
                updatePerson = peopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
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
                        ClearSearchResultsViews();
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
            ClearSearchResultsViews();
            var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
            var peopleSearchtool = new PeopleSearchTool(formattedSearchTerms);
            List<PersonModel> viewResults = peopleSearchtool.SearchResults;

            if (viewResults == null)
            {
                DisplayZeroResultsMessage();
                return;
            }

            FoundPeopleView.ItemsSource = viewResults;
            FoundPeopleView.Visibility = Visibility.Visible;
            DisplayStatusMessage($"Found { viewResults.Count } People. Select a result and click Details button for more information.");
            GetItemDetailsButton.IsEnabled = true;

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
