using HomeSalesTrackerApp.Factory;
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
    public partial class PersonUpdaterWindow : Window, IObserver<NotificationData>
    {
        private Logger logger = null;
        private CollectionMonitor personCollectionMonitor;
        private CollectionMonitor recoCollectionMonitor;

        private bool IsButtonClose { get; set; }
        private RealEstateCompany SelectedReco { get; set; }

        private bool CalledByUpdateMenu { get; set; }
        private string CalledByUpdateMenuType { get; set; } //  "AGENT", "BUYER", or "OWNER"
        private int ReceivedPersonID { get; set; }
        private Person ReceivedPerson { get; set; }
        private Agent ReceivedAgent { get; set; }
        private Owner ReceivedOwner { get; set; }
        private Buyer ReceivedBuyer { get; set; }
        private RealEstateCompany ReceivedRECo { get; set; }
        //private PeopleCollection<Person> _peopleCollection { get; set; }
        //private HomesCollection _homesCollection { get; set; }
        //private HomeSalesCollection _homeSalesCollection { get; set; }
        //private RealEstateCosCollection _recosCollection { get; set; }

        public PersonUpdaterWindow()
        {
            InitializeComponent();
        }

        public PersonUpdaterWindow(bool calledByUpdateMenu, string aboType, int personID) : this()
        {
            CalledByUpdateMenu = calledByUpdateMenu;
            CalledByUpdateMenuType = aboType.Trim().ToUpper();
            ReceivedPersonID = personID;
        }

        //private void LoadDataForBuyerUpdate()
        //{
        //    ReceivedPerson = _peopleCollection.Where(p => p.PersonID == ReceivedPersonID).FirstOrDefault();
        //    if (ReceivedPerson != null)
        //    {
        //        if (ReceivedPerson.Buyer != null)
        //        {
        //            ReceivedBuyer = ReceivedPerson.Buyer;
        //        }
        //        this.Title = "Update Person's Buyer Info";
        //    }
        //}

        //private void LoadDataForOwnerUpdate()
        //{
        //    ReceivedPerson = _peopleCollection.Where(p => p.PersonID == ReceivedPersonID).FirstOrDefault();
        //    if (ReceivedPerson != null)
        //    {
        //        if (ReceivedPerson.Owner != null)
        //        {
        //            ReceivedOwner = ReceivedPerson.Owner;
        //        }
        //        this.Title = "Update Person's Owner Info";
        //    }
        //}

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
            ReceivedPerson = ((App)Application.Current)._peopleCollection.Where(p => p.PersonID == ReceivedPersonID).FirstOrDefault();

            if (ReceivedPerson != null)
            {
                ReceivedAgent = ReceivedPerson.Agent;

                var tempRECo = new RealEstateCompany();
                var tempHomesales = new List<HomeSale>();

                if (ReceivedAgent != null)
                {
                    tempRECo = ((App)Application.Current)._recosCollection.Where(re => re.CompanyID == ReceivedAgent.CompanyID).FirstOrDefault();
                    tempHomesales = ((App)Application.Current)._homeSalesCollection.Where(hs => hs.AgentID == ReceivedAgent.AgentID).ToList();

                    ReceivedAgent.Person = ReceivedPerson;
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
                    ExistingAgentsCombobox.IsEnabled = true;
                    LoadAgentsComboBox();
                }

                else if (ReceivedAgent == null)
                {
                    //ReceivedAgent = new Agent();
                    //var tempPerson = _peopleCollection.Where(p => p.PersonID == ReceivedPerson.PersonID).FirstOrDefault();
                    //tempRECo = null;
                    //tempHomesales = new List<HomeSale>();
                    //ReceivedAgent.Person = tempPerson;
                    //ReceivedAgent.RealEstateCompany = null;
                    AgentReCompanyTextbox.IsReadOnly = true;
                    AgentCommissionTextbox.IsEnabled = true;
                    //if (ReceivedPerson.Buyer != null)
                    //{
                    //    CreditRatingTextbox.Text = ReceivedPerson.Buyer.CreditRating?.ToString() ?? string.Empty;
                    //}
                    DisableBuyerDetailsControls();
                    DisableOwnerDetailsControls();
                    ExistingAgentsCombobox.IsEnabled = false;   //  user is here to create new Agent props for existing person
                    DisableEditingPersonBasicInformation(); //  user is NOT here to change the Person
                    AgentCommissionTextbox.Text = string.Empty;
                    AgentReCompanyTextbox.Text = string.Empty;
                }
                else
                {
                    var loadAgentPanelException = new Exception("Load Agent Panel method failed.");
                    throw loadAgentPanelException;
                }

            LoadRealEstateCoCombobox();
            }
        }

        /// <summary>
        /// Loads Buyer Details with received Buyer properties. Also load the Buyer Combobox.
        /// </summary>
        private void LoadBuyerPanel()
        {
            ReceivedPerson = ((App)Application.Current)._peopleCollection.Where(p => p.PersonID == ReceivedPersonID).FirstOrDefault();
            if (ReceivedPerson != null)
            {
                if (ReceivedPerson.Buyer != null)
                {
                    ReceivedBuyer = ReceivedPerson.Buyer;
                }
                this.Title = "Update Person's Buyer Info";

                var tempHomesales = ((App)Application.Current)._homeSalesCollection.Where(hs => hs.AgentID == ReceivedBuyer.BuyerID).ToList();

                //if (ReceivedPerson.Buyer != null)
                //{
                //    ReceivedBuyer.Person = ReceivedPerson;
                //}

                //if (tempHomesales.Count < 1)
                //{
                //    ReceivedBuyer.HomeSales = tempHomesales;
                //}

                CreditRatingTextbox.Text = ReceivedBuyer.CreditRating?.ToString() ?? string.Empty;
                CreditRatingTextbox.IsEnabled = true;
                DisableAgentDetailsControls();
                DisableOwnerDetailsControls();
                ExistingBuyersCombobox.IsEnabled = true;
                EnableEditingPersonBasicInformation();
                LoadBuyersComboBox();
            }
            else
            {
                var buyerInformationMissingException = new Exception("Load Buyer Panel method failed.");
                throw buyerInformationMissingException;
            }
        }

        /// <summary>
        /// Load Owner Details and the OWners ComboBox.
        /// </summary>
        private void LoadOwnerPanel()
        {
            ReceivedPerson = ((App)Application.Current)._peopleCollection.Where(p => p.PersonID == ReceivedPersonID).FirstOrDefault();
            if (ReceivedPerson != null)
            {
                if (ReceivedPerson.Owner != null)
                {
                    ReceivedOwner = ReceivedPerson.Owner;
                }
                this.Title = "Update Person's Owner Info";

                //var tempPerson = _peopleCollection.Where(p => p.PersonID == ReceivedOwner.OwnerID).FirstOrDefault();
                ////var tempHomes = MainWindow.homesCollection.Where(h => h.OwnerID == ReceivedOwner.OwnerID).ToList();
                //var tempHomes = _homesCollection.Where(h => h.OwnerID == ReceivedOwner.OwnerID).ToList();

                //if (tempPerson != null)
                //{
                //    ReceivedOwner.Person = tempPerson;
                //}

                //if (tempHomes != null)
                //{
                //    ReceivedOwner.Homes = tempHomes;
                //}

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
            else
            {
                var loadOwnersPanelException = new Exception("Load Owners Panel method failed.");
                throw loadOwnersPanelException;
            }

        }

        private void LoadRealEstateCoCombobox()
        {
            var comboBoxRECos = (from re in ((App)Application.Current)._recosCollection
                                 select re).ToList();
            ExistingRECoComboBox.ItemsSource = comboBoxRECos;
        }

        private void ExistingRECosCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayStatusMessage("Real Estate Co selection changed!");
            SelectedReco = new RealEstateCompany();
            SelectedReco = (sender as ComboBox).SelectedItem as RealEstateCompany;

            RealEstateCompany tempRECo = (from re in ((App)Application.Current)._recosCollection
                                          where SelectedReco.CompanyID == re.CompanyID
                                          select re).FirstOrDefault();

            Agent tempAgent = (from hs in ((App)Application.Current)._homeSalesCollection
                               from a in ((App)Application.Current)._peopleCollection
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
            Person tempPerson = ((App)Application.Current)._peopleCollection.Where(p => p.PersonID == comboBoxPerson.PersonID).FirstOrDefault();
            Agent tempAgent = (from p in ((App)Application.Current)._peopleCollection
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
            Person tempPerson = ((App)Application.Current)._peopleCollection.Where(p => p.PersonID == comboBoxPerson.PersonID).FirstOrDefault();
            Owner tempOwner = (from p in ((App)Application.Current)._peopleCollection
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
            Person tempPerson = ((App)Application.Current)._peopleCollection.Where(p => p.PersonID == comboBoxPerson.PersonID).FirstOrDefault();
            Buyer tempBuyer = (from p in ((App)Application.Current)._peopleCollection
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
            var listOfHomesalesAgents = (from hs in ((App)Application.Current)._homeSalesCollection
                                         from a in ((App)Application.Current)._peopleCollection
                                         where a.PersonID == hs.AgentID
                                         select a).ToList();
            ExistingAgentsCombobox.ItemsSource = listOfHomesalesAgents;

        }

        private void LoadBuyersComboBox()
        {
            var listOfBuyers = (from hs in ((App)Application.Current)._homeSalesCollection
                                from a in ((App)Application.Current)._peopleCollection
                                where a.PersonID == hs.BuyerID
                                select a).ToList();
            ExistingBuyersCombobox.ItemsSource = listOfBuyers;
        }

        private void LoadOwnersComboBox()
        {
            var listOfOwners = (from h in ((App)Application.Current)._homesCollection
                                from a in ((App)Application.Current)._peopleCollection
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
                    var inferredReco = (from re in ((App)Application.Current)._recosCollection
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
            var resultMessage = new StringBuilder();
            IsButtonClose = true;
            int personUpdateCount = ((App)Application.Current)._peopleCollection.UpdatePerson(ReceivedPerson);

            if (personUpdateCount == 1)
            {
                resultMessage.Append("Person information updated. ");
                this.IsButtonClose = true;
            }
            else
            {
                resultMessage.Append("No update to Person information. ");
                this.IsButtonClose = false;
            }

            int aboUpdated = 0;
            if (CalledByUpdateMenuType == "Agent")
            {
                ReceivedPerson.Agent = ReceivedAgent;
                ReceivedAgent.AgentID = ReceivedPerson.PersonID;
                aboUpdated = ((App)Application.Current)._peopleCollection.UpdateAgent(ReceivedAgent);
                if (aboUpdated == 1)
                {
                    resultMessage.Append("Agent information updated. ");
                    this.IsButtonClose = true;
                }
                else
                {
                    resultMessage.Append("Agent info NOT updated. ");
                    this.IsButtonClose = false;
                }

            }

            if (CalledByUpdateMenuType == "Buyer")
            {
                ReceivedPerson.Buyer = ReceivedBuyer;
                ReceivedBuyer.BuyerID = ReceivedPerson.PersonID;
                aboUpdated = ((App)Application.Current)._peopleCollection.UpdateBuyer(ReceivedBuyer);
                if (aboUpdated == 1)
                {
                    resultMessage.Append("Buyer information updated. ");
                    this.IsButtonClose = true;
                }
                else
                {
                    resultMessage.Append("Buyer info NOT updated. ");
                    this.IsButtonClose = false;
                }

            }

            if (CalledByUpdateMenuType == "Owner")
            {
                ReceivedPerson.Owner = ReceivedOwner;
                ReceivedOwner.OwnerID = ReceivedPerson.PersonID;
                aboUpdated = ((App)Application.Current)._peopleCollection.UpdateOwner(ReceivedOwner);
                if (aboUpdated == 1)
                {
                    resultMessage.Append("Owner information updated. ");
                    this.IsButtonClose = true;
                }
                else
                {
                    resultMessage.Append("Owner info NOT updated. ");
                    this.IsButtonClose = false;
                }

            }

            DisplayStatusMessage(resultMessage.ToString());
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
            //_peopleCollection = CollectionFactory.GetPeopleCollectionObject();
            //_homesCollection = CollectionFactory.GetHomesCollectionObject();
            //_homeSalesCollection = CollectionFactory.GetHomeSalesCollectionObject();
            //_recosCollection = CollectionFactory.GetRECosCollectionObject();
            
            personCollectionMonitor = ((App)Application.Current)._peopleCollection.collectionMonitor;
            personCollectionMonitor.Subscribe(this);
            recoCollectionMonitor = ((App)Application.Current)._recosCollection.collectionMonitor;
            recoCollectionMonitor.Subscribe(this);

            var ReceivedRECo = new RealEstateCompany();
            var SelectedRECo = new RealEstateCompany();
            LoadPersonInformation();
            logger = new Logger();
            string updateType = CalledByUpdateMenuType;
            switch (updateType)
            {
                case "AGENT":
                    {
                        LoadAgentPanel();
                        break;
                    }
                case "BUYER":
                    {
                        LoadBuyerPanel();
                        break;
                    }
                case "OWNER":
                    {
                        LoadOwnerPanel();
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

        #region IObserver

        //private IDisposable unsubscriber;
        private string notificationMessage;

        //public virtual void Subscribe(IObservable<NotificationData> provider)
        //{
        //    unsubscriber = provider.Subscribe(this);
        //}

        //public virtual void Unsubscribe()
        //{
        //    unsubscriber.Dispose();
        //}

        public void OnNext(NotificationData value)
        {
            string dataType = value.DataType.ToString();
            if (value.ChangeCount < 1)
            {
                notificationMessage = "Received a message with no applicable changes.";
                return;
            }

            notificationMessage = "Received an update to the ";

            switch (dataType)
            {
                case "Owner":
                    {
                        notificationMessage += " Owners combobox.";
                        LoadOwnersComboBox();
                        break;
                    }
                case "Buyer":
                    {
                        notificationMessage += " Buyers combobox.";
                        LoadBuyersComboBox();
                        break;
                    }
                case "Agent":
                    {
                        notificationMessage += " Agents combobox.";
                        LoadAgentsComboBox();
                        break;
                    }
                case "RECo":
                    {
                        notificationMessage += " RE Companies combobox.";
                        LoadRealEstateCoCombobox();
                        break;
                    }
                default:
                    break;
            }

        }

        public void OnError(Exception error)
        {
            //  TODO: set a logger to record this info
            ;
        }

        public void OnCompleted()
        {
            DisplayStatusMessage(notificationMessage);
            notificationMessage = "No new Notifications.";
        }

        #endregion
    }
}
