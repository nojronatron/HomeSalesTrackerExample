using HomeSalesTrackerApp.Helpers;

using HSTDataLayer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for PersonAddUpdateWindow.xaml
    /// </summary>
    public partial class PersonUpdaterWindow : Window
    {
        private bool IsButtonClose { get; set; }
        private Logger logger = null;
        private RealEstateCompany SelectedReco { get; set; }

        public bool CalledByUpdateMenu { get; set; }
        public string CalledByUpdateMenuType { get; set; }
        public Person ReceivedPerson { get; set; }
        public Agent ReceivedAgent { get; set; }
        public Owner ReceivedOwner { get; set; }
        public Buyer ReceivedBuyer { get; set; }
        public RealEstateCompany ReceivedRECo { get; set; }

        public PersonUpdaterWindow()
        {
            InitializeComponent();
        }

        private void DisableAgentDetailsControls()
        {
            //  Agent Details
            AgentCommissionTextbox.IsReadOnly = true;
            AgentReCompanyTextbox.IsReadOnly = true;
            ExistingAgentsCombobox.IsEnabled = false;
            UpdateAgentButton.IsEnabled = false;
        }

        private void DisableBuyerDetailsControls()
        {
            //  Buyer Details
            CreditRatingTextbox.IsReadOnly = true;
            UpdateBuyerButton.IsEnabled = false;
            ExistingBuyersCombobox.IsEnabled = false;
            UpdateBuyerButton.IsEnabled = false;
        }

        private void DisableOwnerDetailsControls()
        {
            //  Owner Details
            preferredLenderTextbox.IsReadOnly = true;
            UpdateOwnerButton.IsEnabled = false;
            existingOwnersCombobox.IsEnabled = false;
            UpdateOwnerButton.IsEnabled = false;
        }

        private void DisableEditingPersonBasicInformation()
        {
            fNameTextbox.IsEnabled = false;
            lNameTextbox.IsEnabled = false;
            phoneTextbox.IsEnabled = false;
            emailTextbox.IsEnabled = false;
        }

        private void EnableEditingPersonBasicInformation()
        {
            fNameTextbox.IsEnabled = true;
            lNameTextbox.IsEnabled = true;
            phoneTextbox.IsEnabled = true;
            emailTextbox.IsEnabled = true;
        }

        private void LoadPersonInformation()
        {
            fNameTextbox.Text = ReceivedPerson.FirstName?.ToString() ?? string.Empty;
            lNameTextbox.Text = ReceivedPerson.LastName?.ToString() ?? string.Empty;
            phoneTextbox.Text = ReceivedPerson.Phone?.ToString() ?? string.Empty;
            emailTextbox.Text = ReceivedPerson.Email?.ToString() ?? string.Empty;
        }

        private void ClearPersonTextboxes()
        {
            fNameTextbox.Text = string.Empty;
            lNameTextbox.Text = string.Empty;
            phoneTextbox.Text = string.Empty;
            emailTextbox.Text = string.Empty;
            DisplayStatusMessage("Cleared all inputs.");
        }

        /// <summary>
        /// Validates user input for Person information and creates a new instance of this.UpdatePerson property.
        /// </summary>
        private void GetPersonInfoFromTextboxes()
        {
            StringBuilder resultMessage = new StringBuilder();
            resultMessage.Append("Required info: ");

            if (string.IsNullOrEmpty(fNameTextbox.Text) || string.IsNullOrWhiteSpace(fNameTextbox.Text))
            {
                resultMessage.Append("First Name ");
            }

            if (string.IsNullOrEmpty(lNameTextbox.Text) || string.IsNullOrWhiteSpace(lNameTextbox.Text))
            {
                resultMessage.Append("Last Name ");
            }

            if (string.IsNullOrEmpty(phoneTextbox.Text) || string.IsNullOrWhiteSpace(phoneTextbox.Text))
            {
                resultMessage.Append("Phone Numer ");
            }

            if (resultMessage.Length > 15)
            {
                resultMessage.Append(".");
                DisplayStatusMessage(resultMessage.ToString());
            }
            else
            {
                ReceivedPerson.FirstName = fNameTextbox.Text.Trim();
                ReceivedPerson.LastName = lNameTextbox.Text.Trim();
                ReceivedPerson.Phone = phoneTextbox.Text.Trim();
                ReceivedPerson.Email = emailTextbox.Text.Trim() ?? null;
            }

        }

        /// <summary>
        /// IN TEST. Heavily refactored to enable adding Agent to existing Person for Add HFS scenario.
        /// Load Agent Details with received Agent properties. Also load Agent combobox and realestateco combobox.
        /// </summary>
        private void LoadAgentPanel()
        {
            var tempPerson = new Person();
            var tempRECo = new RealEstateCompany();
            var tempHomesales = new List<HomeSale>();

            if (ReceivedAgent != null)
            {
                //  make sure the Agent received from sender is fully loaded from latest Collections' data
                tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedAgent.AgentID).FirstOrDefault();
                tempRECo = MainWindow.reCosCollection.Where(re => re.CompanyID == ReceivedAgent.CompanyID).FirstOrDefault();
                tempHomesales = MainWindow.homeSalesCollection.Where(hs => hs.AgentID == ReceivedAgent.AgentID).ToList();

                ReceivedAgent.Person = tempPerson;
                ReceivedAgent.RealEstateCompany = tempRECo;
                ReceivedAgent.HomeSales = tempHomesales;

                if (ReceivedAgent.CompanyID == null)
                {
                    AgentReCompanyTextbox.Text = "Agent no longer active";
                }
                else
                {
                    AgentReCompanyTextbox.Text = tempRECo.CompanyName.ToString();
                    AgentReCompanyTextbox.IsReadOnly = true;
                }

                AgentCommissionTextbox.Text = ReceivedAgent.CommissionPercent.ToString();
                AgentCommissionTextbox.IsEnabled = true;
                DisableBuyerDetailsControls();
                DisableOwnerDetailsControls();
                ExistingAgentsCombobox.IsEnabled = false;
                EnableEditingPersonBasicInformation();
                LoadAgentsComboBox();
            }

            if (ReceivedAgent == null && ReceivedPerson != null)
            {
                ReceivedAgent = new Agent();
                tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedPerson.PersonID).FirstOrDefault();
                tempRECo = null;
                tempHomesales = new List<HomeSale>();
                ReceivedAgent.Person = tempPerson;
                ReceivedAgent.RealEstateCompany = null;
                AgentReCompanyTextbox.IsReadOnly = true;
                AgentCommissionTextbox.IsEnabled = true;
                if (ReceivedPerson.Buyer != null)
                {
                    CreditRatingTextbox.Text = ReceivedPerson.Buyer.CreditRating?.ToString() ?? string.Empty;
                }
                DisableBuyerDetailsControls();
                DisableOwnerDetailsControls();
                ExistingAgentsCombobox.IsEnabled = false;   //  user is here to create new Agent props for existing person
                DisableEditingPersonBasicInformation(); //  user is NOT here to change the Person
                AgentCommissionTextbox.Text = string.Empty;
                AgentReCompanyTextbox.Text = string.Empty;
                LoadRealEstateCoCombobox();
            }

            LoadRealEstateCoCombobox();
        }

        /// <summary>
        /// Loads Buyer Details with received Buyer properties. Also load the Buyer Combobox.
        /// </summary>
        private void LoadBuyerPanel()
        {
            var tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedBuyer.BuyerID).FirstOrDefault();
            var tempHomesales = MainWindow.homeSalesCollection.Where(hs => hs.AgentID == ReceivedBuyer.BuyerID).ToList();

            if (tempPerson != null)
            {
                ReceivedBuyer.Person = tempPerson;
            }

            if (tempHomesales.Count < 1)
            {
                ReceivedBuyer.HomeSales = tempHomesales;
            }

            CreditRatingTextbox.Text = ReceivedBuyer.CreditRating?.ToString() ?? string.Empty;
            CreditRatingTextbox.IsEnabled = true;
            DisableAgentDetailsControls();
            DisableOwnerDetailsControls();
            ExistingBuyersCombobox.IsEnabled = true;
            EnableEditingPersonBasicInformation();
            LoadBuyersComboBox();
        }

        /// <summary>
        /// Load Owner Details and the OWners ComboBox.
        /// </summary>
        private void LoadOwnerPanel()
        {
            var tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedOwner.OwnerID).FirstOrDefault();
            var tempHomes = MainWindow.homesCollection.Where(h => h.OwnerID == ReceivedOwner.OwnerID).ToList();

            if (tempPerson != null)
            {
                ReceivedOwner.Person = tempPerson;
            }

            if (tempHomes != null)
            {
                ReceivedOwner.Homes = tempHomes;
            }

            if (ReceivedOwner == null)
            {
                preferredLenderTextbox.Text = "";
            }
            else
            {
                preferredLenderTextbox.Text = ReceivedOwner.PreferredLender?.ToString();
            }

            preferredLenderTextbox.IsEnabled = true;
            DisableAgentDetailsControls();
            DisableBuyerDetailsControls();
            existingOwnersCombobox.IsEnabled = true;
            EnableEditingPersonBasicInformation();
            LoadOwnersComboBox();
        }

        private void LoadRealEstateCoCombobox()
        {
            var comboBoxRECos = (from re in MainWindow.reCosCollection
                                 select re).ToList();
            ExistingRECoComboBox.ItemsSource = comboBoxRECos;
        }

        private void ExistingRECosCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayStatusMessage("Real Estate Co selection changed!");
            SelectedReco = new RealEstateCompany();
            SelectedReco = (sender as ComboBox).SelectedItem as RealEstateCompany;

            RealEstateCompany tempRECo = (from re in MainWindow.reCosCollection
                                          where SelectedReco.CompanyID == re.CompanyID
                                          select re).FirstOrDefault();

            Agent tempAgent = (from hs in MainWindow.homeSalesCollection
                               from a in MainWindow.peopleCollection
                               where hs.AgentID == tempRECo.CompanyID &&
                               a.PersonID == hs.AgentID
                               select a.Agent).FirstOrDefault();

            if (SelectedReco != null && tempRECo != null && tempAgent != null)
            {
                ReceivedAgent = tempAgent;
                ReceivedAgent.CompanyID = tempRECo.CompanyID;
                ReceivedAgent.RealEstateCompany = tempRECo;
                SelectedReco = tempRECo;
                AgentReCompanyTextbox.Text = tempRECo.CompanyName;
                DisplayStatusMessage("Loaded the selected RE Company.");
            }
            else
            {
                DisplayStatusMessage("Could not find RE Company info.");
            }

        }

        private void ExistingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs sceArgs)
        {
            DisplayStatusMessage("Agent selection changed!");
            Person comboBoxPerson = (sender as ComboBox).SelectedItem as Person;
            Person tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == comboBoxPerson.PersonID).FirstOrDefault();
            Agent tempAgent = (from p in MainWindow.peopleCollection
                               where comboBoxPerson.PersonID == p.Agent.AgentID
                               select p.Agent).FirstOrDefault();

            if (comboBoxPerson != null && tempAgent != null)
            {
                ReceivedPerson = tempPerson;
                ReceivedAgent = tempAgent;
                LoadAgentPanel();
                DisplayStatusMessage("Loaded the selected existing Agent.");
            }
            else
            {
                DisplayStatusMessage("Could not find Agent information.");
            }
        }

        private void ExistingOwnersCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayStatusMessage("Owner selection changed!");

            Person comboBoxPerson = (sender as ComboBox).SelectedItem as Person;
            Person tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == comboBoxPerson.PersonID).FirstOrDefault();
            Owner tempOwner = (from p in MainWindow.peopleCollection
                               where comboBoxPerson.PersonID == p.Owner.OwnerID
                               select p.Owner).FirstOrDefault();

            if (comboBoxPerson != null && tempOwner != null)
            {
                ReceivedPerson = tempPerson;
                ReceivedOwner = tempOwner;
                LoadOwnerPanel();
                DisplayStatusMessage("Loaded the selected existing Owner.");
            }
            else
            {
                DisplayStatusMessage("Could not find Owner information.");
            }
        }

        private void ExistingBuyersCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person comboBoxPerson = (sender as ComboBox).SelectedItem as Person;
            Person tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == comboBoxPerson.PersonID).FirstOrDefault();
            Buyer tempBuyer = (from p in MainWindow.peopleCollection
                               where comboBoxPerson.PersonID == p.Buyer.BuyerID
                               select p.Buyer).FirstOrDefault();

            if (comboBoxPerson != null && tempBuyer != null)
            {
                ReceivedPerson = tempPerson;
                ReceivedBuyer = tempBuyer;
                LoadBuyerPanel();
                DisplayStatusMessage("Loaded the selected existing Buyer.");
            }
            else
            {
                DisplayStatusMessage("Could not find Buyer information.");
            }
        }

        private void LoadAgentsComboBox()
        {
            var listOfHomesalesAgents = (from hs in MainWindow.homeSalesCollection
                                         from a in MainWindow.peopleCollection
                                         where a.PersonID == hs.AgentID
                                         select a).ToList();
            ExistingAgentsCombobox.ItemsSource = listOfHomesalesAgents;

        }

        private void LoadBuyersComboBox()
        {
            var listOfBuyers = (from hs in MainWindow.homeSalesCollection
                                from a in MainWindow.peopleCollection
                                where a.PersonID == hs.BuyerID
                                select a).ToList();
            ExistingBuyersCombobox.ItemsSource = listOfBuyers;
        }

        private void LoadOwnersComboBox()
        {
            var listOfOwners = (from h in MainWindow.homesCollection
                                from a in MainWindow.peopleCollection
                                where a.PersonID == h.OwnerID
                                select a).ToList();
            existingOwnersCombobox.ItemsSource = listOfOwners;
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        /// <summary>
        /// Initialize and set properties to the UpdateAgent object (this.UpdateAgent property).
        /// </summary>
        /// <returns></returns>
        private bool GetAgentUpdatedFields()
        {
            bool result = false;
            int resultCount = 0;
            decimal updateAgentCommish = 0.0m;

            if (string.IsNullOrWhiteSpace(AgentCommissionTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To Update, enter a new Commission Rate. Example 4% would be: 0.04");
            }
            if (Decimal.TryParse(AgentCommissionTextbox.Text.Trim(), out updateAgentCommish))
            {
                if (updateAgentCommish > 0.00m && updateAgentCommish < 1.0m)
                {
                    ReceivedAgent.CommissionPercent = updateAgentCommish;
                    resultCount++;
                    DisplayStatusMessage("Agent Commission updated. Click Save to close or File -> Exit to quit.");
                }
                else
                {
                    DisplayStatusMessage("Enter a valid Commission Rate between 1 and 99 percent.");
                }

            }
            else
            {
                DisplayStatusMessage("Enter a valid Commission Rate between 1 and 99 percent.");
            }

            if (SelectedReco != null)
            {
                ReceivedAgent.RealEstateCompany = SelectedReco;
                ReceivedAgent.CompanyID = SelectedReco.CompanyID;
                resultCount++;
            }
            else
            {
                string recoName = AgentReCompanyTextbox.Text.Trim();
                if (string.IsNullOrWhiteSpace(recoName) != true)
                {
                    var inferredReco = (from re in MainWindow.reCosCollection
                                        where re.CompanyName == recoName
                                        select re).FirstOrDefault();
                    if (inferredReco != null && inferredReco.CompanyName != ReceivedAgent.RealEstateCompany.CompanyName)
                    {
                        ReceivedRECo = inferredReco;
                        ReceivedAgent.RealEstateCompany = ReceivedRECo;
                        resultCount++;
                    }
                }

            }

            DisplayStatusMessage("Agent assigned to RE Company.");

            if (resultCount > 0)
            {
                result = true;
            }

            return result;
        }

        private bool GetBuyerUpdateFields()
        {
            bool result = false;
            int resultCount = 0;
            string creditRating = string.Empty;

            if (string.IsNullOrWhiteSpace(CreditRatingTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To Update, enter a new Credit Rating from 300 850.");
            }
            else
            {
                creditRating = CreditRatingTextbox.Text.Trim();
            }

            if (creditRating.Length != 3)
            {
                DisplayStatusMessage("Enter a valid Credit Rating between 299 and 851.");
            }
            else if (int.TryParse(creditRating, out int credRating))
            {
                ReceivedBuyer.CreditRating = credRating;
                resultCount++;
                DisplayStatusMessage("Buyer Credit Rating Updated. Click Save to close or File -> Exit to quit.");
            }

            if (resultCount > 0)
            {
                result = true;
            }

            return result;
        }

        private bool GetOwnerUpdateFields()
        {
            bool result = false;
            int resultCount = 0;
            string preferredLender = string.Empty;

            if (string.IsNullOrWhiteSpace(preferredLenderTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To update, enter the name of Owner's preferred lending Bank.");
            }
            else
            {
                preferredLender = preferredLenderTextbox.Text.Trim();
            }
            if (preferredLender.Length < 3 || preferredLender.Length > 30)
            {
                DisplayStatusMessage("Enter a Bank Name that is from 3 to 30 characters long.");
            }
            else
            {
                ReceivedOwner.PreferredLender = preferredLender;
                resultCount++;
            }

            if (resultCount > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Save NEW record(s) to the DB and then close the current Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAndCloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            int aboSaveCount = 0;
            bool personSaved = false;

            if (CalledByUpdateMenuType == "Agent")
            {
                ReceivedPerson.Agent = ReceivedAgent;
                personSaved = LogicBroker.UpdateEntity<Person>(ReceivedPerson);
                ReceivedAgent.Person = ReceivedPerson;
                ReceivedAgent.AgentID = ReceivedPerson.PersonID;
                if (LogicBroker.UpdateEntity<Agent>(ReceivedAgent))
                {
                    aboSaveCount++;
                }
            }

            if (CalledByUpdateMenuType == "Buyer")
            {
                ReceivedPerson.Buyer = ReceivedBuyer;
                personSaved = LogicBroker.UpdateEntity<Person>(ReceivedPerson);
                ReceivedBuyer.Person = ReceivedPerson;
                ReceivedBuyer.BuyerID = ReceivedPerson.PersonID;
                if (LogicBroker.UpdateEntity<Buyer>(ReceivedBuyer))
                {
                    aboSaveCount++;
                }
            }

            if (CalledByUpdateMenuType == "Owner")
            {
                ReceivedPerson.Owner = ReceivedOwner;
                personSaved = LogicBroker.UpdateEntity<Person>(ReceivedPerson);
                ReceivedOwner.Person = ReceivedPerson;
                ReceivedOwner.OwnerID = ReceivedPerson.PersonID;
                if (LogicBroker.UpdateEntity<Owner>(ReceivedOwner))
                {
                    aboSaveCount++;
                }
            }

            if (personSaved || aboSaveCount > 0)
            {
                DisplayStatusMessage("Saved!");
                MainWindow.InitializeCollections();
                IsButtonClose = true;
            }
            else
            {
                DisplayStatusMessage("Unable to save.");
                IsButtonClose = false;
            }
            this.Close();
        }

        /// <summary>
        /// Refresh all collections with updated DB data before returning to a parent window so data is up-to-date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsButtonClose)
            {
                e.Cancel = false;
            }
            else
            {
                var userResponse = MessageBox.Show("Changes will not be saved. Continue?", "Closing Add Home Window", MessageBoxButton.YesNo);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ReceivedRECo = new RealEstateCompany();
            var SelectedRECo = new RealEstateCompany();
            LoadPersonInformation();
            logger = new Logger();
            string updateType = CalledByUpdateMenuType.Trim();
            switch (updateType)
            {
                case "Agent":
                    {
                        LoadAgentPanel();
                        break;
                    }
                case "Owner":
                    {
                        LoadOwnerPanel();
                        break;
                    }
                case "Buyer":
                    {
                        LoadBuyerPanel();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            IsButtonClose = false;
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateAgentButton_Click(object sender, RoutedEventArgs e)
        {
            GetAgentUpdatedFields();
            GetPersonInfoFromTextboxes();
            ReceivedPerson.Agent = ReceivedAgent;
        }

        private void UpdateBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            GetBuyerUpdateFields();
            GetPersonInfoFromTextboxes();
        }

        private void UpdateOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            GetOwnerUpdateFields();
            GetPersonInfoFromTextboxes();
            ReceivedPerson.Owner = ReceivedOwner;
            ReceivedOwner.Person = ReceivedPerson;
            DisplayStatusMessage("Owner information updated!");
        }

        /// <summary>
        /// Reset all input fields and refresh all comboboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuRefresh_Click(object sender, RoutedEventArgs e)
        {
            ClearPersonTextboxes();
            LoadAgentPanel();
            LoadBuyerPanel();
            LoadOwnerPanel();
        }
    }
}
