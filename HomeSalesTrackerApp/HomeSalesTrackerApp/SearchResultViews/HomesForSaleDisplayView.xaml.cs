using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;

using System;
using System.Text;
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
            GetDetailsButton.Content = DefaultButtonText;
        }

        private void GetDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                var outputMessage = new StringBuilder();

                if (selectedHomeForSale != null)
                {
                    outputMessage.Append(HomeSalesSearchHelper.GetHomeForSaleItemDetails(selectedHomeForSale));
                }
                else
                {
                    outputMessage.AppendLine("Select an item first.");
                }

                HomesForSaleDetailsTextbox.Text = outputMessage.ToString();
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
                HomeForSaleBorder.Visibility = Visibility.Visible;
            }
            catch
            {
                DetailsWindowIsOpen = false;
                MessageBox.Show("Something went wrong. Call the developer.");
            }
        }

        private void FoundHomesForSaleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedHomesale = FoundHomesForSaleDataGrid.SelectedItem as HomeForSaleModel;
            if (selectedHomesale != null)
            {
                HomeSaleIDSelected registerHomesaleID = new HomeSaleIDSelected(MainWindow.SetSelectedHomesale);
                registerHomesaleID(selectedHomesale.HomeForSaleID);
                HomeIDSelected registerHomeID = new HomeIDSelected(MainWindow.SetSelectedHome);
                registerHomeID(selectedHomesale.HomeID);
            }
        }
    }
}
