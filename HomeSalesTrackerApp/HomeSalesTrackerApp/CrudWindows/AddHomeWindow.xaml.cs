using HomeSalesTrackerApp.CrudWindows;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for AddHomeWindow.xaml
    /// </summary>
    public partial class AddHomeWindow : Window
    {
        public PeopleCollection<Person> PersonCollection = null;
        public Home newHome = new Home();
        public Owner anOwner = new Owner();
        public string AddType { get; set; }
        public static Person NewPersonAddedToCollection { get; set; }

        public AddHomeWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
            string addType = AddType;
            MainWindow.peopleCollection.listOfHandlers += AlertPersonAddedToCollection;
            statusBarText.Text = $"Add a new { addType } to the database.";
        }

        public void AlertPersonAddedToCollection(Person p)
        {
            NewPersonAddedToCollection = p;
            RefreshData();
        }

        private void addNewHome_Button(object sender, RoutedEventArgs e)
        {
            RefreshData();

            Home newHome = null;
            //  TODO: Validate these fields right away
            string address = this.homeAddressTextbox.Text.Trim();
            string city = this.homeAddressTextbox.Text.Trim();
            string state = this.homeAddressTextbox.Text.Trim();
            string zip = this.homeAddressTextbox.Text.Trim();


            //  If owner is in the combobox:
            //      OwnerID == PersonID of the selected Person
            //      No need to create a new Owner just get Owner using PersonID/OwnerID then add to newHome
            if (NewPersonAddedToCollection == null)
            {
                //  Owner is selected in the ComboBox so get the data
                var cbo = (ComboBox)ownersComboBox.SelectionBoxItem;
                Person comboBoxPerson = null;
                comboBoxPerson = cbo.SelectedItem as Person;
                var ownerID = comboBoxPerson.PersonID;
                
                //  get existing Owner instance
                var newHomeExistingOwner = (from h in MainWindow.homesCollection
                                            where h.Owner.OwnerID == ownerID
                                            select h.Owner).FirstOrDefault();
                
                //  create the new Home instance
                newHome = new Home()
                {
                    Address = address,
                    City = city,
                    State = state,
                    Zip = zip,
                    Owner = newHomeExistingOwner
                };
            }

            //  If owner is NOT in the combobox:
            //      OwnerID doesn't matter because user will create a new Person via AddPersonWindow and will include Preferred Lender
            //      New Person would have been added to the peopleCollection and notification includes the added instance named NewPersonAddedToCollection
            //      So just add a new home and tie it to the Owner via PersonID of NewPersonAddedToCollection
            if (NewPersonAddedToCollection != null)
            {
                int ownerId = NewPersonAddedToCollection.PersonID;
                //  create the new Home instance
                newHome = new Home()
                {
                    Address = address,
                    City = city,
                    State = state,
                    Zip = zip,
                    OwnerID = ownerId
                };
            }

            if (newHome != null)
            {
                MainWindow.homesCollection.Add(newHome);
                DisplayStatusMessage("Added new Home to database.");
            }
            else
            {
                DisplayStatusMessage("Home not created. Ensure required fields are completed.");
            }
        }

        private void addOwnerButton_click(object sender, RoutedEventArgs e)
        {
            AddPersonWindow apw = new AddPersonWindow();
            apw.AddType = "Owner";
            apw.Show();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshScreenElements();
        }

        private void menuRefreshAll_Click(object sender, RoutedEventArgs e)
        {
            //  refresh screen elements and data in local collection and ComboBox
            RefreshScreenElements();
            RefreshData();
        }

        private void RefreshScreenElements()
        {
            homeAddressTextbox.Text = string.Empty;
            homeCityTextbox.Text = string.Empty;
            homeStateTextbox.Text = string.Empty;
            homeZipTextbox.Text = string.Empty;
            ownersComboBox.SelectedIndex = -1;
            statusBarText.Text = "Refreshed screen.";
        }

        public void RefreshData()
        {
            var existingOwnersList = from pc in MainWindow.peopleCollection
                                     where pc.Owner != null
                                     select pc;


            ownersComboBox.ItemsSource = (from p in existingOwnersList
                                          select p);
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

    }
}
