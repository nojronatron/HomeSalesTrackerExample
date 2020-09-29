using HomeSalesTrackerApp.DisplayModels;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for BuyersResultsReport.xaml
    /// </summary>
    public partial class BuyersResultsReport : Window
    {
        public IEnumerable<BuyerView> iFoundBuyers { get; set; }
        public List<BuyerView> FoundBuyers { get; set; }

        public BuyersResultsReport()
        {
            InitializeComponent();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshReportView();
        }

        private void MenuRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshReportView();
        }

        private void RefreshReportView()
        {
            //  use a view model to display the results!
            FoundBuyers = iFoundBuyers.ToList();
            buyersResultsListView.ItemsSource = FoundBuyers;
        }

    }
}
