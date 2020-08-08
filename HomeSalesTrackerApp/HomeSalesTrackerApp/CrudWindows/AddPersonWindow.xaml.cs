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
        private bool isButtonClose = false;
        private Agent newAgent = null;
        private Buyer newBuyer = null;
        private Owner newOwner = null;
        private Person newPerson = null;
        public string AddType { get; set; }

        public AddPersonWindow()
        {
            InitializeComponent();
            this.Title = "wowzers";
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
                DisplayStatusMessage("Preferred Lender will be blank (no preferred lender).");
                preferredLenderText = string.Empty; //  ensures an empty field rather than a special or control character that could be unsafe
            }

            if (preferredLenderText.Length > 31)
            {
                DisplayStatusMessage("Preferred Lender Name is too long for this database. Trimmed-down to 31 characters.");
                preferredLenderText = preferredLenderText.Substring(0, 30);
            }

            newOwner = new Owner()
            {
                PreferredLender = preferredLenderText
            };

            string createNewPersonErrorMessages = CreateNewPerson();

            if (newPerson != null)
            {
                newPerson.Owner = newOwner;
                DisplayStatusMessage("Added a new Owner.");
            }
            else
            {
                newPerson = null;   //  explicitly reset newPerson to null to ensure it does not get reused
                newOwner = null;
                DisplayStatusMessage(createNewPersonErrorMessages);
            }
        }

        private void AddBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            //  TODO: Add preemptive input validation prior to creating the new Buyer object instance
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
                SavePersonAndRefreshCollection(newPerson);
            }
            else
            {
                newPerson = null;   //  explicitly reset newPerson to null to ensure it does not get reused
                DisplayStatusMessage(createNewPersonErrorMessage);
            }
        }

        private void addAgentButton_Click(object sender, RoutedEventArgs e)
        {
            //  TODO; Add preemptive input validation prior to creating the new Agent object instance
            string commission = this.commissionTextbox.Text.Trim();
            newAgent = new Agent()
            {
                CommissionPercent = Decimal.Parse(commission)
            };

            string createNewPersonErrorMessage = CreateNewPerson();
            if (newPerson != null)
            {
                newPerson.Agent = newAgent;
                SavePersonAndRefreshCollection(newPerson);
                DisplayStatusMessage("Added new Agent.");
            }
            else
            {
                newPerson = null;   //  explicitly reset newPerson to null to ensure it does not get re-used
                DisplayStatusMessage(createNewPersonErrorMessage);
            }
        }

        /// <summary>
        /// Takes a Person instance and saves it to the DB then synchronizes the People Collection with the DB to ensure each instance has an ID.
        /// </summary>
        /// <param name="p"></param>
        private void SavePersonAndRefreshCollection(Person p)
        {
            //  TODO: Verify updatePersonCollection(Person p) saves the new entity then refreshes the peopleCollection as expected
            if (SaveToEntities())
            {
                MainWindow.InitPeopleCollection();
                AddHomeWindow ahw = new AddHomeWindow();
                //ahw.RefreshOwnersComboBox();
                AddHomeWindow.APerson = p;
                ahw.AddPersonLoadOwnerToAddHomeComboBox();
            }
            else
            {
                DisplayStatusMessage("Unable to add Owner to the database.");
            }
        }

        /// <summary>
        /// Updates this.newPerson field with a Person Object containing fields from user input and returns an empty string.
        /// If user input(s) do not pass validation then a null Person is stored in this.newPerson and the validation error(s) are returns as a string.
        /// </summary>
        /// <returns></returns>
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
            if (result.ToString() == "Missing required field(s) ")
            {
                result.Clear();
                newPerson = new Person()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    Email = email,
                };
            }

            return result.ToString();
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        /// <summary>
        /// Save the Person and any Agent, Buyer, or Owner instance, write them to DB, and refresh all four collections.
        /// ONLY do this if the user clicked the Close button AND says YES to saving changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPersonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isButtonClose)
            {
                e.Cancel = false;
                //  TODO: verify this updates the db and the PeopleCollection
                var userResponse = MessageBox.Show("Save changes?", "Changes not saved!", MessageBoxButton.YesNo);
                if (userResponse == MessageBoxResult.Yes)
                {
                    try
                    {
                        SaveToEntities();
                        MainWindow.InitPeopleCollection();
                        MainWindow.InitHomeSalesCollection();
                        MainWindow.InitHomesCollection();
                        AddHomeWindow.APerson = newPerson;
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
            else
            {
                DisplayStatusMessage("You must click the Close button to exit (with or without saving).");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Save the Person and any Agent, Buyer, or Owner object instances, write them to DB, and refresh ALL FOUR collections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAndCloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            isButtonClose = true;
            DisplayStatusMessage("Closing.");
            this.Close();
        }

        /// <summary>
        /// Automates saving a newly created Agent, Buyer, or Owner instance without updating any of the four Collections.
        /// </summary>
        private bool SaveToEntities()
        {
            bool result = false;
            int count = 0;
            if (newAgent != null)
            {
                if (LogicBroker.SaveEntity<Person>(newPerson))
                {
                    count++;
                }
            }
            if (newBuyer != null)
            {
                if (LogicBroker.SaveEntity<Person>(newPerson))
                {
                    count++;
                }
            }
            if (newOwner != null)
            {
                if(LogicBroker.SaveEntity<Person>(newPerson))
                {
                    count++;
                }
            }
            if (count > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
