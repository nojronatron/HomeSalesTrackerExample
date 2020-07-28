using HSTDataLayer;
using HSTDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for CrudWindow.xaml
    /// </summary>
    public partial class CrudWindow : Window
    {
        private static List<string> personTypes = new List<string>() { "Agent", "Owner", "Buyer" };
        public string crudType { get; set; }
        public CrudWindow(string selectedMenuItem)
        {
            InitializeComponent();
            crudType = selectedMenuItem;
            personTypeComboBox.ItemsSource = personTypes;
            personTypeComboBox.SelectedIndex = -1;
            DisplayStatusMessage($"Selected menu item: { crudType }");
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DisplayStatusMessage(string message)
        {
            statusBarText.Text = message.Trim().ToString();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {
            personTypeComboBox.SelectedIndex = -1;
            InitializeComponent();
            DisplayStatusMessage("Refreshed report output.");
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            int personId = -1;
            string address;
            string city;
            string state;
            string zip;
            string preferredLender;
            string personType;
            string firstname;
            string lastname;
            string phone;
            string email;
            //  1)  accept field input(s) and test them for uniqueness
            //      Fields: Home.Address + Home.Zip unique (not in Collection)
            //              Person.FirstName + Person.LastName unique (not in Collection)
            //      If exist(s): Update error/message output label stating item(s) already exist.
            //      If not exist then continue
            //  2)  create new objects:
            //      Home newHome = new Home() => Address; City; State; Zip
            //      Owner newOwner = new Owner() => PreferredLender
            //      Person newPerson = new Person() => FirstName; LastName; Phone; Email?
            //  3)  Submit changes to the underlying database via LogicBroker.cs
            //  4)  Update the collections via EntityLists.cs to get latest from DB
            //  5)  update error/message output label stating item(s) have been updated

            try
            {
                address = homeAddressTextbox.Text.Trim();  //  required
                city = homeCityTextbox.Text.Trim();  //  required
                state = homeStateTextbox.Text.Trim();  //  required
                zip = homeZipTextbox.Text.Trim();  //  required
                preferredLender = preferredLenderTextbox.Text?.Trim();
                personType = personTypeComboBox.SelectedItem.ToString();
                firstname = firstnameTextbox.Text.Trim();   //  required
                lastname = lastnameTextbox.Text.Trim();     //  required
                phone = phoneTextbox.Text?.Trim();
                email = emailTextbox.Text?.Trim();

                Home newHome = MainWindow.homesCollection.FirstOrDefault(h => h.Address == address && h.Zip == zip);
                Person newPerson = MainWindow.peopleCollection.FirstOrDefault(p => p.FirstName == firstname && p.LastName == lastname);
                Person newOwner = MainWindow.peopleCollection.FirstOrDefault(o => o.Owner.PreferredLender == preferredLender);

                if (newHome == null)
                {
                    
                    newPerson = new Person()
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Phone = phone,
                        Email = email,
                        Owner = new Owner() { PreferredLender = preferredLender }
                    };

                    if (MainWindow.peopleCollection.Add(newPerson))
                    {
                        newHome = new Home()
                        {
                            Address = address,
                            City = city,
                            State = state,
                            Zip = zip,
                            OwnerID = personId
                        };
                    };
                    DisplayStatusMessage($"Added new Home and { personType }.");
                }
                else
                {
                    DisplayStatusMessage("Home already exists.");
                    homeAddressTextbox.Text = string.Empty;
                    homeZipTextbox.Text = string.Empty;
                }
            }

            catch (Exception ex)
            {
                //  TODO: fix this so it doesn't spill coding info to the user
                DisplayStatusMessage(ex.Message);
            }

        }
    }
}
