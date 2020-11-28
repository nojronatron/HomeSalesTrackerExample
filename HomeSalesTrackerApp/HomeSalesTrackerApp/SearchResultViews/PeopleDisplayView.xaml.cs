using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for PeopleDisplayView.xaml
    /// </summary>
    public partial class PeopleDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; }
        private string DefaultButtonText = "Select a Person then click here to see details.";
        public PeopleDisplayView()
        {
            InitializeComponent();
            DetailsWindowIsOpen = false;
            GetDetailsButton.Content = DefaultButtonText;
        }

        private void GetDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailsWindowIsOpen)
            {
                PersonDetailsTextbox.Text = string.Empty;
                DetailsWindowIsOpen = false;
                GetDetailsButton.Content = DefaultButtonText;
                return;
            }

            var selectedPerson = DataGridItemsList.SelectedItem as PersonModel;

            if (selectedPerson != null)
            {
                PersonDetailsTextbox.Text = PeopleSearchTool.GetPersonDetails(selectedPerson);
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
            }

        }

    }
}
