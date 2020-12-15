using HomeSalesTrackerApp.CrudWindows;
using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using HomeSalesTrackerApp.ReportWindows;
using HomeSalesTrackerApp.SearchResultViewModels;
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
            var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
            var soldHomesDisplayViewModel = new SoldHomesDisplayViewModel(formattedSearchTerms);
            DataContext = soldHomesDisplayViewModel;
            FoundSoldHomesView.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Search for existing Homes using user provided search terms in the Search textbox. Return results to customer ListView with headers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSearchHomes_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
            var homeDisplayViewModel = new HomesDisplayViewModel(formattedSearchTerms);
            DataContext = homeDisplayViewModel;
            FoundHomesView.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Uses search terms in Search textbox to find Homes For Sale and returns results to custom Search Results ListView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuSearchHomesForSale_Click(Object sender, RoutedEventArgs args)
        {
            ClearSearchResultsViews();
            var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
            var homesForSaleDisplayViewModel = new HomesForSaleDisplayViewModel(formattedSearchTerms);
            DataContext = homesForSaleDisplayViewModel;
            FoundHomesForSaleView.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Use the Search textbox and UpdateHomeAsSold Menu Item to search for and update a HomeSale, Home, and OwnerID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUpdateHomeAsSold_Click(object sender, RoutedEventArgs e)
        {
            try
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

            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home first, then click Menu, Update Home As Sold.");
                logger.Data("MenuUpdateHome Exception", ex.Message);
                logger.Flush();
            }

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

                var homeSaleToRemove = homeSalesCollection
                    .Where(hs =>
                        hs.MarketDate == selectedHomeForSale.MarketDate &&
                        hs.SaleAmount == selectedHomeForSale.SaleAmount &&
                        hs.SaleID == selectedHomeForSale.HomeForSaleID &&
                        hs.HomeID == selectedHomeForSale.HomeID
                        )
                    .FirstOrDefault();

                if (homeSalesCollection.Remove(homeSaleToRemove))
                {

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            logger.Data("Main Window Closing", "Closing.");
            logger.Flush();
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
                var hfsHome = FoundHomesView.SelectedItem as Home;
                List<HomeSale> hfsHomesales = homeSalesCollection.Where(hs => hs.HomeID == hfsHome.HomeID).ToList();
                var homeUpdaterWindow = new HomeUpdaterWindow();
                homeUpdaterWindow.UpdateType = "PUTONMARKET";
                homeUpdaterWindow.UpdateAgent = new Agent();
                homeUpdaterWindow.UpdatePerson = new Person();
                homeUpdaterWindow.UpdateHome = hfsHome;
                homeUpdaterWindow.UpdateHomeSale = new HomeSale();
                homeUpdaterWindow.UpdateReco = new RealEstateCompany();
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

        private void MenuSearchPeople_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResultsViews();
            var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
            var peopleDisplayViewModel = new PeopleDisplayViewModel(formattedSearchTerms);
            DataContext = peopleDisplayViewModel;
            FoundPeopleView.Visibility = Visibility.Visible;

        }

    }
}
