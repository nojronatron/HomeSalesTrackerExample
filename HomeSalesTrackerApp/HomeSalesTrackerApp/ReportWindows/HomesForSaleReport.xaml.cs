using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Report_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
