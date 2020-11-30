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
                return;
            }

            var selectedHome = DataGridItemsList.SelectedItem as HomeDisplayModel;

            if (selectedHome != null)
            {
                HomeDetailsTextbox.Text = HomeSearchHelper.GetHomeItemDetails(selectedHome);
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
            }

        }

    }
}
