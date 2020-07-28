using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        //  Delegate allows passing information BACK to the calling Window
        //  https://stackoverflow.com/questions/21607925/c-sharp-return-variable-from-child-window-to-parent-window-in-wpf
        public event Action<List<HomeSale>> Check;

        public string SearchType { get; set; }
        public List<string> SearchTerms = new List<string>();
        public List<HomeSale> SearchResults = new List<HomeSale>();
        public SearchWindow()
        {
            InitializeComponent();
            string buttonText = $"Search for { SearchType }";
            searchButton.Content = buttonText;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            HomeSale temp = null;
            switch (SearchType)
            {
                //"Home",
                //"Home For Sale",
                //"Sold Home",
                //"Owner",
                //"Agent",
                //"Buyer"
                case "Agent":
                    {
                        break;
                    }
                case "Buyers":
                    {
                        break;
                    }
                case "Homes":
                    {
                        break;
                    }
                case "Home For Sale":
                    {
                        //  1.  create a collection of user-entered items
                        foreach (var term in searchTextbox.Text.Split(','))
                        {
                            SearchTerms.Add(term.Trim());
                            //  2.  query homesales collection using user query collection
                            temp = new HomeSale();
                            temp = MainWindow.homeSalesCollection.FirstOrDefault(x => x.SoldDate.ToString() == term);
                            if (temp != null)
                            {
                                //  3.  retain all ID's from any matches
                                SearchResults.Add(temp);
                            }
                        }
                        break;
                    }
                case "Owner":
                    {
                        break;
                    }
                case "Sold Home":
                    {
                        break;
                    }
                default:
                    {
                        //  People
                        break;
                    }

            }

            //  TODO: return results to MainWindow.searchResultsListBox ???
            Check(SearchResults);
            this.Close();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {
            //  clears previously entered and displayed information
            searchTextbox.Text = string.Empty;
            statusBarText.Text = "Enter search terms separated by commas.";
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
