using HomeSalesTrackerApp.CrudWindows;
using HSTDataLayer;
using HSTDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private bool isButtonClose = false;

        public static Home NewHome { get; set; }
        public static Owner AnOwner { get; set; }
        public static Person APerson { get; set; }
        public string AddType { get; set; }
        //public static Person NewPersonAddedToCollection { get; set; }

        public AddHomeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshOwnersComboBox();
            string addType = AddType;
            //MainWindow.peopleCollection.listOfHandlers += AlertPersonAddedToCollection;
            statusBarText.Text = $"Add a new { addType } to the database.";
        }

        //public void AlertPersonAddedToCollection(Person p)
        //{
        //    NewPersonAddedToCollection = p;
        //    //  TODO: After Golden Paths completed look at attempting to implement this notification regime.
        //    //LogicBroker.SaveEntity<Person>(p);
        //    RefreshOwnersComboBox();
        //}

        private void AddNewHomeButton_Click(object sender, RoutedEventArgs e)
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
                Person comboBoxPerson = null;
                comboBoxPerson = ownersComboBox.SelectedItem as Person;
                if (comboBoxPerson != null)
                {
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

                if (comboBoxPerson == null && APerson != null)
                {
                    int ownerId = APerson.PersonID;
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
                    isButtonClose = false;
                }
                else
                {
                    SaveHomeAndUpdateCollection(newHome);
                    isButtonClose = true;
                    DisplayStatusMessage("New Home saved! You can now close this window.");
                    APerson = null;
                    NewHome = null;
                    AnOwner = null;
                    closeButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void AddOwnerButton_click(object sender, RoutedEventArgs e)
        {
            PersonAddUpdateWindow pauw = new PersonAddUpdateWindow();
            pauw.UpdateAgent = new Agent();
            pauw.UpdateBuyer = new Buyer();
            pauw.UpdateOwner = new Owner();
            pauw.UpdatePerson = new Person();
            pauw.UpdateType = "Owner";
            pauw.Show();
            RefreshOwnersComboBox();
        }

        private void AddAgentButton_Click(object sender, RoutedEventArgs e)
        {
            PersonAddUpdateWindow pauw = new PersonAddUpdateWindow();
            pauw.UpdateAgent = new Agent();
            pauw.UpdateBuyer = new Buyer();
            pauw.UpdateOwner = new Owner();
            pauw.UpdatePerson = new Person();
            pauw.UpdateType = "Buyer";
            pauw.Show();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            isButtonClose = true;
            this.Close();
        }

        private void MenuClearInputs_Click(object sender, RoutedEventArgs e)
        {
            ClearScreenElements();
        }

        private void MenuReloadOwners_Click(object sender, RoutedEventArgs e)
        {
            RefreshOwnersComboBox();
        }

        public void AddPersonLoadOwnerToAddHomeComboBox()
        { 

            var checkPerson = MainWindow.peopleCollection.FirstOrDefault(p => p.FirstName == APerson.FirstName && p.LastName == APerson.LastName);
            if (APerson != null)    //  user created a new person and it might/not be in collection so deal with it then display JUST THE NEW PERSON in the combo box and select it
            {
                if (checkPerson == null)
                {
                    LogicBroker.SaveEntity<Person>(APerson);
                    MainWindow.peopleCollection = new PeopleCollection<Person>(EntityLists.GetListOfPeople());
                    checkPerson = MainWindow.peopleCollection.FirstOrDefault(p => p.FirstName == APerson.FirstName && p.LastName == APerson.LastName);
                }
                if (checkPerson != null)
                {
                    var newOwnerCreated = (from p in MainWindow.peopleCollection
                                           where p.FirstName == APerson.FirstName
                                           select p).ToList();
                    ownersComboBox.ItemsSource = newOwnerCreated;
                    ownersComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                //  refresh ComboBox with existing owner instances because user did NOT just create a new Person
                RefreshOwnersComboBox();
                ownersComboBox.SelectedIndex = -1;
            }
            DisplayStatusMessage("Owners List refreshed with latest data.");
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
            MainWindow.peopleCollection = new PeopleCollection<Person>( EntityLists.GetListOfPeople() );
            MainWindow.homesCollection = new HomesCollection(EntityLists.GetTreeListOfHomes());
            
            var existingOwnersList = (from p in MainWindow.peopleCollection
                                      from h in MainWindow.homesCollection
                                      where p.PersonID == h.OwnerID
                                      select p).ToList();

            if (APerson != null)
            {
                existingOwnersList.Add(APerson);
            }

            ownersComboBox.ItemsSource = existingOwnersList;
            DisplayStatusMessage("Refreshed Owners list for display.");
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        private void SaveHomeAndUpdateCollection(Home h)
        {
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

        private void OwnerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayStatusMessage("Selecting an existing owner.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isButtonClose)
            {
                e.Cancel = false;

                try
                {
                    SaveHomeAndUpdateCollection(NewHome);
                }
                catch (Exception ex)
                {
                    var userResponse = MessageBox.Show("Save changes failed. Close anyway?", "Something went wrong!", MessageBoxButton.YesNo);
                    if (userResponse == MessageBoxResult.No)
                    {
                        isButtonClose = false;
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                var userResponse = MessageBox.Show("Save changes?", "Closing Add Home Window", MessageBoxButton.YesNo);
                if (userResponse == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveHomeAndUpdateCollection(NewHome);
                    }
                    catch (Exception ex)
                    {
                        userResponse = MessageBox.Show("Save changes failed. Close anyway?", "Something went wrong!", MessageBoxButton.YesNo);
                        if (userResponse == MessageBoxResult.No)
                        {
                            isButtonClose = false;
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isButtonClose = true;
            this.Close();
        }
    }
}
