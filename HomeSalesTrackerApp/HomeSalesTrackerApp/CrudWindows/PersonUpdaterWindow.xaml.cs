using HSTDataLayer;
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
        public bool CalledByUpdateMenu { get; set; }    //  Called from MainWindow UPDATE menu item
        public string CalledByUpdateMenuType { get; set; }  //  The person sub-type: Agent, Buyer, Owner

        public Person UpdatePerson { get; set; }
        public Agent UpdateAgent { get; set; }
        public Owner UpdateOwner { get; set; }
        public Buyer UpdateBuyer { get; set; }


        public PersonUpdaterWindow()
        {
            InitializeComponent();
        }

        private void LoadAgentPanel()
        {
            if (UpdateAgent != null)
            {
                LoadPersonInformation();
                //agentIdTextbox.Text = UpdateAgent.AgentID.ToString() ?? string.Empty;

                //  query the recoCollection to get RE Company Name
                if (UpdateAgent.CompanyID == null)
                {
                    AgentReCompanyTextbox.Text = "Agent no longer active";
                }
                else
                {
                    RealEstateCompany tempRECo = (from reco in MainWindow.reCosCollection
                                                  where reco.CompanyID == UpdateAgent.CompanyID
                                                  select reco).FirstOrDefault();


                    AgentReCompanyTextbox.Text = tempRECo.CompanyName.ToString();
                }

                agentCommissionTextbox.Text = UpdateAgent.CommissionPercent.ToString();
            }
            else
            {
                //agentIdTextbox.Text = string.Empty;
                agentCommissionTextbox.Text = string.Empty;
                AgentReCompanyTextbox.Text = string.Empty;
            }
            
            RefreshAgentsComboBox();
            closeButton.Visibility = Visibility.Visible;
        }

        private void LoadPersonInformation()
        {
            fNameTextbox.Text = UpdatePerson.FirstName?.ToString() ?? string.Empty;
            lNameTextbox.Text = UpdatePerson.LastName?.ToString() ?? string.Empty;
            phoneTextbox.Text = UpdatePerson.Phone?.ToString() ?? string.Empty;
            emailTextbox.Text = UpdatePerson.Email?.ToString() ?? string.Empty;
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
                UpdatePerson = new Person()
                {
                    FirstName = fNameTextbox.Text.Trim(),
                    LastName = lNameTextbox.Text.Trim(),
                    Phone = phoneTextbox.Text.Trim(),
                    Email = emailTextbox.Text.Trim() ?? null
                };
            }

        }

        private void LoadBuyerPanel()
        {
            LoadPersonInformation();
            //if (UpdateBuyer != null)
            //{
            //    if (string.IsNullOrEmpty(UpdateBuyer.CreditRating.ToString()))
            //    {
            //        creditRatingTextbox.Text = string.Empty;
            //        DisplayStatusMessage("Buyer information must be updated and saved.");
            //    }
            //    else
            //    {
            //        creditRatingTextbox.Text = UpdateBuyer.CreditRating.ToString();
            //        DisplayStatusMessage("Displaying existing Buyer information");
            //    }
            //}
            //else
            //{
            //    creditRatingTextbox.Text = string.Empty;
            //}
            creditRatingTextbox.Text = UpdateBuyer.CreditRating?.ToString() ?? string.Empty;
            RefreshBuyersComboBox();
            closeButton.Visibility = Visibility.Visible;
        }

        private void LoadOwnerPanel()
        {
            LoadPersonInformation();
            //if (UpdateOwner != null)
            //{
            //    if (string.IsNullOrEmpty(UpdateOwner.PreferredLender.ToString()))
            //    {
            //        preferredLenderTextbox.Text = string.Empty;
            //        DisplayStatusMessage("Owner information must be updated and saved.");
            //    }
            //    else
            //    {
            //        preferredLenderTextbox.Text = UpdateOwner.PreferredLender.ToString();
            //        DisplayStatusMessage("Displaying existing Owner information.");
            //    }
            //}
            //else
            //{
            //    preferredLenderTextbox.Text = string.Empty;
            //}
            preferredLenderTextbox.Text = UpdateOwner.PreferredLender?.ToString() ?? string.Empty;
            RefreshOwnersComboBox();
            closeButton.Visibility = Visibility.Visible;
        }

        private void ExistingOwnersCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayStatusMessage("Owner selection changed!");

            Person comboBoxPerson = (sender as ComboBox).SelectedItem as Person;
            Owner tempOwner = (from p in MainWindow.peopleCollection
                               where comboBoxPerson.PersonID == p.Owner.OwnerID
                               select p.Owner).FirstOrDefault();

            if (comboBoxPerson!= null && tempOwner != null)
            {
                UpdatePerson = comboBoxPerson;
                UpdateOwner = tempOwner;
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
            Buyer tempBuyer = (from p in MainWindow.peopleCollection
                               where comboBoxPerson.PersonID == p.Buyer.BuyerID
                               select p.Buyer).FirstOrDefault();

            if (comboBoxPerson != null && tempBuyer != null)
            {
                UpdatePerson = comboBoxPerson;
                UpdateBuyer = tempBuyer;
                LoadBuyerPanel();
                DisplayStatusMessage("Loaded the seelcted existing Buyer.");
            }
            else
            {
                DisplayStatusMessage("Could not find Buyer information.");
            }
        }

        private void ExistingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs sceArgs)
        {
            DisplayStatusMessage("Agent selection changed!");
            //  get personid and correlate agentid
            Person comboBoxPerson = (sender as ComboBox).SelectedItem as Person;
            Agent tempAgent = (from p in MainWindow.peopleCollection
                               where comboBoxPerson.PersonID == p.Agent.AgentID
                               select p.Agent).FirstOrDefault();

            if (comboBoxPerson != null && tempAgent != null)
            {
                //  return new Basic Information
                UpdatePerson = comboBoxPerson;

                //  return new Agent Details
                UpdateAgent = tempAgent;

                LoadAgentPanel();
                DisplayStatusMessage("Loaded the selected existing Agent.");
            }
            else
            {
                DisplayStatusMessage("Could not find Agent information.");
            }
        }

        private void RefreshAgentsComboBox()
        {
            var listOfHomesalesAgents = (from hs in MainWindow.homeSalesCollection
                                         from a in MainWindow.peopleCollection
                                         where a.PersonID == hs.AgentID
                                         select a).ToList();
            existingAgentsCombobox.ItemsSource = listOfHomesalesAgents;

        }

        private void RefreshBuyersComboBox()
        {
            var listOfBuyers = (from hs in MainWindow.homeSalesCollection
                                from a in MainWindow.peopleCollection
                                where a.PersonID == hs.BuyerID
                                select a).ToList();
            existingBuyersCombobox.ItemsSource = listOfBuyers;
        }

        private void RefreshOwnersComboBox()
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

        ///// <summary>
        ///// REFACTOR THIS! Used to call LogicBroker Update method (rather than Add method).
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void UpdateAndCloseWindowButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //  TODO: Refactor this method to add Agent, Buyer, Owner information to the existing Person object THEN send it to LogicBroker for saving/updating.

        //    GetPersonInfoFromTextboxes();

        //    //  store the a new or updated person but not a match
        //    Person checkForDouble = (from p in MainWindow.peopleCollection
        //                             where UpdatePerson.FirstName == p.FirstName &&
        //                             UpdatePerson.LastName == p.LastName &&
        //                             UpdatePerson.Phone == p.Phone
        //                             select p).SingleOrDefault();
        //    if (checkForDouble == null)
        //    {
        //        LogicBroker.UpdateEntity<Person>(UpdatePerson);
        //    }

        //    //  store the new/existing person Type depending on the workflow context
        //    string updateType = CalledByUpdateMenuType.Trim().ToUpper();
        //    switch (updateType)
        //    {
        //        case "AGENT":
        //            {
        //                //  TODO: Test this
                        
        //                LogicBroker.UpdateEntity<Agent>(UpdateAgent);
        //                break;
        //            }
        //        case "OWNER":
        //            {
        //                //  TODO: Test this
        //                LogicBroker.UpdateEntity<Owner>(UpdateOwner);
        //                break;
        //            }
        //        case "BUYER":
        //            {
        //                //  TODO: Test this
        //                LogicBroker.UpdateEntity<Buyer>(UpdateBuyer);
        //                break;
        //            }
        //        default:
        //            {
        //                break;
        //            }
        //    }

        //    IsButtonClose = true;
        //    this.Close();
        //}

        /// <summary>
        /// Save NEW record(s) to the DB and then close the current Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAndCloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //  IF an Owner, Buyer, or Agent have been created they will be attached to the new or existing Person in the Basic Information fields on the form
            GetPersonInfoFromTextboxes();

            //  Only add updated fields to the Person instance (remember this form CREATES a new Person)
            if (UpdateAgent != null) {
                UpdatePerson.Agent = UpdateAgent;
            }
            if (UpdateBuyer != null) {
                UpdatePerson.Buyer = UpdateBuyer;
            }
            if (UpdateOwner != null) {
                UpdatePerson.Owner = UpdateOwner;
            }

            //  Save!

            if (LogicBroker.UpdateEntity<Person>(UpdatePerson))
            {
                DisplayStatusMessage("Saved!");
                IsButtonClose = true;
            }
            else
            {
                DisplayStatusMessage("Unable to save.");
            }
            this.Close();

            ////  store the a new or updated person but not a match
            //Person checkForDouble = (from p in MainWindow.peopleCollection
            //                         where UpdatePerson.FirstName == p.FirstName &&
            //                         UpdatePerson.LastName == p.LastName &&
            //                         UpdatePerson.Phone == p.Phone
            //                         select p).SingleOrDefault();
            //if (checkForDouble == null)
            //{

            //    //  store the new/existing person Type depending on the workflow context
            //    string updateType = CalledByUpdateMenuType.Trim().ToUpper();
            //    switch (updateType)
            //    {
            //        case "AGENT":
            //            {
            //                //  TODO: Test this
            //                UpdatePerson.Agent = UpdateAgent;
            //                //LogicBroker.SaveEntity<Agent>(UpdateAgent);
            //                break;
            //            }
            //        case "OWNER":
            //            {
            //                //  TODO: Test this
            //                UpdatePerson.Owner = UpdateOwner;
            //                break;
            //            }
            //        case "BUYER":
            //            {
            //                //  TODO: Test this
            //                UpdatePerson.Buyer = UpdateBuyer;
            //                break;
            //            }
            //        default:
            //            {
            //                break;
            //            }
            //    }

            //    if (LogicBroker.SaveEntity<Person>(UpdatePerson))
            //    {
            //        IsButtonClose = true;
            //        AddHomeWindow.APerson = UpdatePerson;
            //    }
            //    else
            //    {
            //        IsButtonClose = false;
            //        DisplayStatusMessage("Unable to save to database.");
            //    }
            //}
            //else
            //{
            //    string flName = $"{ UpdatePerson.FirstName } { UpdatePerson.LastName }";
            //    DisplayStatusMessage($"Not added: { flName } already exists.");
            //}
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string updateType = CalledByUpdateMenuType.Trim().ToUpper();
            switch (updateType)
            {
                case "AGENT":
                    {
                        LoadAgentPanel();
                        break;
                    }
                case "OWNER":
                    {
                        LoadOwnerPanel();
                        break;
                    }
                case "BUYER":
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
            IsButtonClose = true;
        }

        private void AddAgentButton_Click(object sender, RoutedEventArgs e)
        {
            decimal updateAgentCommish = 0.0m;
            if (string.IsNullOrEmpty(agentCommissionTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To Update, enter a new Commission Rate. Example 4% would be: 0.04");
            }
            else if (Decimal.TryParse(agentCommissionTextbox.Text.Trim(), out updateAgentCommish))
            {
                if (updateAgentCommish > 0.00m && updateAgentCommish < 1.0m)
                {
                    if (updateAgentCommish != UpdateAgent.CommissionPercent)
                    {
                        UpdateAgent.CommissionPercent = updateAgentCommish;
                        DisplayStatusMessage("Agent Commission updated. Click Save to continue or Close to abort.");
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
                DisplayStatusMessage("Unable to parse input. Hey developer! Check this out!!");
            }

        }

        private void AddBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            int updateCreditRating = 0;
            if (string.IsNullOrEmpty(creditRatingTextbox.Text) || string.IsNullOrWhiteSpace(creditRatingTextbox.Text))
            {
                DisplayStatusMessage("To Update, enter a new Credit Rating. Example: 650");
            }
            if(int.TryParse(creditRatingTextbox.Text.Trim(), out updateCreditRating))
            {
                if(updateCreditRating > 299 && updateCreditRating < 851)
                {
                    if (updateCreditRating != UpdateBuyer.CreditRating)
                    {
                        UpdateBuyer.CreditRating = updateCreditRating;
                        DisplayStatusMessage("Credit Rating updated. Click Save to continue or Close to exit without saving.");
                    }
                    else
                    {
                        DisplayStatusMessage("To Update, enter a new Credit Rating between 300 and 850.");
                    }
                }
                else
                {
                    //  see Equifax for an explanation of the credit rating range. Experian has a similar explanation under different branding.
                    DisplayStatusMessage("Enter a valid Credit Rating between 300 and 850.");
                }
            }
            else
            {
                DisplayStatusMessage("Unable to parse input. Hey developer! Check this out!!");
            }

        }

        private void AddOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            //  TODO: Refactor this to ensure the Person object is storing the Owner object
            string quote = char.ToString('"');
            string updatePreferredLender = string.Empty;
            if (string.IsNullOrEmpty(preferredLenderTextbox.Text) || string.IsNullOrWhiteSpace(preferredLenderTextbox.Text))
            {
                DisplayStatusMessage($"To Update, enter a new Preferred Lender. Example: { quote }US Bank NA{ quote }.");
            }
            else
            {
                updatePreferredLender = preferredLenderTextbox.Text.Trim();
            }
            if (updatePreferredLender.Length > 2)
            {
                if (UpdateOwner == null) 
                {
                    UpdateOwner = new Owner();
                }
                UpdateOwner.PreferredLender = updatePreferredLender;
                DisplayStatusMessage("Preferred Lender udpated. Click Save to continue or Close to exit without saving.");
            }
            else
            {
                DisplayStatusMessage("Input was not null or empty but something went wrong. Hey Dev! Get over here!!");
            }
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
