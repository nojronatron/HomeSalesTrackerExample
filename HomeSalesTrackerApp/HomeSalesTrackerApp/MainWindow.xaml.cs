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
using HSTDataLayer.Helpers;
using HomeSalesTrackerApp.CrudWindows;
using System.Windows.Controls.Primitives;

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

        private void clearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            searchResultsListbox.ItemsSource = null;
            searchTermsTextbox.Text = string.Empty;
            DisplayStatusMessage("Ready.");
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            //  App.Exit triggers Application_Exit Method that calls data layer to backup DB to XML
            //this.Close();
            App.Current.Shutdown();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            //  App.Exit triggers Application_Exit Method that calls data layer to backup DB to XML
            App.Current.Shutdown();
            //this.Close();
        }

        private void menuSearchHomes_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Search Homes menu clicked.");
            
        }

        private void menuSearchHomeForSale_Click(Object sender, RoutedEventArgs args)
        {
            int items = 0;
            DisplayStatusMessage("Search for Homes For Sale.");
            List<Home> SearchResults = new List<Home>();
            List<String> searchTerms = new List<String>();
            if (searchTermsTextbox.Text != null)
            {
                foreach (var searchTerm in searchTermsTextbox.Text.Split(','))
                {
                    searchTerms.Add(searchTerm.Trim());

                    foreach (Home h in homesCollection)
                    {
                        if (h.Address.Contains(searchTerm) ||
                            h.City.Contains(searchTerm) ||
                            h.State.Contains(searchTerm) ||
                            h.Zip.Contains(searchTerm))
                        {
                            SearchResults.Add(h);
                        }
                    }
                }

                if (SearchResults.Count > 0)
                {
                    //  deliver results to the screen
                    var results = (from h in SearchResults
                                   from hs in homeSalesCollection
                                   where h.HomeID == hs.HomeID && hs.SoldDate == null
                                   select new
                                   {
                                       HomeID = h.HomeID,
                                       Address = h.Address,
                                       City = h.City,
                                       State = h.State,
                                       Zip = h.Zip,
                                       SaleAmount = hs.SaleAmount,
                                       MarketDate = hs.MarketDate
                                   });
                    //this.searchResultsListbox.DisplayMemberPath = "SaleAmount";
                    //this.searchResultsListbox.SelectedValue = "MarketDate";
                    var listResults = results.ToList();
                    searchResultsListbox.ItemsSource = listResults;
                    items = listResults.Count;
                    DisplayStatusMessage($"Displaying { items } search results.");
                }
                else
                {
                    DisplayStatusMessage($"Search return { items } items.");
                }
            }
        }

        private void menuSearchSoldHomes_Click(object sender, RoutedEventArgs e)
        {
            //  SoldDate != null

        }

        private void menuSearchOwners_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuSearchAgents_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuSeawrchBuyers_Click(object sender, RoutedEventArgs e)
        {
            //
        }
        
        private void menuAddHome_Click(object sender, RoutedEventArgs e)
        {
            AddHomeWindow ahw = new AddHomeWindow();
            ahw.AddType = "Home";
            ahw.Show();
        }

        private void menuAddOwner_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Owner";
        }

        private void menuAddAgent_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Agent";
        }

        private void menuAddBuyer_Click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Buyer";
        }

        private void menuUpdateHome_Click(object sender, RoutedEventArgs e)
        {
            //
        }

        private void menuUpdateHomeForSale_Click(object sender, RoutedEventArgs e)
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  TODO: Flush all Collections to the DB

            //  TODO: Make a call to backup DB to XML

        }

        private void FullDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayStatusMessage("Full Details button clicked. Not yet implemented");
        }
    }
}
