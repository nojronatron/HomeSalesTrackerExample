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

        private void agentsTab_GotFocus(object sender, RoutedEventArgs e)
        {
            //var exploratoryLinqQuery = homeSalesCollection.Count(x => x.AgentID == 1 && x.SoldDate != null);

            string agentNotActive = "Agent No Longer Active";
            //  TODO: solve lazyload? problem: https://stackoverflow.com/questions/18398356/solving-the-objectcontext-instance-has-been-disposed-and-can-no-longer-be-used
            //  TODO: Create Methods to calculate Total Homes Sold, Total Homes For Sale, Total Commission Paid, and Total Home Sales Amount
            //  TODO: Solve Object not Instantiated exception thrown when doing select new, below
            var agentsReport = (from p in MainWindow.peopleCollection
                                from hs in MainWindow.homeSalesCollection
                                where hs.AgentID == p.PersonID
                                select new  //  TODO: Fix the problem where Exception is thrown if record tries to draw from Agent.AgentID and Agent is null
                                {
                                    hs.AgentID,
                                    AgentName = $"{ p.FirstName } { p.LastName }",
                                    AgentRECompany = $"{ hs.Agent.CompanyID?.ToString() ?? agentNotActive }",
                                    AgentPhone = p.Phone,
                                    AgentEmail = p.Email,
                                    AgentCommish = $"{ hs.Agent.CommissionPercent } %",
                                    AgentTotalHomesSold = $"frustrating",//{ homeSalesCollection.Count(x => x.AgentID == hs.AgentID && x.SoldDate != null) }",
                                    AgentForSale = $"TBD: Sum HomeSales.MarketDate - HomeSales.SoldDate",
                                    AgentTtlCommishPaid = $"TBD: Sum HomeSales.SaleAmount if HomeSales.SoldDate and HomeSales.AgentID == Agents.AgentID",
                                    AgentTtlSales = $"TBD: Sum HomeSale.SaleAmounts if SoldDate"
                                });

            agentsResultsListView.ItemsSource = agentsReport.ToList();
            DisplayStatusMessage("agentsReport rendering complete.");
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
