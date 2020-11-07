using HomeSalesTrackerApp.CrudWindows;
using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for AddHomeWindow.xaml
    /// </summary>
    public partial class AddHomeWindow : Window, IObserver<NotificationData>
    {
        private bool IsButtonClose = false;
        //  Note: Menu Update Home will set this to true to enable Address and Owner update (instead of New Home)
        private CollectionMonitor collectionMonitor = null;
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

            collectionMonitor = MainWindow.peopleCollection.collectionMonitor;
            collectionMonitor.Subscribe(this);
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
                if (1 > address.Length || address.Length > 50 || 1> city.Length || city.Length > 30 ||
                    1 > state.Length || state.Length > 2 || 1 > zip.Length || zip.Length > 9)
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
                        AnOwner.Homes.Add(NewHome);
                        MainWindow.peopleCollection.UpdateOwner(AnOwner);

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


        public void RefreshOwnersComboBox()
        {
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

            if (tempOwnerPerson == null)
            {
                DisplayStatusMessage("An error occurred. Unable to display selected person.");
                PreferredLenderTextbox.Text = string.Empty;
                return;
            }

            APerson = MainWindow.peopleCollection.Where(p => p.PersonID == tempOwnerPerson.PersonID).FirstOrDefault();
            AnOwner = APerson.Owner;
            PreferredLenderTextbox.Text = AnOwner.PreferredLender ?? "Lender info not found";
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (IsButtonClose)
            {
                e.Cancel = false;
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

        #region IObserver implementation

        private IDisposable unsubscriber;
        private string notificationMessage;

        public virtual void Subscribe(IObservable<NotificationData> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        public void OnNext(NotificationData value)
        {
            if (value.ChangeCount > 0 && value.DataType.Contains("Person"))
            {
                RefreshOwnersComboBox();
                notificationMessage = "Received an update to the Owners Combo Box.";
            }

            else
            {
                notificationMessage = "Received a message with no applicable changes.";
            }

        }

        public void OnError(Exception error)
        {
            //  do nothing
            ;
        }

        public void OnCompleted()
        {
            DisplayStatusMessage(notificationMessage);
            notificationMessage = "No new Notifications";
        }

        #endregion

    }
}
