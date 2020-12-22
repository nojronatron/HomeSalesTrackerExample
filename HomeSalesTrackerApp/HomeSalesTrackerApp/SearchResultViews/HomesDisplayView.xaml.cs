using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for HomeDisplayView.xaml
    /// </summary>
    public partial class HomesDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; } = false;
        
        public HomesDisplayView()
        {
            InitializeComponent();
            GetDetailsButton.Content = "Select a Home then click here to see details.";
        }

        private void GetDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DetailsWindowIsOpen)
                {
                    HomeDetailsTextbox.Text = string.Empty;
                    DetailsWindowIsOpen = false;
                    GetDetailsButton.Content = "Select a Home then click here to see details.";
                    HomeDetailsBorder.Visibility = Visibility.Collapsed;
                    return;
                }

                var selectedHome = FoundHomesDataGrid.SelectedItem as HomeDisplayModel;
                var outputMessage = new StringBuilder();

                if (selectedHome != null)
                {
                    outputMessage.Append(HomeSearchHelper.GetHomeItemDetails(selectedHome));
                }
                else
                {
                    outputMessage.AppendLine("Select an item first.");
                }

                HomeDetailsTextbox.Text = outputMessage.ToString();
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
                HomeDetailsBorder.Visibility = Visibility.Visible;
            }
            catch
            {
                DetailsWindowIsOpen = false;
                MessageBox.Show("Something went wrong. Call the developer.");
            }
        }

        private void FoundHomesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedHome = FoundHomesDataGrid.SelectedItem as HomeDisplayModel;
            if (selectedHome != null)
            {
                HomeIDSelected registerHomeID = new HomeIDSelected(MainWindow.SetSelectedHome);
                registerHomeID(selectedHome.HomeID);
            }
        }
    }
}
