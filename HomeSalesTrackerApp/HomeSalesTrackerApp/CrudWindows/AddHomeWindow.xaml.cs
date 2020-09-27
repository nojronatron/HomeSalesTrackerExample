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
        private bool IsButtonClose = false;
        public bool UpdateInsteadOfAdd = false; //  Menu Update Home will set this to true to enable Address and Owner update (instead of New Home)
        public Home NewHome { get; set; }
        public Owner AnOwner { get; set; }
        public Person APerson { get; set; }
        public string AddType { get; set; }
        //public static Person NewPersonAddedToCollection { get; set; }

        public AddHomeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (UpdateInsteadOfAdd)
            {
                statusBarText.Text = $"Update this home's address or Owner";
                this.homeAddressTextbox.Text = NewHome.Address.Trim();
                this.homeCityTextbox.Text = NewHome.City.Trim();
                this.homeStateTextbox.Text = NewHome.State.Trim();
                this.homeZipTextbox.Text = NewHome.Zip.Trim();
            }
            else 
            { 
                statusBarText.Text = $"Add a new { this.AddType } to the database.";
            }

            //MainWindow.peopleCollection.listOfHandlers += AlertPersonAddedToCollection;
            RefreshOwnersComboBox();
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
                bool saveSucceeded = false;
                if (APerson != null)
                {
                    if (UpdateInsteadOfAdd)
                    {
                        //  the HomeID would not have changed
                        NewHome.Address = address;
                        NewHome.City = city;
                        NewHome.State = state;
                        NewHome.Zip = zip;
                        NewHome.OwnerID = AnOwner.OwnerID;
                        saveSucceeded = LogicBroker.UpdateEntity<Home>(NewHome);
                    }
                    else 
                    {
                        //  TODO: AddNewHome with existing Person that was not an Owner before saves a NEW PERSON instance, otherwise works fine. TShoot and fix this.
                        //if (LogicBroker.UpdateEntity<Home>(NewHome))
                        NewHome = new Home()
                        {
                            //  HomeID does not matter when adding a new Home
                            Address = address,
                            City = city,
                            State = state,
                            Zip = zip,
                            Owner = AnOwner
                        };
                        saveSucceeded = LogicBroker.SaveEntity<Home>(NewHome);
                    }
                    if (saveSucceeded)
                    {
                        IsButtonClose = true;
                        DisplayStatusMessage("New Home saved! You can now close this window.");
                        MainWindow.InitializeCollections();
                        APerson = null;
                        NewHome = null;
                        AnOwner = null;
                    }
                    else
                    {
                        DisplayStatusMessage("Unable to save Home.");
                        IsButtonClose = false;
                    }
                }
                else
                {
                    DisplayStatusMessage("Be sure to select an Owner before saving this home.");
                    IsButtonClose = false;
                }

            }

        }

        private void AddOwnerButton_click(object sender, RoutedEventArgs e)
        {
            var Apw = new AddPersonWindow();
            Apw.AddType = "Owner";
            Apw.Title = "Add new Owner to the Database";
            Apw.Show();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
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

        private void ClearScreenElements()
        {
            homeAddressTextbox.Text = string.Empty;
            homeCityTextbox.Text = string.Empty;
            homeStateTextbox.Text = string.Empty;
            homeZipTextbox.Text = string.Empty;
            PotentialOwnerPeopleCombobox.SelectedIndex = -1;
            statusBarText.Text = "Cleared all entries.";
        }

        public void RefreshOwnersComboBox()
        {
            MainWindow.InitializeCollections();
            var existingOwnersList = (from p in MainWindow.peopleCollection
                                      select p).ToList();

            int selectedIndex = 0;
            existingOwnersList = existingOwnersList.Distinct().ToList();
            if (APerson != null)
            {
                selectedIndex = existingOwnersList.FindIndex(p => p.PersonID == APerson.PersonID);
                if (APerson != null && selectedIndex < 0)
                {
                    existingOwnersList.Add(APerson);
                    selectedIndex = existingOwnersList.Count - 1;
                }

            }

            PotentialOwnerPeopleCombobox.ItemsSource = existingOwnersList;
            PotentialOwnerPeopleCombobox.SelectedIndex = selectedIndex;
            DisplayStatusMessage("Refreshed Owners list for display.");
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        /// <summary>
        /// When a user selects a Person in the ComboBox it should return a hydrated Person and Owner even if Owner is null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PotentialOwnerPeopleCombobox_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            //RefreshOwnersComboBox();
            var tempOwnerPerson = new Person();
            tempOwnerPerson = (sender as ComboBox).SelectedItem as Person;
            APerson = new Person();
            APerson = MainWindow.peopleCollection.Where(p => p.FirstName == tempOwnerPerson.FirstName && p.LastName == tempOwnerPerson.LastName).FirstOrDefault();
            if (AnOwner == null)
            {
                AnOwner = new Owner();
                AnOwner.Person = APerson;

                var ownersHome = MainWindow.homesCollection.Where(h => h.OwnerID == tempOwnerPerson.PersonID).FirstOrDefault();
                AnOwner = ownersHome.Owner;
                DisplayStatusMessage("New Owner and Preferred Lender selected. Click Add New Home to save.");
            }
            if (APerson.Owner != null)
            {
                AnOwner = APerson.Owner;
                PreferredLenderTextbox.Text = APerson.Owner.PreferredLender;
                DisplayStatusMessage("New Owner and Preferred Lender selected. Click Add New Home to save.");
            }
            else
            {
                PreferredLenderTextbox.Text = "Enter Preferred Lender then click Add.";
                PreferredLenderTextbox.IsReadOnly = false;
                AddPreferredLenderButton.IsEnabled = true;
                DisplayStatusMessage("New Owner selected. Enter Prefered Lender info.");
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsButtonClose)
            {
                e.Cancel = false;
                MainWindow.InitializeCollections();
            }
            else
            {
                var userResponse = MessageBox.Show("Closing Add Home Window", "Changes will not be saved. Continue?", MessageBoxButton.YesNo);
                if (userResponse == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void AddPreferredLenderButton_Click(object sender, RoutedEventArgs e)
        {
            string tempPreferredLender = PreferredLenderTextbox.Text.Trim();
            if(tempPreferredLender.Length > 2)
            {
                AnOwner.PreferredLender = tempPreferredLender;
                DisplayStatusMessage("Preferred Lender added.");
            }
        }
    }
}
