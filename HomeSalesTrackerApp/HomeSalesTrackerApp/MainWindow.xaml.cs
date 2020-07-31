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
using HSTDataLayer;
using HomeSalesTrackerApp.CrudWindows;
using System.Windows.Controls.Primitives;
using HSTDataLayer.Helpers;

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
            homeSalesCollection = new HomeSalesCollection();
            List<HomeSale> homeSales = EntityLists.GetTreeListOfHomeSales();
            foreach (var homeSale in homeSales)
            {
                homeSalesCollection.Add(homeSale);
            }

            //peopleCollection = new PeopleCollection<Person>();
            List<Person> people = EntityLists.GetTreeListOfPeople();
            foreach (var person in people)
            {
                peopleCollection.Add(person);
            }


            homesCollection = new HomesCollection();
            List<Home> homes = EntityLists.GetTreeListOfHomes();
            foreach (var home in homes)
            {
                homesCollection.Add(home);
            }

            reCosCollection = new RealEstateCosCollection();
            List<RealEstateCompany> recos = EntityLists.GetTreeListOfRECompanies();
            foreach (var reco in recos)
            {
                reCosCollection.Add(reco);
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

        private void MenuSearchHomesForSale_Click(Object sender, RoutedEventArgs args)
        {
            ClearSearchResultsViews();
            var searchResults = new List<Home>();
            var searchTerms = new List<String>();
            string searchTermsText = searchTermsTextbox.Text;
            string[] searchTermsArr = searchTermsText.Split(',');
            searchTerms = searchTermsArr.Where(st => st.Length > 0 && string.IsNullOrEmpty(st) == false).ToList();

            if (searchTerms.Count > 0)
            {
                var unsoldHomes = (from hs in homeSalesCollection
                                   from h in homesCollection
                                   where hs.SoldDate == null
                                   select h).ToList();

                HomeSearchHelper(ref searchResults, ref searchTerms);

                searchResults = (from sr in searchResults
                                 from uh in unsoldHomes
                                 where sr.HomeID == uh.HomeID
                                 select sr).ToList();

                searchResults.Distinct();
            }

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

        private void menuUpdateHomeAsSold_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuRemoveHomeFromMarket_Click(object sender, RoutedEventArgs e)
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

        private void menuAddHomesForSale_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateSoldHome_Click(object sender, RoutedEventArgs e)
        {
            //
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
            //  TODO: Flush all Collections to the DB

            //  TODO: Make a call to backup DB to XML

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

    }
}
