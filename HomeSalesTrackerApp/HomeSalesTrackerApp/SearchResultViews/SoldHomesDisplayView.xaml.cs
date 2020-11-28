using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for SoldHomesDisplayView.xaml
    /// </summary>
    public partial class SoldHomesDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; } = false;
        public SoldHomesDisplayView()
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

            var selectedHomeForSale = DataGridItemsList.SelectedItem as SoldHomeModel;

            if (selectedHomeForSale != null)
            {
                HomesForSaleDetailsTextbox.Text = HomeSalesSearchHelper.GetSoldHomeItemDetails(selectedHomeForSale);
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
            }

        }

    }
}
