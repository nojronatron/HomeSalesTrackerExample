using HomeSalesTrackerApp.Helpers;

using HSTDataLayer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for UpdaterWindow.xaml
    /// </summary>
    public partial class HomeUpdaterWindow : Window, IObserver<NotificationData>
    {
        private bool IsButtonClose { get; set; }
        private bool BuyerUpdated { get; set; }
        private bool HomesaleUpdated { get; set; }
        private Logger logger = null;
        private CollectionMonitor collectionMonitor = null;

        public string UpdateType { get; set; }
        public Person UpdatePerson { get; set; }
        public Agent UpdateAgent { get; set; }
        public Owner UpdateOwner { get; set; }
        public Buyer UpdateBuyer { get; set; }
        public Home UpdateHome { get; set; }
        public HomeSale UpdateHomeSale { get; set; }
        public RealEstateCompany UpdateReco { get; set; }

        public HomeUpdaterWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsButtonClose)
            {
                e.Cancel = false;
            }
            else
            {
                var userResponse = MessageBox.Show("Save changes failed. Close anyway?", "Something went wrong!", MessageBoxButton.YesNo);

                if (userResponse == MessageBoxResult.No)
                {
                    logger.Data("HUW Window_Closing User response", "Close anyway? No");
                    IsButtonClose = false;
                    e.Cancel = true;
                }
                else
                {
                    logger.Data("HUW Window_Closing User response", "Close anyway? Yes");
                }

                logger.Flush();
                //MessageBox.Show("Use the Close button or File -> Exit menut item. You will be prompted to save or discard changes before exiting.", "Warning!", MessageBoxButton.OK);
                //IsButtonClose = false;
                e.Cancel = true;
            }

        }

        private void LoadAgentsCombobox(bool includeAllPeople)
        {
            var existingAgentPeopleList = new List<Person>();
            
            if (includeAllPeople)
            {
                existingAgentPeopleList = (from p in MainWindow.peopleCollection
                                           select p).ToList();
            }
            else
            {
                existingAgentPeopleList = (from p in MainWindow.peopleCollection
                                           where p.Agent != null
                                           select p).ToList();
            }
            
            existingAgentPeopleList.Distinct();
            ExistingAgentsCombobox.ItemsSource = existingAgentPeopleList;
        }

        private void LoadBuyersCombobox()
        {
            var existingBuyersList = (from p in MainWindow.peopleCollection
                                      select p).ToList();
            ExistingBuyersCombobox.ItemsSource = existingBuyersList;
        }

        /// <summary>
        /// Validate that minimal inputs are received when this Window Opens, otherwise turn off all editing options and only allow user to Close it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            collectionMonitor = MainWindow.peopleCollection.collectionMonitor;
            collectionMonitor.Subscribe(this);
            AddNewAgentButton.Visibility = Visibility.Hidden;
            AddNewAgentButton.IsEnabled = false;
            AddNewBuyerButton.Visibility = Visibility.Hidden;
            AddNewBuyerButton.IsEnabled = false;
            LoadBuyersCombobox();
            LoadAgentsCombobox(true);

            if (UpdateHome != null && UpdateHome.HomeID > -1)
            {
                UpdateHomeSale.HomeID = UpdateHome.HomeID;
            }

            logger = new Logger();
            int count = 0;

            if (UpdateHome != null)
            {
                count++;
            }

            if (UpdateType.Count() > 3)
            {
                count++;
            }

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

            if (count > 2)  //  UpdateHome and UpdateType are absolutely required
            {
                LoadPanelsAndFields();
            }
            else
            {
                ShowCloseButtonOnly();
            }

        }

        /// <summary>
        /// User is closing the Window (close without saving).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        /// <summary>
        /// Scenario: User want to update a Home For Sale as Sold with a Buyer and possibly new Agent and/or RECo.
        /// </summary>
        private void Case_HomeSold()
        {
            //  HOME INFO
            LoadHomeInfoFields();

            //  BUYER INFO
            BuyerUpdated = false;
            BuyerNameTextbox.IsReadOnly = true;
            BuyerCreditRatingTextbox.IsReadOnly = true;
            ExistingBuyersCombobox.IsEnabled = true;
            AddNewBuyerButton.Visibility = Visibility.Visible;
            AddNewBuyerButton.IsEnabled = true;
            LoadBuyersCombobox();

            //  HOMESALE INFO
            HomesaleUpdated = false;
            forSaleHomeIdTextbox.IsReadOnly = true;
            forSaleHomeIdTextbox.Text = UpdateHomeSale.HomeID.ToString();
            hfsSoldDatePicker.IsEnabled = true;
            hfsSoldDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-5)));
            hfsSoldDatePicker.SelectedDate = DateTime.Today;
            hfsSaleAmountTextbox.Text = UpdateHomeSale.SaleAmount.ToString();
            hfsMarketDatePicker.SelectedDate = UpdateHomeSale.MarketDate;
            hfsMarketDatePicker.IsEnabled = false;

            //  AGENT INFO
            AgentNameTextbox.IsReadOnly = true;
            UpdateAgentCompanyNameTextbox.IsReadOnly = true;
            UpdateAgentCommissionTextbox.IsReadOnly = true;
            updateChangedAgentFieldsButton.IsEnabled = true;
            AddNewAgentButton.Visibility = Visibility.Hidden;
            AddNewAgentButton.IsEnabled = false;
            
            if (UpdateAgent != null)
            {
                UpdateAgentCommissionTextbox.Text = UpdateAgent.CommissionPercent.ToString() ?? string.Empty;
                UpdateAgentCompanyNameTextbox.Text = UpdateReco.CompanyName.ToString() ?? "Agent no longer active";
                AgentNameTextbox.Text = UpdatePerson.GetFirstAndLastName();
            }
            
            LoadAgentsCombobox(true);
        }

        private void Case_AddHfsWithAgent()
        {
            //  HOME INFO
            LoadHomeInfoFields();

            //  HOMESALE INFO
            HomesaleUpdated = false;
            forSaleHomeIdTextbox.IsReadOnly = true;
            forSaleHomeIdTextbox.Text = UpdateHome.HomeID.ToString();
            hfsSoldDatePicker.SelectedDate = UpdateHomeSale.SoldDate;
            hfsSoldDatePicker.IsEnabled = false;
            hfsSaleAmountTextbox.Text = UpdateHomeSale.SaleAmount.ToString();
            hfsMarketDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-5)));
            hfsMarketDatePicker.SelectedDate = DateTime.Today;

            //  AGENT INFO
            AgentNameTextbox.IsReadOnly = true;
            UpdateAgentCompanyNameTextbox.IsReadOnly = true;
            UpdateAgentCommissionTextbox.IsReadOnly = true;
            updateChangedAgentFieldsButton.IsEnabled = true;
            AddNewAgentButton.Visibility = Visibility.Visible;
            AddNewAgentButton.IsEnabled = true;
            LoadAgentsCombobox(true);

            //  BUYER INFO
            BuyerUpdated = false;
            BuyerNameTextbox.IsReadOnly = true;
            BuyerCreditRatingTextbox.IsReadOnly = true;
            ExistingBuyersCombobox.IsEnabled = false;
        }

        private void ShowCloseButtonOnly()
        {
            CloseButton.Visibility = Visibility.Visible;
        }

        private void LoadPanelsAndFields()
        {
            CloseButton.Visibility = Visibility.Visible;
            string updateType = UpdateType.Trim().ToUpper();
            switch (updateType)
            {
                case "HOME":
                    {
                        this.Title = "Update Home Information";
                        LoadHomeInfoFields();
                        break;
                    }
                case "PUTONMARKET":
                    {
                        this.Title = "Update Home: Put on the Market";
                        Case_AddHfsWithAgent();
                        break;
                    }
                case "HOMESOLD":
                    {
                        this.Title = "Update Home Sale as SOLD";
                        Case_HomeSold();
                        break;
                    }
                case "OWNER":
                    {
                        this.Title = "Update Owner information";
                        LoadHomeInfoFields();
                        break;
                    }
                case "BUYER":
                    {
                        this.Title = "Update Buyer information";
                        LoadHomeInfoFields();
                        break;
                    }
                case "AGENT":
                    {
                        this.Title = "Update Agent information";
                        LoadHomeInfoFields();
                        break;
                    }
                default:
                    {
                        ShowCloseButtonOnly();
                        break;
                    }
            }

        }

        /// <summary>
        /// Loads caller's passed-in data into the Update Existing Home section of the Home Update Window
        /// </summary>
        private void LoadHomeInfoFields()
        {
            homeAddressTextbox.Text = UpdateHome.Address;
            homeCityTextbox.Text = UpdateHome.City;
            homeStateTextbox.Text = UpdateHome.State;
            homeZipTextbox.Text = UpdateHome.Zip;
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        /// <summary>
        /// In-memory objects are updated if SoldDate and SaleAmount fields are valid otherwise no update happens and user is prompted to try again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateHomeForSaleFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            int countChanges = 0;
            var updatedSoldDate = new DateTime();
            var updatedMarketDate = new DateTime();
            var updatedSaleAmount = new decimal();

            if (DateTime.TryParse(hfsSoldDatePicker.SelectedDate.ToString(), out updatedSoldDate))
            {
                countChanges++;
            }

            if (DateTime.TryParse(hfsMarketDatePicker.SelectedDate.ToString(), out updatedMarketDate))
            {
                countChanges++;
            }

            if (Decimal.TryParse(hfsSaleAmountTextbox.Text.Trim(), out updatedSaleAmount))
            {
                if (updatedSaleAmount > 0 && updatedSaleAmount < Decimal.MaxValue)
                {
                    countChanges++;
                }
            }

            if (countChanges >= 2)
            {
                if (updatedSoldDate.Year < 2020)
                {
                    UpdateHomeSale.SoldDate = null;
                }
                else
                {
                    UpdateHomeSale.SoldDate = updatedSoldDate;
                }

                UpdateHomeSale.MarketDate = updatedMarketDate;
                UpdateHomeSale.SaleAmount = updatedSaleAmount;
                DisplayStatusMessage("Home Sale Information Updated!");
                
                if (UpdateBuyer != null)
                {
                    UpdateHomeSale.BuyerID = UpdateBuyer.BuyerID;
                }

                HomesaleUpdated = true;
            }
            else
            {
                DisplayStatusMessage("Are you putting home on Market or has it Sold? Try again.");
            }

        }

        private void UpdateBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateBuyer != null)
            {
                if (!string.IsNullOrWhiteSpace(BuyerCreditRatingTextbox.Text))
                {
                    if (int.TryParse(BuyerCreditRatingTextbox.Text, out int credRating))
                    {
                        if (credRating < 300 || credRating > 850)
                        {
                            DisplayStatusMessage("Enter a valid Credit Rating between 300 and 850.");
                            UpdateBuyer.CreditRating = null;
                        }
                        else
                        {
                            UpdateBuyer.CreditRating = credRating;
                        }
                    }
                }
                UpdateHomeSale.BuyerID = UpdateBuyer.BuyerID;
                var buyerPerson = MainWindow.peopleCollection.Where(p => p.PersonID == UpdateBuyer.BuyerID).FirstOrDefault();
                if (buyerPerson != null)
                {
                    BuyerUpdated = true;
                    UpdateBuyer.Person = buyerPerson;
                    UpdateBuyer.HomeSales.Add(UpdateHomeSale);
                    DisplayStatusMessage("Buyer information updated!");
                }
                else
                {
                    DisplayStatusMessage("Selected Person could not be added as a Buyer.");
                }
            }
            else
            {
                DisplayStatusMessage("First, select a Person to buy this home.");
            }

        }

        private void UpdateChangedAgentFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateHomeSale.AgentID = UpdateAgent.AgentID;
            UpdateHomeSale.CompanyID = UpdateReco.CompanyID;
            DisplayStatusMessage("Agent information updated.");
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int savedCount = 0;
                string updateType = UpdateType.Trim().ToUpper();
                switch (updateType)
                {
                    case "PUTONMARKET":
                        {
                            savedCount += MainWindow.homeSalesCollection.Add(UpdateHomeSale);
                            break;
                        }
                    case "HOMESOLD":
                        {
                            if (BuyerUpdated && HomesaleUpdated)
                            {

                                Person buyerPerson = MainWindow.peopleCollection.Get(UpdateBuyer.BuyerID);
                                if (buyerPerson == null)
                                {
                                    break;
                                }

                                buyerPerson.Buyer.CreditRating = UpdateBuyer.CreditRating;
                                savedCount += MainWindow.peopleCollection.UpdatePerson(buyerPerson);

                                var homesaleToSave = new HomeSale()
                                {
                                    SaleID = UpdateHomeSale.SaleID,
                                    HomeID = UpdateHomeSale.HomeID,
                                    SoldDate = UpdateHomeSale.SoldDate,
                                    AgentID = UpdateHomeSale.AgentID,
                                    SaleAmount = UpdateHomeSale.SaleAmount,
                                    BuyerID = UpdateBuyer.BuyerID,
                                    MarketDate = UpdateHomeSale.MarketDate,
                                    CompanyID = UpdateHomeSale.CompanyID
                                };

                                savedCount += MainWindow.homeSalesCollection.Update(homesaleToSave);

                            }

                            break;
                        }
                    default:
                        {
                            DisplayStatusMessage("Nothing was saved.");
                            ShowCloseButtonOnly();
                            break;
                        }
                }

                if (savedCount < 1)
                {
                    DisplayStatusMessage("Home or Home Sale info required for: Agent, Buyer, and Owner changes.");
                }

                if (savedCount > 0)
                {
                    DisplayStatusMessage("Save completed! Click Close to exit.");
                    IsButtonClose = true;
                }

            }
            catch (Exception ex)
            {
                DisplayStatusMessage("Unable to save changes.");
                logger.Data("Home Updater Window Exception Caught!", ex.Message);
                logger.Flush();
                logger.Data("UpdateHome: ", UpdateHome?.ToString());
                logger.Flush();
                logger.Data("UpdateRealEstateCompany: ", UpdateReco?.ToString());
                logger.Flush();
                logger.Data("UpdateHomeSale (Market): ", UpdateHomeSale?.ToString());
                logger.Flush();
                logger.Data("UpdateBuyer: ", $"{ UpdateBuyer?.ToString() }");
                logger.Flush();
                logger.Data("UpdatedHomeSale (Sold): ", $"{ UpdateHomeSale?.ToString() }");
                logger.Flush();
            }

        }

        private void ListOfExistingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selectedAgent = (sender as ComboBox).SelectedItem as Person;

            if (selectedAgent != null)
            {
                UpdatePerson = selectedAgent;

                if (selectedAgent.Agent != null)
                {
                    UpdateAgent = selectedAgent.Agent;
                    AgentNameTextbox.Text = UpdatePerson.GetFirstAndLastName();

                    if (selectedAgent.Agent.CompanyID != null)
                    {
                        var agentsAffiliatedReco = (from re in MainWindow.reCosCollection
                                                    where selectedAgent.Agent.CompanyID == re.CompanyID
                                                    select re).FirstOrDefault();
                        UpdateAgentCompanyNameTextbox.Text = agentsAffiliatedReco.CompanyName;
                        UpdateReco = agentsAffiliatedReco;
                    }
                    else
                    {
                        UpdateAgentCompanyNameTextbox.Text = "Agent no longer active";
                    }

                    UpdateAgentCommissionTextbox.Text = UpdateAgent.CommissionPercent.ToString().Trim();
                }

                if (selectedAgent.Agent == null)
                {
                    AgentNameTextbox.IsReadOnly = true;
                    AddNewAgentButton.IsEnabled = true;
                    AddNewAgentButton.Visibility = Visibility.Visible;
                    UpdateAgentCompanyNameTextbox.Text = "Click ADD NEW.";
                    UpdateAgentCommissionTextbox.Text = "Click ADD NEW";
                }

                UpdateHomeSale.AgentID = UpdateAgent.AgentID;
            }
        }

        private void AddNewBuyer()
        {
            var apw = new AddPersonWindow();
            apw.AddType = "Buyer";
            apw.Title = "Add a new Buyer to the database";
            apw.Show();
        }

        private void AddNewAgent()
        {
            var apw = new AddPersonWindow();
            apw.AddType = "Agent";
            apw.Title = "Add a new Agent to the database";
            apw.Show();
        }

        private void ExistingBuyersCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selectedBuyer = (sender as ComboBox).SelectedItem as Person;

            if (selectedBuyer != null)
            {
                BuyerCreditRatingTextbox.IsEnabled = true;
                BuyerCreditRatingTextbox.IsReadOnly = false;
                UpdateBuyer = new Buyer()
                {
                    BuyerID = selectedBuyer.PersonID,
                    CreditRating = selectedBuyer.Buyer.CreditRating ?? null
                };

                BuyerNameTextbox.Text = $"{ selectedBuyer.FirstName } {selectedBuyer.LastName }";
                BuyerCreditRatingTextbox.Text = UpdateBuyer.CreditRating?.ToString() ?? string.Empty;
                UpdateBuyerButton.IsEnabled = true;
            }

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void AddNewAgentButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewAgent();
        }

        private void MenuRefreshAgents_Click(object sender, RoutedEventArgs e)
        {
            LoadAgentsCombobox(true);
            DisplayStatusMessage("Refreshed List of Agents.");
        }

        private void MenuRefreshBuyers_Click(object sender, RoutedEventArgs e)
        {
            LoadBuyersCombobox();
            DisplayStatusMessage("Refreshed List of Buyers.");
        }

        private void AddNewBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewBuyer();
        }

        #region IObserver

        private IDisposable unsubscriber;
        private string notificationMessage;

        public virtual void Subscribe(IObservable<NotificationData> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }

        public void OnNext(NotificationData value)
        {
            notificationMessage = "Received an update to the ";

            if (value.ChangeCount > 0)
            {
                if (value.DataType.Contains("Buyer"))
                {
                    LoadBuyersCombobox();
                    notificationMessage += " Buyers combobox.";
                }
                else if (value.DataType.Contains("Agent"))
                {
                    LoadAgentsCombobox(false);
                    notificationMessage += " Agents combobox.";
                }
                else if (value.DataType.Contains("Person"))
                {
                    LoadBuyersCombobox();
                    LoadAgentsCombobox(true);
                    notificationMessage += " Agents and Buyers comboboxes.";
                }
            }
            else
            {
                notificationMessage = "Received a message with no applicable changes.";
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
