using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for UpdaterWindow.xaml
    /// </summary>
    public partial class UpdaterWindow : Window
    {
        private bool IsButtonClose { get; set; }
        
        public string UpdateType { get; set; }
        public Person UpdatePerson { get; set; }
        public Home UpdateHome { get; set; }
        public RealEstateCompany UpdateReco { get; set; }
        public HomeSale UpdateHomeSale { get; set; }
        public Agent UpdateAgent { get; set; }
        public Owner UpdateOwner { get; set; }
        public Buyer UpdateBuyer { get; set; }
                
        public UpdaterWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsButtonClose)
            {
                e.Cancel = false;

                try
                {
                    //  Reload collections
                    MainWindow.InitPeopleCollection();
                    MainWindow.InitHomesCollection();
                    MainWindow.InitRealEstateCompaniesCollection();
                }
                catch (Exception ex)
                {
                    var userResponse = MessageBox.Show("Save changes failed. Close anyway?", "Something went wrong!", MessageBoxButton.YesNo);
                    if (userResponse == MessageBoxResult.No)
                    {
                        IsButtonClose = false;
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Use the Close button or File -> Exit menut item. You will be prompted to save or discard changes before exiting.", "Warning!", MessageBoxButton.OK);
                IsButtonClose = false;
                e.Cancel = true;
            }

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if (UpdatePerson != null)
            {
                count++;
            }
            if (UpdateHome != null)
            {
                count++;
            }
            if (UpdateReco != null)
            {
                count++;
            }
            if (UpdateHomeSale != null)
            {
                count++;
            }
            if (UpdateAgent != null)
            {
                count++;
            }
            if (UpdateBuyer != null)
            {
                count++;
            }
            if (UpdateOwner != null)
            {
                count++;
            }
            if (count > 0)
            {
                LoadPanelsAndFields();
            }
            else
            {
                ShowCloseButtonOnly();
            }
        }

        private void LoadPersonPanel()
        {
            personIdTextbox.Text = UpdatePerson.PersonID.ToString();
            personFirstNameTextbox.Text = UpdatePerson.FirstName;
            personLastNameTextbox.Text = UpdatePerson.LastName;
            personPhoneTextbox.Text = UpdatePerson.Phone;
            personEmailTextbox.Text = UpdatePerson.Email;
            updatePersonPanel.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Hidden;
        }

        private void LoadHomePanel()
        {
            homeIdTextBox.Text = UpdateHome.HomeID.ToString();
            homeAddressTextbox.Text = UpdateHome.Address;
            homeCityTextbox.Text = UpdateHome.City;
            homeStateTextbox.Text = UpdateHome.State;
            homeZipTextbox.Text = UpdateHome.Zip;
            //  Note: Could add OwnerID field here but users should not need it
            updateHomePanel.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Hidden;
        }

        private void LoadRECoPanel()
        {
            companyIdTextBox.Text = UpdateReco.CompanyID.ToString();
            companyNameTextbox.Text = UpdateReco.CompanyName;
            companyPhoneTextbox.Text = UpdateReco.Phone;
            updateRealEstateCoPanel.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Hidden;
        }

        private void LoadHomeSalesPanel()
        {

            closeButton.Visibility = Visibility.Hidden;
        }

        private void LoadAgentPanel()
        {
            updateAgentPanel.Visibility = Visibility.Visible;
            updateAgentAgentIdTextbox.Text = UpdateAgent.AgentID.ToString();
            updateAgentAgentPersonNameTextbox.Text = $"{UpdatePerson.FirstName} {UpdatePerson.LastName}";
            updateAgentCompanyIdTextbox.Text = UpdateAgent.CompanyID.ToString();
            updateAgentCommissionTextbox.Text = UpdateAgent.CommissionPercent.ToString();
            var listOfHomesalesAgents = (from hs in MainWindow.homeSalesCollection
                                         from a in MainWindow.peopleCollection
                                         where a.PersonID == hs.AgentID
                                         select a).ToList();
            //  TODO: Fix the output so it displays data in the combobox instead of entity wrappers
            listOfExistingAgentsCombobox.ItemsSource = listOfHomesalesAgents;
            closeButton.Visibility = Visibility.Visible;
        }

        private void ShowCloseButtonOnly()
        {
            updatePersonPanel.Visibility = Visibility.Collapsed;
            updateHomePanel.Visibility = Visibility.Collapsed;
            updateRealEstateCoPanel.Visibility = Visibility.Collapsed;
            closeButton.Visibility = Visibility.Visible;
        }

        private void LoadPanelsAndFields()
        {
            closeButton.Visibility = Visibility.Visible;
            string updateType = UpdateType.Trim().ToUpper();
            switch (updateType)
            {
                case "PERSON":
                    {
                        LoadPersonPanel();
                        break;
                    }
                case "HOME":
                    {
                        LoadHomePanel();
                        break;
                    }
                case "REALESTATECOMPANY":
                    {
                        LoadRECoPanel();
                        break;
                    }
                case "HOMESALE":
                    {
                        LoadHomeSalesPanel();
                        break;
                    }
                case "OWNER":
                    {
                        break;
                    }
                case "BUYER":
                    {
                        break;
                    }
                case "AGENT":
                    {
                        LoadAgentPanel();
                        break;
                    }
                default:
                    {
                        ShowCloseButtonOnly();
                        break;
                    }
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void UpdateChangedPersonFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            //  1)  Validate changed input(s)
            string fName = personFirstNameTextbox.Text.ToString().Trim();
            string lName = personLastNameTextbox.Text.ToString().Trim();
            string phone = personPhoneTextbox.Text.ToString().Trim();
            string email = personEmailTextbox.Text.ToString().Trim();

            if (string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(lName) || string.IsNullOrWhiteSpace(phone))
            {
                //  display nuguduh message
                DisplayStatusMessage("Make sure to fill in required fields");
                return; //  TODO: Test that return in a void method actually breaks out right then and there
            }

            var comparisonPerson = new Person()
            {
                FirstName = fName,
                LastName = lName,
                Phone = phone,
                Email = email
            };

            //  2)  Update object instance with change(s)
            if (UpdatePerson.Equals(comparisonPerson))
            {
                DisplayStatusMessage("No changes detected.");
                return;
            }

            //  3)  Store changes in the DB via entities helper
            if (LogicBroker.SaveEntity<Person>(UpdatePerson))
            {
                DisplayStatusMessage("Changes saved!");
            }
            else
            {
                DisplayStatusMessage("Unable to save changes. Fill all required fields.");
            }
        }

        private void UpdateChangedRecoFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            if (LogicBroker.SaveEntity<RealEstateCompany>(UpdateReco))
            {
                DisplayStatusMessage("Changes saved!");
            }
            else
            {
                DisplayStatusMessage("Unable to save changes. Fill all required fields.");
            }

        }

        private void UpdateChangedHomeFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            if (LogicBroker.SaveEntity<Home>(UpdateHome))
            {
                DisplayStatusMessage("Changes saved!");
            }
            else
            {
                DisplayStatusMessage("Unable to save changes. Fill all required fields.");
            }

        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        private void UpdateChangedHomeSalesFieldsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateChangedAgentFieldsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listOfExistingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
