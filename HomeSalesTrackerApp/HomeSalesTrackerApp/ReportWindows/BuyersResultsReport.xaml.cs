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
using System.Windows.Shapes;
using HomeSalesTrackerApp.DisplayModels;
using HSTDataLayer;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for BuyersResultsReport.xaml
    /// </summary>
    public partial class BuyersResultsReport : Window
    {
        public Buyer buyer { get; set; }
        public Person person { get; set; }
        public IEnumerable<FoundBuyerView> iFoundBuyers { get; set; }
        public List<FoundBuyerView> FoundBuyers { get; set; }

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
