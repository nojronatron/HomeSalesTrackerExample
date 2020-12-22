using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;

using System;
using System.Text;
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
        private string DefaultButtonText = "Select a Sold Home then click here to see details.";
        public SoldHomesDisplayView()
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
                    SoldHomeDetailBorder.Visibility = Visibility.Collapsed;
                    return;
                }

                var selectedHomeForSale = FoundSoldHomesDataGrid.SelectedItem as SoldHomeModel;
                var outputMessage = new StringBuilder();
                
                if (selectedHomeForSale != null)
                {
                    outputMessage.Append( HomeSalesSearchHelper.GetSoldHomeItemDetails(selectedHomeForSale) );
                }
                else
                {
                    outputMessage.AppendLine("Select an item first.");
                }

                HomesForSaleDetailsTextbox.Text = outputMessage.ToString();
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
                SoldHomeDetailBorder.Visibility = Visibility.Visible;
            }
            catch
            {
                DetailsWindowIsOpen = false;
                MessageBox.Show("Something went wrong. Call the developer.");
            }
        }
    }
}
