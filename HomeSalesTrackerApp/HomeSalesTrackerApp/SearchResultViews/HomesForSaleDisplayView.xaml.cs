using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for HomesForSaleDisplayView.xaml
    /// </summary>
    public partial class HomesForSaleDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; } = false;
        public HomesForSaleDisplayView()
        {
            InitializeComponent();
            DetailsWindowIsOpen = false;
            GetDetailsButton.Content = "Select a Home then click here to see details.";
        }

        private void GetDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailsWindowIsOpen)
            {
                HomesForSaleDetailsTextbox.Text = string.Empty;
                DetailsWindowIsOpen = false;
                GetDetailsButton.Content = "Select a Home then click here to see details.";
                return;
            }

            var selectedHomeForSale = DataGridItemsList.SelectedItem as HomeForSaleModel;

            if (selectedHomeForSale != null)
            {
                HomesForSaleDetailsTextbox.Text = HomeSalesSearchHelper.GetHomeForSaleItemDetails(selectedHomeForSale);
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
            }

        }
    }
}
