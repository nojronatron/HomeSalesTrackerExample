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
        }


        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DisplayStatusMessage(string message)
        {
            statusBarText.Text = message.Trim().ToString();
        }

    }
}
