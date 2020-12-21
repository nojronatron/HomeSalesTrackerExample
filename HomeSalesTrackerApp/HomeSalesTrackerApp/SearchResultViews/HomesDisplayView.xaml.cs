using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for HomeDisplayView.xaml
    /// </summary>
    public partial class HomesDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; }
        
        public HomesDisplayView()
        {
            InitializeComponent();
            DetailsWindowIsOpen = false;
            GetDetailsButton.Content = "Select a Home then click here to see details.";
        }

        private void GetDetailsButton_Click(object sender, RoutedEventArgs e)
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

            if (selectedHome != null)
            {
                HomeDetailsTextbox.Text = HomeSearchHelper.GetHomeItemDetails(selectedHome);
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
                HomeDetailsBorder.Visibility = Visibility.Visible;
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
