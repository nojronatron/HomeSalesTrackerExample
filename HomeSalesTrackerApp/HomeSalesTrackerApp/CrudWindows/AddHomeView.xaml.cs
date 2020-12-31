using System;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddHomeView.xaml
    /// </summary>
    public partial class AddHomeView : UserControl
    {
        //  Create an event to handle when a new Home has been created
        public static event EventHandler<HSTDataLayer.Person> RaiseNewHomeCreatedEvent;

        public AddHomeView()
        {
            InitializeComponent();
        }

        private void AddNewOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("AddNewownerButton clicked!");
        }

        private void ExistingOwnersCombobox_selectionchanged(object sender, SelectionChangedEventArgs e)
        {
            //  TODO: do something with selectedOwner or remove this code
            MessageBox.Show("ExistingOwnersCombobox selectionchanged!");
        }

        private void SaveHomeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOwner = ExistingOwnersCombobox.SelectedItem as HSTDataLayer.Person;

            //  trigger
            RaiseNewHomeCreatedEvent?.Invoke(this, selectedOwner);
        }
    }
}
