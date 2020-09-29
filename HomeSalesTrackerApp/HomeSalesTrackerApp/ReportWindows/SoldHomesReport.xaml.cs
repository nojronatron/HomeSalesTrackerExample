using HomeSalesTrackerApp.DisplayModels;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for SoldHomesReport.xaml
    /// </summary>
    public partial class SoldHomesReport : Window
    {
        public IEnumerable<SoldHomesView> iFoundSoldHomes { get; set; }
        public List<SoldHomesView> FoundSoldHomes { get; set; }

        public SoldHomesReport()
        {
            InitializeComponent();
        }

        private void SoldHomeReportWindowLoaded(object sender, RoutedEventArgs e)
        {
            //  solved: lazyload problem ref: https://stackoverflow.com/questions/18398356/solving-the-objectcontext-instance-has-been-disposed-and-can-no-longer-be-used

            FoundSoldHomes = iFoundSoldHomes.ToList();
            soldHomesReportListview.ItemsSource = FoundSoldHomes;
            DisplayStatusMessage("soldHomesReport rendering complete.");
        }

        private void DisplayStatusMessage(string message)
        {
            statusBarText.Text = message.Trim().ToString();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {
            soldHomesReportListview.ItemsSource = FoundSoldHomes;
            DisplayStatusMessage("Refreshed report output.");
        }

    }
}
