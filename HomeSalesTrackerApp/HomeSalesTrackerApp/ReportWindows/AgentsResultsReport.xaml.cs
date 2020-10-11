using HomeSalesTrackerApp.ReportsViewModels;
using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for AgentsResultsReport.xaml
    /// </summary>
    public partial class AgentsResultsReport : Window
    {
        public AgentsResultsReport()
        {
            InitializeComponent();
            var agentsReportViewModel = new AgentsReportViewModel();
            DataContext = agentsReportViewModel;
        }

    }
}
