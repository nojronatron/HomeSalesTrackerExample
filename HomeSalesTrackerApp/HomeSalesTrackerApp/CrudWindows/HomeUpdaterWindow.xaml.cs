using HomeSalesTrackerApp.DisplayModels;
using HomeSalesTrackerApp.Helpers;
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
    public partial class HomeUpdaterWindow : Window
    {
        private bool IsButtonClose { get; set; }
        private Logger logger = null;

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

        private void SetDatePickerDefaults()
        {
            DateTime date1 = new DateTime(2020, 1, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2020, 1, 11, 0, 0, 0);
            TimeSpan fortnight = date2.Subtract(date1);
            CalendarDateRange calendarDateRange = new CalendarDateRange(DateTime.Now, DateTime.Now.Subtract(fortnight));
            hfsSoldDatePicker.BlackoutDates.Add(calendarDateRange);
            hfsMarketDatePicker.BlackoutDates.Add(calendarDateRange);
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
                    MainWindow.InitHomeSalesCollection();
                }
                catch
                {
                    logger.Data("HUW Window_Closing Exception", "Save changes failed. Close anyway?");
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
                }
            }
            else
            {
                MessageBox.Show("Use the Close button or File -> Exit menut item. You will be prompted to save or discard changes before exiting.", "Warning!", MessageBoxButton.OK);
                IsButtonClose = false;
                e.Cancel = true;
            }

        }

        private bool RehydrateAgent()
        {
            Person tempAgent = null;
            if (UpdateAgent == null && UpdatePerson != null)
            {
                tempAgent = MainWindow.peopleCollection.Where(p => p.PersonID == UpdatePerson.PersonID).FirstOrDefault();
            }
            else if (UpdateAgent != null)
            {
                tempAgent = MainWindow.peopleCollection.Where(p => p.PersonID == UpdateAgent.AgentID).FirstOrDefault();
            }

            if (tempAgent != null)
            {
                UpdateAgent = tempAgent.Agent;
                RealEstateCompany tempReco = MainWindow.reCosCollection.Where(re => re.CompanyID == tempAgent.Agent.CompanyID).FirstOrDefault();
                if (tempReco != null)
                {
                    UpdateAgent.RealEstateCompany = tempReco;
                    UpdateAgent.CompanyID = tempReco.CompanyID;
                }

                List<HomeSale> tempHomesalesList = MainWindow.homeSalesCollection.Where(hs => hs.AgentID == tempAgent.Agent.AgentID).ToList();
                if (tempHomesalesList != null)
                {
                    UpdateAgent.HomeSales = tempHomesalesList;
                }
                return true;

            }

            return false;
        }

        private bool RehydrateBuyer()
        {
            if (UpdateBuyer != null)
            {
                Person tempBuyer = MainWindow.peopleCollection.Where(p => p.PersonID == UpdateBuyer.BuyerID).FirstOrDefault();
                if (tempBuyer != null)
                {
                    UpdateBuyer = tempBuyer.Buyer;
                    List<HomeSale> tempHomesalesList = MainWindow.homeSalesCollection.Where(hs => hs.BuyerID == tempBuyer.Buyer.BuyerID).ToList();
                    if (tempHomesalesList.Count > 0)
                    {
                        UpdateBuyer.HomeSales = tempHomesalesList;
                    }
                    return true;

                }
            }

            return false;
        }

        private bool RehydrateOwner()
        {
            //  TODO: Wire up RehydrateOwner for the appropriate code path
            if (UpdateOwner != null)
            {
                Person tempOwner = MainWindow.peopleCollection.Where(p => p.PersonID == UpdateOwner.OwnerID).FirstOrDefault();
                if (tempOwner != null)
                {
                    UpdateOwner = tempOwner.Owner;
                    List<Home> tempHomesList = MainWindow.homesCollection.Where(h => h.OwnerID == UpdateOwner.OwnerID).ToList();
                    if (tempHomesList.Count > 0)
                    {
                        UpdateOwner.Homes = tempHomesList;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool RehydrateHome()
        {
            //  TODO: Wire up RehydrateHome for an appropriate code path
            if (UpdateHome != null)
            {
                Home tempHome = MainWindow.homesCollection.Where(h => h.HomeID == UpdateHome.HomeID).FirstOrDefault();
                if (tempHome != null)
                {
                    UpdateHome = tempHome;
                    return true;
                }
            }
            return false;
        }

        private bool RehydrateHomeSale()
        {
            //  TODO: Wire up RehydrateHomeSale for an appropriate code path
            if (UpdateHomeSale != null)
            {
                HomeSale tempHomeSale = MainWindow.homeSalesCollection.Where(hs => hs.HomeID == UpdateHomeSale.HomeID).FirstOrDefault();
                if (tempHomeSale != null)
                {
                    UpdateHomeSale = tempHomeSale;
                    UpdateHomeSale.Agent = tempHomeSale.Agent;
                    UpdateHomeSale.Buyer = tempHomeSale.Buyer;
                    RealEstateCompany tempReco = MainWindow.reCosCollection.Where(re => re.CompanyID == UpdateHomeSale.AgentID).FirstOrDefault();
                    if (tempReco != null)
                    {
                        UpdateHomeSale.RealEstateCompany = tempReco;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool RehydrateRealEstateCompany()
        {
            if (UpdateReco != null)
            {
                RealEstateCompany tempRECo = MainWindow.reCosCollection.Where(re => re.CompanyID == UpdateReco.CompanyID).FirstOrDefault();
                if (tempRECo != null)
                {
                    UpdateReco = tempRECo;
                    List<HomeSale> tempHomeSalesList = MainWindow.homeSalesCollection.Where(hs => hs.CompanyID == UpdateReco.CompanyID).ToList();
                    if (tempHomeSalesList.Count > 0)
                    {
                        UpdateReco.HomeSales = tempHomeSalesList;
                    }
                    List<Agent> tempAgentList = (from p in MainWindow.peopleCollection
                                                 from hs in MainWindow.homeSalesCollection
                                                 where p.PersonID == hs.AgentID &&
                                                 hs.Agent.CompanyID == UpdateReco.CompanyID
                                                 select hs.Agent).ToList();
                    if (tempAgentList.Count > 0)
                    {
                        UpdateReco.Agents = tempAgentList;
                    }

                    return true;
                }
            }

            return false;
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
                                      where p.Buyer != null
                                      select p).ToList();
            ExistingBuyersCombobox.ItemsSource = existingBuyersList;
        }

        private void LoadRECosCombobox()
        {
            var existingRECosList = (from re in MainWindow.reCosCollection
                                     select re).ToList();
            ExistingRecosCombobox.ItemsSource = existingRECosList;
        }

        /// <summary>
        /// Validate that minimal inputs are received when this Window Opens, otherwise turn off all editing options and only allow user to Close it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRECosCombobox();
            LoadBuyersCombobox();
            LoadAgentsCombobox(true);

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //  Close Without Saving button
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
            UpdateChangedHomeFields_Button.IsEnabled = false;

            //  BUYER INFO
            BuyerNameTextbox.IsReadOnly = true;
            LoadBuyersCombobox();

            //  RECO INFO
            companyNameTextbox.Text = UpdateReco.CompanyName.ToString();
            companyNameTextbox.IsReadOnly = true;
            companyPhoneTextbox.Text = UpdateReco.Phone.ToString();
            companyPhoneTextbox.IsReadOnly = true;
            UpdateRECoFieldsButton.IsEnabled = false;
            LoadRECosCombobox();

            //  HOMESALE INFO
            SetDatePickerDefaults();
            forSaleHomeIdTextbox.IsReadOnly = true;
            forSaleHomeIdTextbox.Text = UpdateHomeSale.HomeID.ToString();
            hfsSoldDatePicker.SelectedDate = UpdateHomeSale.SoldDate;
            hfsSaleAmountTextbox.Text = UpdateHomeSale.SaleAmount.ToString();
            hfsMarketDatePicker.SelectedDate = UpdateHomeSale.MarketDate;
            hfsMarketDatePicker.IsEnabled = false;

            //  AGENT INFO
            AgentNameTextbox.IsReadOnly = true;
            UpdateAgentCompanyNameTextbox.IsReadOnly = true;
            UpdateAgentCommissionTextbox.IsReadOnly = true;
            updateChangedAgentFieldsButton.IsEnabled = true;
            LoadAgentsCombobox(false);
        }

        private void Case_AddHfsWithAgent()
        {
            //  HOME INFO
            LoadHomeInfoFields();
            UpdateChangedHomeFields_Button.IsEnabled = false;

            //  HOMESALE INFO
            SetDatePickerDefaults();
            forSaleHomeIdTextbox.IsReadOnly = true;
            forSaleHomeIdTextbox.Text = UpdateHome.HomeID.ToString();
            hfsSoldDatePicker.SelectedDate = UpdateHomeSale.SoldDate;
            hfsSoldDatePicker.IsEnabled = false;
            hfsSaleAmountTextbox.Text = UpdateHomeSale.SaleAmount.ToString();
            hfsMarketDatePicker.SelectedDate = UpdateHomeSale.MarketDate;
            hfsMarketDatePicker.IsEnabled = true;

            //  AGENT INFO
            AgentNameTextbox.IsReadOnly = true;
            UpdateAgentCompanyNameTextbox.IsReadOnly = true;
            UpdateAgentCommissionTextbox.IsReadOnly = true;
            updateChangedAgentFieldsButton.IsEnabled = true;
            LoadAgentsCombobox(true);
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
                case "REALESTATECOMPANY":
                    {
                        this.Title = "Update Real Estate Company";
                        LoadRECoInfoFields();
                        break;
                    }
                case "HOMESALE":
                    {
                        this.Title = "Update Home Sale Information";
                        Case_AddHfsWithAgent();
                        break;
                    }
                case "HOMESOLD":
                    {
                        this.Title = "Update Home Sale as SOLD!";
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
                        //  TODO: anything else???
                        break;
                    }
                default:
                    {
                        ShowCloseButtonOnly();
                        break;
                    }
            }

        }

        private void LoadRECoInfoFields()
        {
            //  TODO: code this function: called by LoadPanelsAndFields()

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
            DateTime updatedSoldDate = new DateTime();
            DateTime updatedMarketDate = new DateTime();
            Decimal updatedSaleAmount = new decimal();

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
                    UpdateHomeSale.Buyer = UpdateBuyer;
                    UpdateHomeSale.BuyerID = UpdateBuyer.BuyerID;
                }
                if (UpdateHome != null)
                {
                    UpdateHomeSale.Home = UpdateHome;
                    UpdateHomeSale.HomeID = UpdateHome.HomeID;
                }

            }
            else
            {
                DisplayStatusMessage("Only update Sale Amount and Market Date or Sold Date.");
            }

        }

        /// <summary>
        /// In-memory objects are updated if Address, City, State, and Zip fields are valid otherwise no update happens and user is prompted to try again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateChangedHomeFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            int countChanges = 0;
            string address = homeAddressTextbox.Text.Trim();
            string city = homeCityTextbox.Text.Trim();
            string state = homeStateTextbox.Text.Trim();
            string zip = homeZipTextbox.Text.Trim();
            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state) || string.IsNullOrWhiteSpace(zip))
            {
                DisplayStatusMessage("Unable to update. Address, City, State, and Zip are required.");
            }

            if (address.Length < 50 || city.Length > 30 || state.Length > 2 || zip.Length > 9)
            {
                DisplayStatusMessage("Maximum characters exceeded: Address 50; City 30; State 2; Zip 9");
            }
            else
            {
                if (address.Length > 0)
                {
                    countChanges++;
                }
                if (city.Length > 0)
                {
                    countChanges++;
                }
                if (state.Length > 0)
                {
                    countChanges++;
                }
                if (zip.Length > 4)
                {
                    countChanges++;
                }
            }

            if (countChanges == 4)
            {
                UpdateHome.Address = address;
                UpdateHome.City = city;
                UpdateHome.State = state;
                UpdateHome.Zip = zip;
                DisplayStatusMessage("Home Information Updated!");
            }
            else
            {
                DisplayStatusMessage("Unable to make updates. Check entries and try again.");
            }

        }

        private void UpdateBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateBuyer != null)
            {
                UpdateHomeSale.Buyer = UpdateBuyer;
                DisplayStatusMessage("Buyer information updated!");
            }
            else
            {
                DisplayStatusMessage("Select an Existing Buyer first.");
            }

        }

        private void UpdateChangedRecoFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateHomeSale.CompanyID = UpdateReco.CompanyID;
            UpdateHomeSale.RealEstateCompany = UpdateReco;
            DisplayStatusMessage("Real Estate Company updated.");
        }

        private void UpdateChangedAgentFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateHomeSale.AgentID = UpdateAgent.AgentID;
            UpdateHomeSale.Agent = UpdateAgent;
            UpdateHomeSale.CompanyID = UpdateReco.CompanyID;
            UpdateHomeSale.RealEstateCompany = UpdateReco;
            DisplayStatusMessage("UpdateChangedAgentFieldsButton Clicked!");
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int savedCount = 0;
                string updateType = UpdateType.Trim().ToUpper();
                switch (updateType)
                {
                    case "HOME":
                        {
                            if (LogicBroker.UpdateEntity<Home>(UpdateHome))
                            {
                                savedCount++;
                            }

                            break;
                        }
                    case "REALESTATECOMPANY":
                        {
                            if (LogicBroker.UpdateEntity<RealEstateCompany>(UpdateReco))
                            {
                                savedCount++;
                            }

                            break;
                        }
                    case "HOMESALE":
                        {
                            if (LogicBroker.SaveEntity<HomeSale>(UpdateHomeSale))
                            {
                                savedCount++;
                            }

                            break;
                        }
                    case "HOMESOLD":
                        {
                            if (LogicBroker.UpdateEntity<HomeSale>(UpdateHomeSale))
                            {
                                savedCount++;
                            }

                            break;
                        }
                    case "OWNER":
                        {
                            savedCount = -1;
                            break;
                        }
                    case "BUYER":
                        {
                            savedCount = -1;
                            break;
                        }
                    case "AGENT":
                        {
                            savedCount = -1;
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
                }
            }
            catch
            {
                throw;
            }
        }

        private void ListOfExistingRecosCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RealEstateCompany selectedReco = (sender as ComboBox).SelectedItem as RealEstateCompany;
            if (selectedReco != null)
            {
                UpdateReco = selectedReco;
                RehydrateRealEstateCompany();
            }
            companyNameTextbox.Text = UpdateReco.CompanyName;
            companyPhoneTextbox.Text = UpdateReco.CompanyName;

        }

        private void ListOfExistingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selectedAgent = (sender as ComboBox).SelectedItem as Person;

            if (selectedAgent != null)
            {
                UpdatePerson = selectedAgent;
                UpdateAgent = selectedAgent.Agent;
                //if (RehydrateAgent() != false)
                //{
                //    logger.Data("ListOfExistingAgentsCombobox_SelectionChanged", "RehydrageAgent returned False.");
                //    logger.Flush();
                //    DisplayStatusMessage("Working.");
                //}

                //if (UpdateAgent != null)
                //{
                    AgentNameTextbox.Text = $"{ UpdateAgent.Person.FirstName } { UpdateAgent.Person.LastName }";
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
                else
                {
                    //  TODO: Test Enabling an Add New button to allow user to create a new Agent for Person SelectedAgent in a PUW window
                    AddNewAgentButton.Visibility = Visibility.Visible;
                    AddNewAgentButton.IsEnabled = true;
                    AgentNameTextbox.Text = selectedAgent.GetFirstAndLastName();
                    AgentNameTextbox.IsReadOnly = true;
                    UpdateAgentCompanyNameTextbox.Text = "Not an agent. Click Add New.";
                    UpdateAgentCommissionTextbox.Text = string.Empty;
                    UpdatePerson = selectedAgent;   //  set UpdatePerson so it can be used 
                    UpdateAgent = (from p in MainWindow.peopleCollection
                                   where p.PersonID == UpdatePerson.PersonID
                                   select p.Agent).FirstOrDefault();
                        //MainWindow.peopleCollection.Where(p => p.PersonID == UpdatePerson.PersonID).FirstOrDefault();  //  set UpdateAgent so it can be used
                }
            //}
            //else
            //{
            //    logger.Data("Existing Agents combobox selection changed", "A selected Agent could not be acquired.");
            //    logger.Flush();
            //    DisplayStatusMessage("Unable to use selected item. Menu > Refresh could help.");
            //}
            UpdateHomeSale.Agent = UpdateAgent; //  this forces the changes to UpdateHomeSale so the correct Agent is saved along with the HomeSale record update.
        }

        private void ExistingBuyersCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selectedBuyer = (sender as ComboBox).SelectedItem as Person;
            if (selectedBuyer != null)
            {
                UpdateBuyer = selectedBuyer.Buyer;
                RehydrateBuyer();
                BuyerNameTextbox.Text = $"{ UpdateBuyer.Person.FirstName } {UpdateBuyer.Person.LastName }";
            }

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void AddNewAgentButton_Click(object sender, RoutedEventArgs e)
        {
            //  This button only appears when a non-Agent Person is selected in the Existing Agents combobox
            //      and it allows user to create a new Agent via PersonUpaterWindow
            //  TODO: Examine what UpdateAgent looks like prior to editing a Person to add Agent to their instance.
            var puw = new PersonUpdaterWindow();
            puw.CalledByUpdateMenuType = "Agent";
            puw.CalledByUpdateMenu = false;
            puw.ReceivedPerson = UpdatePerson;
            puw.Show();
        }

        private void MenuRefreshAgents_Click(object sender, RoutedEventArgs e)
        {
            LoadAgentsCombobox(false);
            DisplayStatusMessage("Refreshed List of Agents.");
        }

        private void MenuRefreshBuyers_Click(object sender, RoutedEventArgs e)
        {
            LoadBuyersCombobox();
            DisplayStatusMessage("Refreshed List of Buyers.");
        }

        private void MenuRefreshAgentsInclusive_Click(object sender, RoutedEventArgs e)
        {
            LoadAgentsCombobox(true);
            DisplayStatusMessage("Refreshed Agents list including eligible people.");
        }
    }
}
