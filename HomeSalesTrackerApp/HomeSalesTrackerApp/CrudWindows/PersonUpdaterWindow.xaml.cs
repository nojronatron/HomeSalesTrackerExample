﻿using HSTDataLayer;
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

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for PersonAddUpdateWindow.xaml
    /// </summary>
    public partial class PersonUpdaterWindow : Window
    {
        private bool IsButtonClose { get; set; }
        private RealEstateCompany SelectedReco { get; set; }

        public bool CalledByUpdateMenu { get; set; }    //  Called from MainWindow UPDATE menu item
        public string CalledByUpdateMenuType { get; set; }  //  The person sub-type: Agent, Buyer, Owner

        public Person ReceivedPerson { get; set; }
        private Person UpdatePerson { get; set; }
        public Agent ReceivedAgent { get; set; }
        private Agent UpdateAgent { get; set; }
        public Owner ReceivedOwner { get; set; }
        private Owner UpdateOwner { get; set; }
        public Buyer ReceivedBuyer { get; set; }
        private Buyer UpdateBuyer { get; set; }
        public RealEstateCompany RecievedRECo { get; set; } //  does this window ever need to receive a RECo?
        private RealEstateCompany UpdateRECo { get; set; }

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
                UpdatePerson.FirstName = fNameTextbox.Text.Trim();
                UpdatePerson.LastName = lNameTextbox.Text.Trim();
                UpdatePerson.Phone = phoneTextbox.Text.Trim();
                UpdatePerson.Email = emailTextbox.Text.Trim() ?? null;
            }

        }

        /// <summary>
        /// IN TEST. Load Agent Details with received Agent properties. Also load Agent combobox and realestateco combobox.
        /// </summary>
        private void LoadAgentPanel()
        {
            UpdateAgent = new Agent();
            UpdatePerson = new Person();
            UpdateRECo = new RealEstateCompany();

            //  make sure the Agent received from sender is fully loaded from latest Collections' data
            var tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedAgent.AgentID).FirstOrDefault();
            var tempRECo = MainWindow.reCosCollection.Where(re => re.CompanyID == ReceivedAgent.CompanyID).FirstOrDefault();
            var tempHomesales = MainWindow.homeSalesCollection.Where(hs => hs.AgentID == ReceivedAgent.AgentID).ToList();

            ReceivedAgent.Person = tempPerson;
            ReceivedAgent.RealEstateCompany = tempRECo;
            ReceivedAgent.HomeSales = tempHomesales;

            if (ReceivedAgent != null)
            {

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
            }
            else
            {
                AgentCommissionTextbox.Text = string.Empty;
                AgentReCompanyTextbox.Text = string.Empty;
            }
            LoadAgentsComboBox();
            LoadRealEstateCoCombobox();
        }

        /// <summary>
        /// INCOMPLETE. Load Buyer Details with ereceived Buyer properties. Also load the Buyer Combobox.
        /// </summary>
        private void LoadBuyerPanel()
        {
            UpdateBuyer = new Buyer();
            UpdatePerson = new Person();

            var tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedBuyer.BuyerID).FirstOrDefault();
            var tempHomesales = MainWindow.homeSalesCollection.Where(hs => hs.AgentID == ReceivedBuyer.BuyerID).ToList();

            ReceivedBuyer.Person = tempPerson;
            ReceivedBuyer.HomeSales = tempHomesales;

            if (ReceivedBuyer == null)
            {
                CreditRatingTextbox.Text = "";
            }
            else 
            { 
                CreditRatingTextbox.Text = ReceivedBuyer.CreditRating?.ToString();
                //CreditRatingTextbox.Text = ReceivedBuyer.CreditRating?.ToString() ?? string.Empty;
            }

            CreditRatingTextbox.IsEnabled = true;
            DisableAgentDetailsControls();
            DisableOwnerDetailsControls();
            ExistingBuyersCombobox.IsEnabled = true;
            EnableEditingPersonBasicInformation();

            LoadBuyersComboBox();
        }

        /// <summary>
        /// INCOMPLETE. Load Owner Details and the OWners ComboBox.
        /// </summary>
        private void LoadOwnerPanel()
        {
            UpdateOwner = new Owner();
            UpdatePerson = new Person();

            var tempPerson = MainWindow.peopleCollection.Where(p => p.PersonID == ReceivedOwner.OwnerID).FirstOrDefault();
            var tempHomes = MainWindow.homesCollection.Where(h => h.OwnerID == ReceivedOwner.OwnerID).ToList();

            ReceivedOwner.Person = tempPerson;
            ReceivedOwner.Homes = tempHomes;

            if (ReceivedOwner == null)
            {
                preferredLenderTextbox.Text = "";
            }
            else
            {
                preferredLenderTextbox.Text = ReceivedOwner.PreferredLender?.ToString();
                //preferredLenderTextbox.Text = ReceivedOwner.PreferredLender?.ToString() ?? string.Empty;
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
            //  When user changes the selection, RE Company Name TextBox should be updated and the selection stored in a Property of this instance
            DisplayStatusMessage("Real Estate Co selection changed!");

            RealEstateCompany comboBoxRECo = (sender as ComboBox).SelectedItem as RealEstateCompany;

            RealEstateCompany tempRECo = (from re in MainWindow.reCosCollection
                                          where comboBoxRECo.CompanyID == re.CompanyID
                                          select re).FirstOrDefault();

            Agent tempAgent = (from hs in MainWindow.homeSalesCollection
                               from a in MainWindow.peopleCollection
                               where hs.AgentID == tempRECo.CompanyID &&
                               a.PersonID == hs.AgentID
                               select a.Agent).FirstOrDefault();

            if (comboBoxRECo != null && tempRECo != null && tempAgent != null)
            {
                UpdateAgent = tempAgent;
                UpdateAgent.CompanyID = tempRECo.CompanyID;
                UpdateAgent.RealEstateCompany = tempRECo;
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
                UpdatePerson = tempPerson;
                UpdateAgent = tempAgent;

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
        /// Initialize and set properties to the UpdateAgent object (this.UpdateAgent property). UpdateAgent gets initialized here EVERY TIME this is run.
        /// </summary>
        /// <returns></returns>
        private bool GetAgentUpdatedFields()
        {
            bool result = false;
            int resultCount = 0;
            decimal updateAgentCommish = 0.0m;
            UpdateAgent = new Agent();
            UpdatePerson = ReceivedPerson;
            UpdateAgent = ReceivedAgent;    //  captures AgentID
            UpdateAgent.HomeSales = ReceivedAgent.HomeSales;


            if (string.IsNullOrWhiteSpace(AgentCommissionTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To Update, enter a new Commission Rate. Example 4% would be: 0.04");
            }
            if (Decimal.TryParse(AgentCommissionTextbox.Text.Trim(), out updateAgentCommish))
            {
                if (updateAgentCommish > 0.00m && updateAgentCommish < 1.0m)
                {
                    if (updateAgentCommish != ReceivedAgent.CommissionPercent)
                    {
                        UpdateAgent.CommissionPercent = updateAgentCommish;
                        resultCount++;
                        DisplayStatusMessage("Agent Commission updated. Click Save to close or File -> Exit to quit.");
                    }
                    else
                    {
                        DisplayStatusMessage("To Update, enter a new Commission Rate. Example 4% would be: 0.04");
                    }
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
                if (SelectedReco.CompanyName != UpdateRECo.CompanyName)
                {
                    UpdateAgent.RealEstateCompany = SelectedReco;
                    UpdateRECo = SelectedReco;
                    resultCount++;
                }
            }
            else
            {
                string recoName = AgentReCompanyTextbox.Text.Trim();
                if (string.IsNullOrWhiteSpace(recoName) != true)
                {
                    var inferredReco = (from re in MainWindow.reCosCollection
                                        where re.CompanyName == recoName
                                        select re).FirstOrDefault();
                    if (inferredReco != null && inferredReco.CompanyName != UpdateAgent.RealEstateCompany.CompanyName)
                    {
                        UpdateRECo = inferredReco;
                        UpdateAgent.RealEstateCompany = UpdateRECo;
                        UpdateAgent.CompanyID = UpdateRECo.CompanyID;
                        resultCount++;
                    }
                }
            }

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
            UpdateBuyer = new Buyer();
            UpdatePerson = ReceivedPerson;
            UpdateBuyer = ReceivedBuyer;    //  captures BuyerID
            UpdateBuyer.HomeSales = ReceivedBuyer.HomeSales;

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
                UpdateBuyer.CreditRating = credRating;
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
            UpdateOwner = new Owner();
            UpdatePerson = ReceivedPerson;
            UpdateOwner = ReceivedOwner;    //  captures OwnerID
            UpdateOwner.Homes = ReceivedOwner.Homes;

            if (string.IsNullOrWhiteSpace(preferredLenderTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To update, enter the name of Owner's preferred lending Bank.");
            }
            else
            {
                preferredLender = preferredLenderTextbox.Text.Trim();
            }
            if(preferredLender.Length < 3 || preferredLender.Length > 30)
            {
                DisplayStatusMessage("Enter a Bank Name that is from 3 to 30 characters long.");
            }
            else
            {
                UpdateOwner.PreferredLender = preferredLender;
                resultCount++;
            }

            if(resultCount > 0)
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
                UpdatePerson.Agent = UpdateAgent;
                personSaved = LogicBroker.SaveEntity<Person>(UpdatePerson);
                if (LogicBroker.SaveEntity<Agent>(UpdateAgent))
                {
                    aboSaveCount++;
                }
            }

            if (CalledByUpdateMenuType == "Buyer")
            {
                UpdatePerson.Buyer = UpdateBuyer;
                personSaved = LogicBroker.SaveEntity<Person>(UpdatePerson);
                if(LogicBroker.SaveEntity<Buyer>(UpdateBuyer))
                {
                    aboSaveCount++;
                }
            }

            if(CalledByUpdateMenuType == "Owner")
            {
                UpdatePerson.Owner = UpdateOwner;
                personSaved = LogicBroker.SaveEntity<Person>(UpdatePerson);
                if (LogicBroker.SaveEntity<Owner>(UpdateOwner))
                {
                    aboSaveCount++;
                }
            }

            if (personSaved || aboSaveCount > 0)
            {
                DisplayStatusMessage("Saved!");
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
            LoadPersonInformation();
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
        }

        private void MenuRefresh_Click(object sender, RoutedEventArgs e)
        {
            //  reset all input fields and refresh all comboboxes
            ClearPersonTextboxes();
            LoadAgentPanel();
            LoadBuyerPanel();
            LoadOwnerPanel();
        }
    }
}
