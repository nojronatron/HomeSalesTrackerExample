using HomeSalesTrackerApp.Report_Models;

using System.Windows;

namespace HomeSalesTrackerApp.ReportWindows
{
    /// <summary>
    /// Interaction logic for HomesForSaleReport.xaml
    /// </summary>
    public partial class HomesForSaleReport : Window
    {
        public HomesForSaleReport()
        {
            InitializeComponent();
            var hfsViewModel = new HomesForSaleViewModel();
            DataContext = hfsViewModel;
        }
    }
}
