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
        private bool IsButtonClose = false;
        private int NewPersonID = -1;
        private Agent NewAgent = null;
        private Buyer NewBuyer = null;
        private Owner NewOwner = null;
        private Person NewPerson = null;
        private RealEstateCompany ExistingRECo = null;

        public string AddType { get; set; }

        public AddPersonWindow()
        {
            InitializeComponent();
            //Title = $"Add { AddType }";
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRECoComboBox();
            DisplayStatusMessage("Refreshed entries and updates.");
        }

        private void SaveOwnerInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string preferredLenderText = this.PreferredLenderTextbox.Text.Trim();

            if (preferredLenderText.Length == 0)
            {
                DisplayStatusMessage("Preferred Lender will be blank (no preferred lender).");
                preferredLenderText = string.Empty; //  ensures an empty field rather than a special or control character that could be unsafe
            }

            if (preferredLenderText.Length > 31)
            {
                DisplayStatusMessage("Preferred Lender Name will be trimmed-down to 31 characters (max).");
                preferredLenderText = preferredLenderText.Substring(0, 30);
            }

            NewOwner = new Owner()
            {
                PreferredLender = preferredLenderText
            };

            DisplayStatusMessage("New Owner information created.");
        }

        private void SaveBuyerInfoButton_Click(object sender, RoutedEventArgs e)
        {
            NewBuyer = new Buyer() { };
            string credRating = this.CreditRatingTextbox.Text.Trim();
            if (credRating.Length > 0)
            {
                if (int.TryParse(credRating, out int creditRating))
                {
                    NewBuyer.CreditRating = creditRating;
                    DisplayStatusMessage("Added new Buyer.");
                }
            }
            else
            {
                NewBuyer.CreditRating = null;
            }

        }

        private void SaveAgentInfoButton_Click(object sender, RoutedEventArgs e)
        {
            NewAgent = new Agent();
            string commission = this.CommissionTextbox.Text.Trim();
            if (commission.Length > 0)
            {
                if (Decimal.TryParse(commission, out decimal commish))
                {
                    NewAgent.CommissionPercent = commish;
                    if (ExistingRECo != null)
                    {
                        NewAgent.CompanyID = ExistingRECo.CompanyID;
                        DisplayStatusMessage("Agent information updated!");
                    }
                }
            }
            else
            {
                DisplayStatusMessage("Enter a valid Commission Percentage.");
            }

        }

        /// <summary>
        /// Takes a Person instance and saves it to the DB then synchronizes the People Collection with the DB to ensure each instance has an ID.
        /// </summary>
        /// <param name="p"></param>
        private void SavePersonAndRefreshCollection(Person p)
        {
            if (SaveToEntities())
            {
                MainWindow.InitPeopleCollection();
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
        private bool CreateNewPerson()
        {
            bool result = false;
            int itemsCount = 0;
            var resultMessage = new StringBuilder();
            resultMessage.Append("Missing required fields: ");
            string firstName = this.FNameTextbox.Text.Trim();
            string lastName = this.LNameTextbox.Text.Trim();
            string phone = this.PhoneTextbox.Text.Trim();
            string email = this.EmailTextbox?.Text.Trim() ?? string.Empty;

            if (firstName.Length > 0 || firstName.Length < 30)
            {
                itemsCount++;
            }
            else
            {
                resultMessage.Append("First Name ");
            }

            if (lastName.Length > 0 && lastName.Length < 50)
            {
                itemsCount++;
            }
            else 
            {
                resultMessage.Append("Last Name ");
            }

            if (phone.Length == 10)
            {
                itemsCount++;
            }
            else 
            { 
                resultMessage.Append("Phone Number ");
            }

            if (itemsCount > 2)
            {
                resultMessage.Clear();
                NewPerson.FirstName = firstName;
                NewPerson.LastName = lastName;
                NewPerson.Phone = phone;
                NewPerson.Email = email;

                if (LogicBroker.SaveEntity<Person>(NewPerson))
                {
                    MainWindow.InitializeCollections();
                    NewPersonID = MainWindow.peopleCollection.Where(p => p.FirstName == NewPerson.FirstName && p.LastName == NewPerson.LastName).FirstOrDefault().PersonID;
                }
                DisplayStatusMessage("Person created!");
                result = true;
            }
            else
            {
                DisplayStatusMessage(resultMessage.ToString());
                result = false;
            }

            return result;
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
            if (IsButtonClose)
            {
                e.Cancel = false;
                MainWindow.InitializeCollections();
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
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
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
            if (LogicBroker.SaveEntity<Person>(NewPerson))
            {
                count++;
            }
            if (count > 0)
            {
                result = true;
            }
            return result;
        }

        private void LoadAgentPanel()
        {
            ExistingRECoComboBox.IsEnabled = true;
            CommissionTextbox.IsReadOnly = false;
            CommissionTextbox.IsEnabled = true;
            AgentRecoTextbox.IsReadOnly = true;
            AddAgentButton.IsEnabled = true;
        }

        /// <summary>
        /// Load necessary Window controls to allow creating a new Owner Person.
        /// </summary>
        private void LoadOwnerPanel()
        {
            PreferredLenderTextbox.IsEnabled = true;
            PreferredLenderTextbox.IsReadOnly = false;
            AddOwnerButton.IsEnabled = true;
        }

        /// <summary>
        /// Load necessary Window controls to allow creating a new Buyer Person.
        /// </summary>
        private void LoadBuyerPanel()
        {
            CreditRatingTextbox.IsReadOnly = false;
            AddBuyerButton.IsEnabled = true;
        }

        private void LoadRECoComboBox()
        {
            var recosList = (from re in MainWindow.reCosCollection
                             select re).ToList();
            ExistingRECoComboBox.ItemsSource = recosList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsButtonClose = false;
            NewPerson = new Person();

            switch (this.AddType)
            {
                case "Agent":
                    {
                        NewAgent = new Agent();
                        ExistingRECo = new RealEstateCompany();
                        LoadAgentPanel();
                        LoadRECoComboBox();
                        DisableBuyerPanel();
                        DisableOwnerPanel();
                        break;
                    }
                case "Owner":
                    {
                        NewOwner = new Owner();
                        LoadOwnerPanel();
                        DisableAgentPanel();
                        DisableBuyerPanel();
                        break;
                    }
                case "Buyer":
                    {
                        NewBuyer = new Buyer();
                        LoadBuyerPanel();
                        DisableAgentPanel();
                        DisableOwnerPanel();
                        break;
                    }
                default:
                    {
                        DisplayStatusMessage("No type included. Click Close to exit without saving.");
                        AddOwnerButton.IsEnabled = false;
                        AddBuyerButton.IsEnabled = false;
                        AddAgentButton.IsEnabled = false;
                        UpdatePersonInfoButton.IsEnabled = false;
                        CloseButton.IsEnabled = true;
                        this.IsButtonClose = true;
                        break;
                    }
            }

        }

        private void CreateThisPersonButton_Click(object sender, RoutedEventArgs e)
        {
            int itemsCount = 0;
            if(CreateNewPerson()) 
            {
                if (NewAgent != null && NewPersonID > -1)
                {
                    NewAgent.AgentID = NewPersonID;
                    if (LogicBroker.SaveEntity<Agent>(NewAgent))
                    {
                        itemsCount++;
                    }
                }
                if (NewBuyer != null && NewPersonID > -1)
                {
                    NewBuyer.BuyerID = NewPersonID;
                    if (LogicBroker.SaveEntity<Buyer>(NewBuyer))
                    {
                        itemsCount++;
                    }
                }
                if (NewOwner != null && NewPersonID > -1)
                {
                    NewOwner.OwnerID = NewPersonID;
                    if (LogicBroker.SaveEntity<Owner>(NewOwner))
                    {
                    itemsCount++;
                    }
                }
                if (itemsCount > 0)
                {
                    DisplayStatusMessage($"{ AddType } saved! Click Close button to exit this window.");
                }
            }
            else
            {
                DisplayStatusMessage($"Unable to save { AddType }");
                IsButtonClose = false;  //  no person created so no save possible
            }

        }

        private void ExistingRECosCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRECo = (sender as ComboBox).SelectedItem as RealEstateCompany;
            ExistingRECo = MainWindow.reCosCollection.Where(re => re.CompanyID == selectedRECo.CompanyID).FirstOrDefault();
            if (ExistingRECo != null)
            {
                AgentRecoTextbox.Text = selectedRECo.CompanyName;
            }
            else
            {
                AgentRecoTextbox.Text = "Agent no longer active.";
            }
            NewAgent.CompanyID = ExistingRECo.CompanyID;
        }

        private void DisableAgentPanel()
        {
            AddAgentButton.IsEnabled = false;
            AgentRecoTextbox.IsReadOnly = true;
            CommissionTextbox.IsReadOnly = true;
        }

        private void DisableBuyerPanel()
        {
            AddBuyerButton.IsEnabled = false;
            CreditRatingTextbox.IsReadOnly = true;
        }

        private void DisableOwnerPanel()
        {
            AddOwnerButton.IsEnabled = false;
            PreferredLenderTextbox.IsReadOnly = true;
        }

    }
}
