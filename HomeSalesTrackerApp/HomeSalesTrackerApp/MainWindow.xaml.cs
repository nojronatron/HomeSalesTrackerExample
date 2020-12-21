﻿using HomeSalesTrackerApp.CrudWindows;
using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using HomeSalesTrackerApp.ReportWindows;
using HomeSalesTrackerApp.SearchResultViewModels;
using HSTDataLayer;

using System;
using System.Linq;
using System.Windows;

namespace HomeSalesTrackerApp
{
    delegate void HomeIDSelected(int selectedHomeID);
    delegate void HomeSaleIDSelected(int selectedHomesaleID);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger logger = null;
        protected static int SelectedHomeID { get; set; } = 0;   //  to be received when a Home is Selected in another view
        protected static int SelectedHomesaleID { get; set; } = 0;   //  to be received when a Homesale is Selected in another view
        internal static void SetSelectedHome(int homeID)
        {
            SelectedHomeID = homeID;
        }
        internal static void SetSelectedHomesale(int homesaleID)
        {
            SelectedHomesaleID = homesaleID;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logger = new Logger();

            if (App.DatabaseInitLoaded)
            {
                logger.Data("MainWindow Loaded", "Database data loaded.");
                DisplayStatusMessage("Database data loaded.");
            }
            else
            {
                logger.Data("MainWindow Loaded", "Database data NOT loaded.");
            }

            logger.Flush();
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
                //var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
                //StringBuilder statusMessage = new StringBuilder("Ok. ");

                //int homeID = -1;
                int homesaleID = 0;
                int homeID = 0;
                //if (FoundHomesForSaleView.IsVisible)
                //{
                //    HomeForSaleModel selectedHomesaleView = FoundHomesForSaleView.SelectedItem as HomeForSaleModel;
                //    homeID = selectedHomesaleView.HomeID;
                //}
                //else if (FoundHomesView.IsVisible)
                //{
                //    HomeSearchModel selectedHomeSearchModel = FoundHomesView.SelectedItem as HomeSearchModel;
                //    homeID = selectedHomeSearchModel.HomeID;
                //}
                if (MainWindow.SelectedHomesaleID > 0 && MainWindow.SelectedHomeID > 0)
                {
                    homesaleID = MainWindow.SelectedHomesaleID;
                    homeID = MainWindow.SelectedHomeID;
                }
                else
                {
                    DisplayStatusMessage("Select an item from the results and try again.");
                    return;
                }

                //HomeSale hfsHomesale = fHomeSalesCollection.Where(hs => hs.HomeID == homeID &&
                                                                       //hs.MarketDate != null &&
                                                                       //hs.SoldDate == null).FirstOrDefault();

                ////Home hfsHome = homesCollection.Where(h => h.HomeID == homeID).FirstOrDefault();
                //Home hfsHome = (Factory.CollectionFactory.GetHomesCollectionObject()).Where(h => h.HomeID == homeID).FirstOrDefault();

                //if (hfsHome != null && hfsHomesale != null)
                //{
                //    var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
                //    Person hfsAgent = new Person();
                //    hfsAgent = fPeopleCollection.Where(p => p.PersonID == hfsHomesale.AgentID).FirstOrDefault();

                //    if (hfsAgent != null && hfsAgent.Agent.CompanyID != null)
                //    {
                //        var fRECosCollection = CollectionFactory.GetRECosCollectionObject();
                //        RealEstateCompany hfsReco = new RealEstateCompany();
                //        hfsReco = fRECosCollection.Where(r => r.CompanyID == hfsAgent.Agent.CompanyID).FirstOrDefault();
                //        if (hfsReco != null)
                //        {
                var homeUpdaterWindow = new HomeUpdaterWindow(homeID, homesaleID);
                //            //homeUpdaterWindow.UpdateType = "HOMESOLD";
                //            //homeUpdaterWindow.UpdatePerson = hfsAgent;
                //            //homeUpdaterWindow.UpdateAgent = hfsAgent.Agent;
                //            //homeUpdaterWindow.UpdateHome = hfsHome;
                //            //homeUpdaterWindow.UpdateHomeSale = hfsHomesale;
                //            //homeUpdaterWindow.UpdateReco = hfsReco;
                DisplayStatusMessage("Loading update window");
                homeUpdaterWindow.Show();
                ClearSearchResultsViews();

                //if (statusMessage.Length > 4)
                //{
                //    DisplayStatusMessage(statusMessage.ToString());
                //}

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
                //var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
                var homeSaleToRemove = ((App)Application.Current)._homeSalesCollection
                    .Where(hs =>
                        hs.MarketDate == selectedHomeForSale.MarketDate &&
                        hs.SaleAmount == selectedHomeForSale.SaleAmount &&
                        hs.SaleID == selectedHomeForSale.HomeForSaleID &&
                        hs.HomeID == selectedHomeForSale.HomeID
                        )
                    .FirstOrDefault();

                if (((App)Application.Current)._homeSalesCollection.Remove(homeSaleToRemove))
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
            var ahw = new AddHomeWindow("Home", "Add New Home");
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
                //var fHomeSalesCollection = CollectionFactory.GetHomesCollectionObject();
                //var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
                HomeSearchModel selectedHome = FoundHomesView.SelectedItem as HomeSearchModel;
                //Home home = homesCollection.Where(h => h.HomeID == selectedHome.HomeID).FirstOrDefault();
                //Home home = (Factory.CollectionFactory.GetHomesCollectionObject()).Where(h => h.HomeID == selectedHome.HomeID).FirstOrDefault();
                //home.HomeSales = (ICollection<HomeSale>) fHomeSalesCollection.Where(hs => hs.HomeID == home.HomeID).ToList();
                //home.Owner = fPeopleCollection.Where(o => o.PersonID == home.OwnerID).FirstOrDefault().Owner;
                //var ahw = new AddHomeWindow();
                //ahw.NewHome = home;
                //ahw.AnOwner = home.Owner;
                //ahw.APerson = fPeopleCollection.Where(p => p.PersonID == home.Owner.OwnerID).FirstOrDefault();
                //ahw.UpdateInsteadOfAdd = true;
                var homeID = selectedHome.HomeID;
                var ahw = new AddHomeWindow();
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
                //var fHomeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
                //List<HomeSale> hfsHomesales = fHomeSalesCollection.Where(hs => hs.HomeID == hfsHome.HomeID).ToList();

                //var hfsHome = FoundHomesView.SelectedItem as Home;
                //var homeUpdaterWindow = new HomeUpdaterWindow(hfsHome.HomeID);
                DisplayStatusMessage($"Selected Home ID: { SelectedHomeID }.");
                var homeUpdaterWindow = new HomeUpdaterWindow(MainWindow.SelectedHomeID);

                //homeUpdaterWindow.UpdateType = "PUTONMARKET";
                //homeUpdaterWindow.UpdateAgent = new Agent();
                //homeUpdaterWindow.UpdatePerson = new Person();
                //homeUpdaterWindow.UpdateHome = hfsHome;
                //homeUpdaterWindow.UpdateHomeSale = new HomeSale();
                //homeUpdaterWindow.UpdateReco = new RealEstateCompany();
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
                //var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
                //if (selectedPerson != null)
                //{
                //    updatePerson = fPeopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();

                //    if (updatePerson != null)
                //    {
                //        updateAgent = updatePerson.Agent;
                //        var puw = new PersonUpdaterWindow();
                //        puw.CalledByUpdateMenu = true;
                //        puw.CalledByUpdateMenuType = "Agent";
                //        puw.ReceivedPerson = updatePerson;
                //        puw.ReceivedAgent = updateAgent;
                //        puw.Title = "Update Person's Agent Info";
                //        puw.Show();
                //        ClearSearchResultsViews();
                //    }
                //}
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
                //var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
                //updatePerson = fPeopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
                //if (updatePerson.Buyer != null)
                //{
                //    updateBuyer = updatePerson.Buyer;
                //}

                //if (selectedPerson != null)
                //{
                //    if (updatePerson != null)
                //    {
                //        var puw = new PersonUpdaterWindow();
                //        puw.CalledByUpdateMenu = true;
                //        puw.CalledByUpdateMenuType = "Buyer";
                //        puw.ReceivedPerson = updatePerson;
                //        puw.ReceivedBuyer = updateBuyer;
                //        puw.Title = "Update Person's Buyer Info";
                //        puw.Show();
                //        ClearSearchResultsViews();
                //    }
                //}

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
                //var fPeopleCollection = CollectionFactory.GetPeopleCollectionObject();
                //updatePerson = fPeopleCollection.Where(p => p.PersonID == selectedPerson.PersonID).FirstOrDefault();
                //if (updatePerson.Owner != null)
                //{
                //    updateOwner = updatePerson.Owner;
                //}

                //if (selectedPerson != null)
                //{

                //    if (updatePerson != null)
                //    {
                //        var puw = new PersonUpdaterWindow();
                //        puw.CalledByUpdateMenu = true;
                //        puw.CalledByUpdateMenuType = "Owner";
                //        puw.ReceivedPerson = updatePerson;
                //        puw.ReceivedOwner = updateOwner;
                //        puw.Title = "Update Person's Owner Info";
                //        puw.Show();
                //        ClearSearchResultsViews();
                //    }
                //}

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
