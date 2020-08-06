using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for SoldHomesReport.xaml
    /// </summary>
    public partial class SoldHomesReport : Window
    {
        public SoldHomesReport()
        {
            InitializeComponent();
        }

        private void soldHomeReportWindowLoaded(object sender, RoutedEventArgs e)
        {
            //  solved: lazyload problem ref: https://stackoverflow.com/questions/18398356/solving-the-objectcontext-instance-has-been-disposed-and-can-no-longer-be-used
            var soldHomesReport = (from hsc in MainWindow.homeSalesCollection
                                   from b in MainWindow.peopleCollection
                                   from a in MainWindow.peopleCollection
                                   from rec in MainWindow.reCosCollection
                                   where b.PersonID == hsc.BuyerID
                                   where a.PersonID == hsc.AgentID
                                   select new
                                   {
                                       hsc.HomeID,
                                       hsc.Home.Address,
                                       hsc.Home.City,
                                       hsc.Home.State,
                                       hsc.Home.Zip,
                                       BuyerName = $"{ b.FirstName } { b.LastName }", //   Buyer.Person.FirstName Buyer.Person.LastName",
                                       AgentName = $"{ a.FirstName } { a.LastName }",   // hsc.Agent.ToString() }",   // Agent.Person.FirstName Agent.Person.LastName"
                                       RECoName =  rec.CompanyName, //    hsc.RealEstateCompany.CompanyName,
                                       SaleAmount = hsc.SaleAmount,
                                       SoldDate = hsc.SoldDate
                                   });
            soldHomesReportListview.ItemsSource = soldHomesReport.ToList();
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
            soldHomesReportListview.ItemsSource = null;
            DisplayStatusMessage("Refreshed report output.");
        }

    }
}
