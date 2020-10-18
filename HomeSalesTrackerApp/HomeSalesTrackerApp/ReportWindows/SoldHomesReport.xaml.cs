using HomeSalesTrackerApp.ReportsViewModels;

using System.Windows;

namespace HomeSalesTrackerApp.ReportWindows
{
    /// <summary>
    /// Interaction logic for SoldHomesReport.xaml
    /// </summary>
    public partial class SoldHomesReport : Window
    {
        public SoldHomesReport()
        {
            InitializeComponent();
            var soldHomesViewModel = new SoldHomesViewModel();
            DataContext = soldHomesViewModel;
        }

    }
}
