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
    public partial class PersonAddUpdateWindow : Window
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


        public PersonAddUpdateWindow()
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

        private void UpdateAndCloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //  store the a new or updated person but not a match
            Person checkForDouble = (from p in MainWindow.peopleCollection
                                     where UpdatePerson.FirstName == p.FirstName &&
                                     UpdatePerson.LastName == p.LastName &&
                                     UpdatePerson.Phone == p.Phone
                                     select p).SingleOrDefault();
            if (checkForDouble == null)
            {
                LogicBroker.UpdateEntity<Person>(UpdatePerson);
            }

            //  store the new/existing person Type depending on the workflow context
            string updateType = UpdateType.Trim().ToUpper();
            switch (updateType)
            {
                case "AGENT":
                    {
                        //  TODO: Test this
                        UpdateHomeSale.AgentID = UpdateAgent.AgentID;
                        //UpdateHomeSale.Agent = UpdateAgent;
                        LogicBroker.UpdateEntity<HomeSale>(UpdateHomeSale);
                        break;
                    }
                case "OWNER":
                    {
                        //  TODO: Test this
                        UpdateHome.OwnerID = UpdateOwner.OwnerID;
                        LogicBroker.UpdateEntity<Home>(UpdateHome);
                        break;
                    }
                case "BUYER":
                    {
                        //  TODO: Test this
                        UpdateHomeSale.BuyerID = UpdateBuyer.BuyerID;
                        LogicBroker.UpdateEntity<HomeSale>(UpdateHomeSale);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            IsButtonClose = true;
            this.Close();
        }
        
        private void SaveAndCloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //  store the a new or updated person but not a match
            Person checkForDouble = (from p in MainWindow.peopleCollection
                                     where UpdatePerson.FirstName == p.FirstName &&
                                     UpdatePerson.LastName == p.LastName &&
                                     UpdatePerson.Phone == p.Phone
                                     select p).SingleOrDefault();
            if (checkForDouble == null)
            {
                LogicBroker.SaveEntity<Person>(UpdatePerson);
            }

            //  store the new/existing person Type depending on the workflow context
            string updateType = UpdateType.Trim().ToUpper();
            switch (updateType)
            {
                case "AGENT":
                    {
                        //  TODO: Test this
                        UpdateHomeSale.AgentID = UpdateAgent.AgentID;
                        UpdateHomeSale.Agent = UpdateAgent;
                        //LogicBroker.SaveEntity<Agent>(UpdateAgent);
                        LogicBroker.SaveEntity<HomeSale>(UpdateHomeSale);
                        break;
                    }
                case "OWNER":
                    {
                        //  TODO: Test this
                        UpdateHome.OwnerID = UpdateOwner.OwnerID;
                        LogicBroker.SaveEntity<Home>(UpdateHome);
                        LogicBroker.SaveEntity<Owner>(UpdateOwner);
                        break;
                    }
                case "BUYER":
                    {
                        //  TODO: Test this
                        UpdateHomeSale.BuyerID = UpdateBuyer.BuyerID;
                        LogicBroker.SaveEntity<HomeSale>(UpdateHomeSale);
                        LogicBroker.SaveEntity<Buyer>(UpdateBuyer);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            IsButtonClose = true;
            this.Close();
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
                    MainWindow.InitHomeSalesCollection();
                    MainWindow.InitRealEstateCompaniesCollection();
                }
                catch (Exception ex)
                {
                    var userResponse = MessageBox.Show("An error occurred. Close anyway?", "Something went wrong!", MessageBoxButton.YesNo);
                    if (userResponse == MessageBoxResult.No)
                    {
                        IsButtonClose = false;
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Use the Save And Close button to exit.", "Warning!", MessageBoxButton.OK);
                IsButtonClose = false;
                e.Cancel = true;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string updateType = UpdateType.Trim().ToUpper();
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
            if (string.IsNullOrEmpty(creditRatingTextbox.Text.Trim()))
            {
                DisplayStatusMessage("To Update, enter a new Credit Rating. Example: 650");
            }
            else if(int.TryParse(creditRatingTextbox.Text.Trim(), out updateCreditRating))
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
            string quote = char.ToString('"');
            string updatePreferredLender = string.Empty;
            if (string.IsNullOrEmpty(preferredLenderTextbox.Text.Trim()))
            {
                DisplayStatusMessage($"To Update, enter a new Preferred Lender. Example: { quote }US Bank NA{ quote }.");
            }
            else if (updatePreferredLender.Length > 2)
            {
                if (updatePreferredLender != UpdateOwner.PreferredLender)
                {
                    UpdateOwner.PreferredLender = updatePreferredLender;
                    DisplayStatusMessage("Preferred Lender udpated. Click Save to continue or Close to exit without saving.");
                }
                else
                {
                    DisplayStatusMessage($"To Update, enter a new Preferred Lender name. Example: { quote }HSBC{ quote }.");
                }
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
