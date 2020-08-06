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
            RefreshOwnersComboBox();
            string addType = AddType;
            MainWindow.peopleCollection.listOfHandlers += AlertPersonAddedToCollection;
            statusBarText.Text = $"Add a new { addType } to the database.";
        }

        public void AlertPersonAddedToCollection(Person p)
        {
            NewPersonAddedToCollection = p;
            //  TODO: After Golden Paths completed look at attempting to implement this notification regime.
            //LogicBroker.SaveEntity<Person>(p);
            RefreshOwnersComboBox();
        }

        private void addNewHome_Button(object sender, RoutedEventArgs e)
        {
            Home newHome = null;

            string address = this.homeAddressTextbox.Text.Trim();
            string city = this.homeCityTextbox.Text.Trim();
            string state = this.homeStateTextbox.Text.Trim();
            string zip = this.homeZipTextbox.Text.Trim();

            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(zip))
            {
                if (address.Length > 50 || city.Length > 30 || state.Length > 2 || zip.Length > 9)
                {
                    DisplayStatusMessage("Home not created. Ensure required fields are completed.");
                }
            }
            else
            {
                if (NewPersonAddedToCollection == null)
                {
                    Person comboBoxPerson = null;
                    comboBoxPerson = ownersComboBox.SelectedItem as Person;
                    var ownerID = comboBoxPerson.PersonID;

                    newHome = new Home()
                    {
                        Address = address,
                        City = city,
                        State = state,
                        Zip = zip,
                        OwnerID = ownerID
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

                if (newHome == null)
                {
                    DisplayStatusMessage("Home not created. Ensure required fields are completed.");
                }
                else
                {
                    if (LogicBroker.SaveEntity<Home>(newHome))
                    {
                        DisplayStatusMessage("Added new Home to database.");

                        //  TODO: Test this solution. HomeID will be required so change homesCollection.Add to fully refresh from DB
                        UpdateHomesCollection(newHome);
                        //MainWindow.homesCollection.Add(newHome);
                    }
                    else
                    {
                        DisplayStatusMessage("Home was not saved to Database.");
                    }
                }
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

        private void menuClearInputs_Click(object sender, RoutedEventArgs e)
        {
            ClearScreenElements();
        }

        private void menuReloadData_Click(object sender, RoutedEventArgs e)
        {
            RefreshOwnersComboBox();
        }

        private void ClearScreenElements()
        {
            homeAddressTextbox.Text = string.Empty;
            homeCityTextbox.Text = string.Empty;
            homeStateTextbox.Text = string.Empty;
            homeZipTextbox.Text = string.Empty;
            ownersComboBox.SelectedIndex = -1;
            statusBarText.Text = "Cleared all entries.";
        }

        public void RefreshOwnersComboBox()
        {
            //var existingOwnersList = from pc in MainWindow.peopleCollection
            //                         where pc.Owner != null
            //                         select pc;

            var existingOwnersList = (from p in MainWindow.peopleCollection
                                      from h in MainWindow.homesCollection
                                      where p.PersonID == h.OwnerID
                                      select p).ToList();

            //ownersComboBox.ItemsSource = (from p in existingOwnersList
            //                              select p).ToList();

            ownersComboBox.ItemsSource = existingOwnersList;
        }

        private void ownersComboBoxOpened(object sender, EventArgs e)
        {
            //  ComboBox did not always should bound data at FormOpened() method so force refresh when user clicks the drop-down arrow
            RefreshOwnersComboBox();
            DisplayStatusMessage("Refreshed Owners list for display.");
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        private void UpdateHomesCollection(Home h)
        {
            //  TODO: Verify updateHomesCollection(Home h) saves the new entity then refreshes the homesCollection as expected
            if (LogicBroker.SaveEntity<Home>(h))
            {
                MainWindow.InitHomesCollection();
                AddHomeWindow ahw = new AddHomeWindow();
                ahw.RefreshOwnersComboBox();
            }
            else
            {
                DisplayStatusMessage("Unable to update database with this home.");
            }
        }

    }
}
