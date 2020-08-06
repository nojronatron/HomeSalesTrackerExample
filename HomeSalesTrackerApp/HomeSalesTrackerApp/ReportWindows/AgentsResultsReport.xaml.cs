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

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {
            agentsResultsListView.ItemsSource = null;
            DisplayStatusMessage("Refreshed report output.");
        }

    }
}
