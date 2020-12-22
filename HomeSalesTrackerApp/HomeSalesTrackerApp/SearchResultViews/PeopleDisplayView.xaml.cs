using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.SearchResultViews
{
    /// <summary>
    /// Interaction logic for PeopleDisplayView.xaml
    /// </summary>
    public partial class PeopleDisplayView : UserControl
    {
        private bool DetailsWindowIsOpen { get; set; } = false;
        private string DefaultButtonText = "Select a Person then click here to see details.";
        public PeopleDisplayView()
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
                    PersonDetailsTextbox.Text = string.Empty;
                    DetailsWindowIsOpen = false;
                    GetDetailsButton.Content = DefaultButtonText;
                    PeopleDetailsBorder.Visibility = Visibility.Collapsed;
                    return;
                }

                var selectedPerson = FoundPeopleDataGrid.SelectedItem as PersonModel;
                var outputMessage = new StringBuilder();

                if (selectedPerson != null)
                {
                    outputMessage.Append( PeopleSearchTool.GetPersonDetails(selectedPerson) );
                }
                else
                {
                    outputMessage.AppendLine( "Select an item first." );
                }

                PersonDetailsTextbox.Text = outputMessage.ToString();
                DetailsWindowIsOpen = true;
                GetDetailsButton.Content = "Click here to close the details bubble.";
                PeopleDetailsBorder.Visibility = Visibility.Visible;
            }
            catch
            {
                DetailsWindowIsOpen = false;
                MessageBox.Show("Something went wrong. Call the developer.");
            }
        }

        private void FoundPeopleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPerson = FoundPeopleDataGrid.SelectedItem as PersonModel;
            if (selectedPerson != null)
            {
                PersonIDSelected registerPersonID = new PersonIDSelected(MainWindow.SetSelectedPerson);
                registerPersonID(selectedPerson.PersonID);
            }
        }
    }

}
