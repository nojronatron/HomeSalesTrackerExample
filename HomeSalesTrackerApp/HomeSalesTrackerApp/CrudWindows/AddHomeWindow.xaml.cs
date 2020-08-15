﻿using HomeSalesTrackerApp.CrudWindows;
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
            //  Address, City, State, and Zip fields are validated and an existing Owner in the ComboBox will be attached to the new Home.
            NewHome = null; //  must be initialized as null to test for null later

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
                Person tempPerson = ownersComboBox.SelectedItem as Person;
                if (tempPerson != null)
                {
                    var selectedOwnerPerson = (from p in MainWindow.peopleCollection
                                               where p.FirstName == tempPerson.FirstName &&
                                               p.LastName == tempPerson.LastName
                                               select p).FirstOrDefault();
                    NewHome = new Home()
                    {
                        Address = address,
                        City = city,
                        State = state,
                        Zip = zip,
                        OwnerID = selectedOwnerPerson.PersonID,
                        Owner = selectedOwnerPerson.Owner
                    };

                    if (LogicBroker.SaveEntity<Home>(NewHome))
                    {
                        isButtonClose = true;
                        DisplayStatusMessage("New Home saved! You can now close this window.");
                        MainWindow.InitializeCollections();
                        APerson = null;
                        NewHome = null;
                        AnOwner = null;
                    }
                    else
                    {
                        DisplayStatusMessage("Unable to save Home.");
                        isButtonClose = false;
                    }
                }
                else
                {
                    DisplayStatusMessage("Be sure to select an Owner before saving this new home.");
                    isButtonClose = false;
                }
               
            }

        }

        private void AddOwnerButton_click(object sender, RoutedEventArgs e)
        {
            PersonUpdaterWindow personUpdaterWindow = new PersonUpdaterWindow();
            personUpdaterWindow.ReceivedAgent = new Agent();
            personUpdaterWindow.ReceivedBuyer = new Buyer();
            personUpdaterWindow.ReceivedOwner = new Owner();
            personUpdaterWindow.ReceivedPerson = new Person();
            personUpdaterWindow.CalledByUpdateMenuType = "Owner";
            personUpdaterWindow.Show();
        }

        //private void AddAgentButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //PersonUpdaterWindow personUpdaterWindow = new PersonUpdaterWindow();
        //    //personUpdaterWindow.UpdateAgent = new Agent();
        //    //personUpdaterWindow.UpdateBuyer = new Buyer();
        //    //personUpdaterWindow.UpdateOwner = new Owner();
        //    //personUpdaterWindow.UpdatePerson = new Person();
        //    //personUpdaterWindow.CalledByUpdateMenuType = "Buyer";
        //    //personUpdaterWindow.Show();
        //}

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
            MainWindow.InitializeCollections();

            //var existingOwnersList = (from p in MainWindow.peopleCollection
            //                          from h in MainWindow.homesCollection
            //                          where p.PersonID == h.OwnerID
            //                          select p).ToList();
            var existingOwnersList = (from p in MainWindow.peopleCollection
                                      where p.Owner != null
                                      select p).ToList();

            if (APerson == null)
            {
                ownersComboBox.ItemsSource = existingOwnersList;
            }
            else
            {
                existingOwnersList.Add(APerson);
            }

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
                MainWindow.InitializeCollections();
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
            isButtonClose = true;
            this.Close();
        }
    }
}
