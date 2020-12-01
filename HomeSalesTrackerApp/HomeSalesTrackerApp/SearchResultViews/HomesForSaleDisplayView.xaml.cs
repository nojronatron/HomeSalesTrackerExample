using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for HomesForSaleDisplayView.xaml
    /// </summary>
    public partial class HomesForSaleDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; } = false;
        private string DefaultButtonText = "Select a Home For Sale then click here to see details.";
        public HomesForSaleDisplayView()
        {
            InitializeComponent();
            DetailsWindowIsOpen = false;
            GetDetailsButton.Content = DefaultButtonText;
        }

        private void GetDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailsWindowIsOpen)
            {
                HomesForSaleDetailsTextbox.Text = string.Empty;
                DetailsWindowIsOpen = false;
                GetDetailsButton.Content = DefaultButtonText;
                HomeForSaleBorder.Visibility = Visibility.Collapsed;
                return;
            }

            var selectedHomeForSale = FoundHomesForSaleDataGrid.SelectedItem as HomeForSaleModel;

            if (selectedHomeForSale != null)
            {
                HomesForSaleDetailsTextbox.Text = HomeSalesSearchHelper.GetHomeForSaleItemDetails(selectedHomeForSale);
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
                HomeForSaleBorder.Visibility = Visibility.Visible;
            }

        }
    }
}
