using HomeSalesTrackerApp.ReportsViewModels;
using System.Windows;

namespace HomeSalesTrackerApp.ReportWindows
{
    /// <summary>
    /// Interaction logic for RealEstateCoReport.xaml
    /// </summary>
    public partial class RealEstateCoReport : Window
    {
        public RealEstateCoReport()
        {
            InitializeComponent();
            var realEstateCoReportViewModel = new RealEstateCoReportViewModel();
            DataContext = realEstateCoReportViewModel;
        }
    }
}
