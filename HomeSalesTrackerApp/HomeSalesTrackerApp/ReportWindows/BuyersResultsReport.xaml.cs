using HomeSalesTrackerApp.ReportsViewModels;

using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for BuyersResultsReport.xaml
    /// </summary>
    public partial class BuyersResultsReport : Window
    {
        public BuyersResultsReport()
        {
            InitializeComponent();
            var buyersReportViewModel = new BuyersReportViewModel();
            DataContext = buyersReportViewModel;
        }

    }
}
