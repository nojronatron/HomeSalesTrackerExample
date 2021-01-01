using HomeSalesTrackerApp.CrudWindows;
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
    public enum TypeOfPerson { Agent, Buyer, Owner };

    delegate void HomeIDSelected(int selectedHomeID);
    delegate void HomeSaleIDSelected(int selectedHomesaleID);
    delegate void PersonIDSelected(int selectedPersonID);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger logger = null;
        protected static int SelectedHomeID { get; set; } = 0;   //  to be received when a Home is selected in another view
        protected static int SelectedHomesaleID { get; set; } = 0;   //  to be received when a Homesale is selected in another view
        protected static int SelectedPersonID { get; set; } = 0;    //  to be received when a Person is selected in another view
        internal static void SetSelectedHome(int homeID)
        {
            SelectedHomeID = homeID;
        }
        internal static void SetSelectedHomesale(int homesaleID)
        {
            SelectedHomesaleID = homesaleID;
        }
        internal static void SetSelectedPerson(int personID)
        {
            SelectedPersonID = personID;
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
            AddNewHomeView.Visibility = Visibility.Hidden;
            AddNewPersonView.Visibility = Visibility.Hidden;
            DataContext = null;
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
            try
            {
                ClearSearchResultsViews();
                var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
                var soldHomesDisplayViewModel = new SoldHomesDisplayViewModel(formattedSearchTerms);
                DataContext = soldHomesDisplayViewModel;
                FoundSoldHomesView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Search Sold Homes results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }

        }

        /// <summary>
        /// Search for existing Homes using user provided search terms in the Search textbox. Return results to customer ListView with headers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSearchHomes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();
                var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
                var homeDisplayViewModel = new HomesDisplayViewModel(formattedSearchTerms);
                DataContext = homeDisplayViewModel;
                FoundHomesView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Home Search results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }

        }

        /// <summary>
        /// Uses search terms in Search textbox to find Homes For Sale and returns results to custom Search Results ListView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuSearchHomesForSale_Click(Object sender, RoutedEventArgs args)
        {
            try
            {
                ClearSearchResultsViews();
                var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
                var homesForSaleDisplayViewModel = new HomesForSaleDisplayViewModel(formattedSearchTerms);
                DataContext = homesForSaleDisplayViewModel;
                FoundHomesForSaleView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Home For Sale Search results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
            
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
                ClearSearchResultsViews();

                if (MainWindow.SelectedHomesaleID > 0 && MainWindow.SelectedHomeID > 0)
                {
                    var homeUpdaterWindow = new HomeUpdaterWindow(MainWindow.SelectedHomeID, SelectedHomesaleID);
                    DisplayStatusMessage("Loading update window");
                    homeUpdaterWindow.Show();
                }
                else
                {
                    DisplayStatusMessage("Select an item from the results and try again.");
                    return;
                }

            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home first, then click Menu, Update Home As Sold.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }

        }

        /// <summary>
        /// Selected Home For Sale is taken off the Market.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuRemoveHomeFromMarket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();

                if (MainWindow.SelectedHomesaleID > 0 && MainWindow.SelectedHomeID > 0)
                {
                    RemoveHomeFromMarket(MainWindow.SelectedHomeID, MainWindow.SelectedHomesaleID);
                }
                else
                {
                    DisplayStatusMessage("Select an item in the search results before choosing to remove it from the Market.");
                }
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to Remove Home from Market.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Helper method for Removing a Homesale (take home off Market).
        /// </summary>
        /// <param name="homeID"></param>
        /// <param name="homesaleID"></param>
        /// <returns></returns>
        private void RemoveHomeFromMarket(int homeID, int homesaleID)
        {
            var homeSaleToRemove = ((App)Application.Current)._homeSalesCollection
                .Where(hs =>
                    hs.SaleID == homesaleID &&
                    hs.HomeID == homeID
                    )
                .FirstOrDefault();

            if (homeSaleToRemove != null)
            {
                var homesaleAddress = homeSaleToRemove.Home.Address;

                if (((App)Application.Current)._homeSalesCollection.Remove(homeSaleToRemove))
                {
                    SetSelectedHomesale(0);
                    DisplayStatusMessage($"Home at { homesaleAddress } removed from the For Sale market.");
                    ClearSearchResultsViews();
                }
            }
        }

        /// <summary>
        /// Add a new Home instance to the DB and a attach it to new or existing Agent or Owner instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();
                //var ahw = new AddHomeWindow("Home", "Add New Home");
                //ahw.Show();
                DataContext = new AddHomeViewModel();
                AddNewHomeView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Add Home window.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Add a new Owner instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddOwner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();
                DataContext = new AddPersonViewModel(TypeOfPerson.Owner.ToString());
                AddNewPersonView.Visibility = Visibility.Visible;
                //var apw = new AddPersonWindow();
                //apw.AddType = "Owner";
                //apw.Title = "Add New Owner Person";
                //apw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Add Owner window.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Add a new Agent instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddAgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();
                DataContext = new AddPersonViewModel(TypeOfPerson.Agent.ToString());
                AddNewPersonView.Visibility = Visibility.Visible;
                //var apw = new AddPersonWindow();
                //apw.AddType = "Agent";
                //apw.Title = "Add New Agent Person";
                //apw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Add Agent window.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Add a new Buyer instance to the DB and attach it to a new or existing Person instance, regardless of Search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAddBuyer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();
                var apw = new AddPersonWindow();
                apw.AddType = "Buyer";
                apw.Title = "Add New Buyer Person";
                apw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Add Buyer window.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Display a summary of Sold Homes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuDisplaySoldHomes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var soldHomesReport = new SoldHomesReport();
                soldHomesReport.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Sold Homes.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void MenuDisplayBuyers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var buyersResultsReport = new BuyersResultsReport();
                buyersResultsReport.Show();

            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Buyers.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void menuDisplayAgents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //  testing forced disposal/GC
                using (AgentsResultsReport agentsResultsReport = new AgentsResultsReport())
                {
                    agentsResultsReport.Show();
                };
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Agents.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void menuDisplayRECoTotals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var realEstateCoReport = new RealEstateCoReport();
                realEstateCoReport.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Real Estate Totals.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {

            }
        }

        private void menuDisplayHomesForSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var homesForSaleReport = new HomesForSaleReport();
                homesForSaleReport.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display Home For Sale.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void MenuAboutAppInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxCaption = "About: Home Sales Tracker App";
                string messageBoxText = $"App: Home Sales Tracker\nAuthor: Jon Rumsey\nSummer and Fall of 2020";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, messageBoxCaption, button, icon);
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to display About information.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
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
                ClearSearchResultsViews();
                HomeSearchModel selectedHome = FoundHomesView.SelectedItem as HomeSearchModel;
                var homeID = selectedHome.HomeID;
                var ahw = new AddHomeWindow();
                ahw.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home first, then click Menu, Update Home.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
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
                ClearSearchResultsViews();
                DisplayStatusMessage($"Selected Home ID: { SelectedHomeID }.");
                var homeUpdaterWindow = new HomeUpdaterWindow(MainWindow.SelectedHomeID);
                homeUpdaterWindow.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Select a Home that is not already for Sale then click Menu Add Home For Sale.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
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
                ClearSearchResultsViews();
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
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to load Agent Update Window. Refresh, search again, and select a Person in the results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void MenuUpdateBuyer_Click(object sender, RoutedEventArgs e)
        {
            var updatePerson = new Person();
            var updateBuyer = new Buyer();
            PersonModel selectedPerson = FoundPeopleView.SelectedItem as PersonModel;
            try
            {
                ClearSearchResultsViews();
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
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to load Buyer Update Window. Refresh, search again, and select a Person in the results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void MenuUpdateOwner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearSearchResultsViews();
                var personUpdaterWindow = new PersonUpdaterWindow(true, "Owner", MainWindow.SelectedPersonID);
                personUpdaterWindow.Show();
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to load Owner Update Window. Refresh, search again, and then select a Person in the results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }
        }

        private void MenuSearchPeople_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formattedSearchTerms = FormatSearchTerms.FormatTerms(searchTermsTextbox.Text);
                var peopleDisplayViewModel = new PeopleDisplayViewModel(formattedSearchTerms);
                DataContext = peopleDisplayViewModel;
                FoundPeopleView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to load People Search results.");
                logger.Data(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                logger.Flush();
            }
            finally
            {
            }

        }

    }

}
