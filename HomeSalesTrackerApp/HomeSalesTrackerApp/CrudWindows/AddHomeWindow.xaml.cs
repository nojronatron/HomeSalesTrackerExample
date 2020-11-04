using HomeSalesTrackerApp.CrudWindows;
using HSTDataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for AddHomeWindow.xaml
    /// </summary>
    public partial class AddHomeWindow : Window
    {
        private bool IsButtonClose = false;
        //  Note: Menu Update Home will set this to true to enable Address and Owner update (instead of New Home)
        public bool UpdateInsteadOfAdd = false;
        public Home NewHome { get; set; }
        public Owner AnOwner { get; set; }
        public Person APerson { get; set; }
        public string AddType { get; set; }

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

            RefreshOwnersComboBox();
        }

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
                int itemsProcessed = 0;
                if (APerson != null)
                {
                    if (UpdateInsteadOfAdd)
                    {
                        NewHome.Address = address;
                        NewHome.City = city;
                        NewHome.State = state;
                        NewHome.Zip = zip;
                        NewHome.OwnerID = APerson.PersonID;
                        itemsProcessed += MainWindow.homesCollection.Update(NewHome);
                    }
                    else
                    {
                        itemsProcessed += MainWindow.peopleCollection.UpdatePerson(APerson);
                        NewHome = new Home()
                        {
                            Address = address,
                            City = city,
                            State = state,
                            Zip = zip,
                            OwnerID = APerson.PersonID
                        };

                        itemsProcessed += MainWindow.homesCollection.Add(NewHome);
                    }

                    if (itemsProcessed > 0)
                    {
                        IsButtonClose = true;
                        DisplayStatusMessage("New Home saved! You can now close this window.");
                        APerson = null;
                        NewHome = null;
                        AnOwner = null;
                    }
                    else
                    {
                        DisplayStatusMessage("Unable to save Home. Close this window and try again.");
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

            int selectedIndex = -1;
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
            var tempOwnerPerson = (sender as ComboBox).SelectedItem as Person;
            PreferredLenderTextbox.IsReadOnly = false;
            addOwnerButton.IsEnabled = false;
            AddPreferredLenderButton.IsEnabled = false;
            int personID = tempOwnerPerson.PersonID;
            APerson = MainWindow.peopleCollection.Where(p => p.PersonID == personID).FirstOrDefault();

            if (APerson.Owner != null)
            {
                List<Home> homesOwnedByPerson = (from h in MainWindow.homesCollection
                                                 where h.OwnerID == personID
                                                 select h).ToList();

                if (homesOwnedByPerson.Count > 0)
                {
                    AnOwner = (from o in MainWindow.homesCollection
                               where o.OwnerID == personID
                               select o.Owner).FirstOrDefault();

                }
                else
                {
                    AnOwner = (from o in MainWindow.peopleCollection
                               where o.PersonID == personID
                               select o.Owner).FirstOrDefault();
                }

                var preferredLender = AnOwner.PreferredLender.Trim();
                PreferredLenderTextbox.Text = preferredLender;
                PreferredLenderTextbox.IsReadOnly = true;
                addOwnerButton.IsEnabled = true;
                DisplayStatusMessage("New Owner and Preferred Lender selected. Click Add New Home to save.");
            }
            else
            {
                PreferredLenderTextbox.IsReadOnly = false;
                PreferredLenderTextbox.Text = "Enter Preferred Lender here.";
                AnOwner = new Owner()
                {
                    OwnerID = personID
                };
                DisplayStatusMessage("Enter new Preferred Lender and then click Add button.");
                AddPreferredLenderButton.IsEnabled = true;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (IsButtonClose)
            {
                e.Cancel = false;
                //MainWindow.InitializeCollections();
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
            if (tempPreferredLender.Length > 2)
            {
                AnOwner.PreferredLender = tempPreferredLender;
                DisplayStatusMessage("Preferred Lender added.");
            }
        }

    }
}
