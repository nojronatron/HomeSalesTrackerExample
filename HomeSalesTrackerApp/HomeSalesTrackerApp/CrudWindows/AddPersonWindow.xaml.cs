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
using System.Windows.Shapes;
using HSTDataLayer;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window
    {
        private Agent newAgent = null;
        private Buyer newBuyer = null;
        private Owner newOwner = null;
        private Person newPerson = null;
        public string AddType { get; set; }
        public AddPersonWindow()
        {
            InitializeComponent();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            //  TODO: Add validation, preferredLender is not required but limited to 30 chars
            string preferredLenderText = this.preferredLenderTextbox.Text.Trim();

            if (preferredLenderText.Length == 0)
            {
                preferredLenderText = string.Empty; //  ensures an empty field rather than a special or control character that could be unsafe
            }
            if (preferredLenderText.Length < 31)
            {
                newOwner = new Owner()
                {
                    PreferredLender = this.preferredLenderTextbox.Text.Trim()
                };

                string createNewPersonErrorMessages = CreateNewPerson();
                if (newPerson != null)
                {
                    newPerson.Owner = newOwner;
                    DisplayStatusMessage("Added new Owner.");
                    UpdatePersonCollection(newPerson);
                }
                else
                {
                    newPerson = null;   //  explicitly reset newPerson to null to ensure it does not get reused
                    DisplayStatusMessage(createNewPersonErrorMessages);
                }
            }
            if (preferredLenderText.Length < 1)
            {
                DisplayStatusMessage("Preferred Lender Name is too long for this database. Please shorten it and try again.");
            }
        }

        private void UpdatePersonCollection(Person p)
        {
            //  TODO: Verify updatePersonCollection(Person p) saves the new entity then refreshes the peopleCollection as expected
            if (LogicBroker.SaveEntity<Person>(p))
            {
                MainWindow.InitPeopleCollection();
                AddHomeWindow ahw = new AddHomeWindow();
                ahw.RefreshOwnersComboBox();
            }
            else
            {
                DisplayStatusMessage("Unable to add Owner to the database.");
            }
        }

        private void addBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            string credRating = this.creditRatingTextbox.Text.Trim();
            newBuyer = new Buyer()
            {
                CreditRating = int.Parse(credRating)
            };
            
            string createNewPersonErrorMessage = CreateNewPerson();
            if (newPerson != null)
            {
                newPerson.Buyer = newBuyer;
                DisplayStatusMessage("Added new Buyer.");
                UpdatePersonCollection(newPerson);
            }
            else
            {
                newPerson = null;   //  explicitly reset newPerson to null to ensure it does not get reused
                DisplayStatusMessage(createNewPersonErrorMessage);
            }
        }

        private void addAgentButton_Click(object sender, RoutedEventArgs e)
        {
            string commission = this.commissionTextbox.Text.Trim();
            newAgent = new Agent()
            {
                CommissionPercent = Decimal.Parse(commission)
            };

            string createNewPersonErrorMessage = CreateNewPerson();
            if (newPerson != null)
            {
                newPerson.Agent = newAgent;
                UpdatePersonCollection(newPerson);
                DisplayStatusMessage("Added new Agent.");
            }
            else
            {
                newPerson = null;   //  explicitly reset newPerson to null to ensure it does not get re-used
                DisplayStatusMessage(createNewPersonErrorMessage);
            }
        }

        private string CreateNewPerson()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Missing required field(s) ");
            
            //  TODO: confirm the null coalescing operator used on email string is properly implemented
            string firstName = this.fNameTextbox.Text.Trim();
            string lastName = this.lNameTextbox.Text.Trim();
            string phone = this.phoneTextbox.Text.Trim();
            string email = this.emailTextbox?.Text.Trim() ?? string.Empty;

            if (firstName.Length < 1)
            {
                result.Append("First Name ");
            }
            if (lastName.Length < 1)
            {
                DisplayStatusMessage("Last Name ");
            }
            if (phone.Length < 1)
            {
                DisplayStatusMessage("Phone Number ");
            }
            if (result.Length < 19)
            {
                result.Clear();
                newPerson = new Person()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    Email = email
                    //  Owner = newOwner
                };
            }

            return result.ToString();
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        private void AddPersonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  TODO: add person window closing: does it need to do anything else besidese just close?
            var userResponse = MessageBox.Show("Save changes?", "Changes not saved!", MessageBoxButton.YesNo);
            if (userResponse == MessageBoxResult.Yes)
            {
                SaveToEntities();
            }
            this.Close();
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //  DONT save stuff on exit because exiting != to wanting to save changes
            //SaveToEntities();
            //MainWindow.InitPeopleCollection();
            DisplayStatusMessage("Closing.");
            this.Close();
        }

        private void SaveToEntities()
        {
            if (newAgent != null)
            {
                LogicBroker.SaveEntity<Person>(newPerson);
            }
            if (newBuyer != null)
            {
                LogicBroker.SaveEntity<Person>(newPerson);
            }
            if (newOwner != null)
            {
                LogicBroker.SaveEntity<Person>(newPerson);
            }
        }
    }
}
