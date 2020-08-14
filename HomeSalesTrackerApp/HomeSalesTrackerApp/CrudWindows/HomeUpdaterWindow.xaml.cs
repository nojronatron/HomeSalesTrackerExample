using HomeSalesTrackerApp.DisplayModels;
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
        
        public string UpdateType { get; set; }
        public Person UpdatePerson { get; set; }
        public Person UpdateAgentPerson { get; set; }
        public Person UpdateBuyerPerson { get; set; }
        public Person UpdateOwnerPerson { get; set; }

        public Home UpdateHome { get; set; }
        public RealEstateCompany UpdateReco { get; set; }
        public HomeSale UpdateHomeSale { get; set; }
        public Agent UpdateAgent { get; set; }
        public Owner UpdateOwner { get; set; }
        public Buyer UpdateBuyer { get; set; }
                
        public HomeUpdaterWindow()
        {
            InitializeComponent();
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
                catch (Exception ex)
                {
                    var userResponse = MessageBox.Show("Save changes failed. Close anyway?", "Something went wrong!", MessageBoxButton.YesNo);
                    if (userResponse == MessageBoxResult.No)
                    {
                        IsButtonClose = false;
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Use the Close button or File -> Exit menut item. You will be prompted to save or discard changes before exiting.", "Warning!", MessageBoxButton.OK);
                IsButtonClose = false;
                e.Cancel = true;
            }

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int count = 0;
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
            if (count > 0)
            {
                LoadPanelsAndFields();
            }
            else
            {
                ShowCloseButtonOnly();
            }
        }

        private void LoadHomePanel()
        {
            //homeIdTextBox.Text = UpdateHome.HomeID.ToString();
            homeAddressTextbox.Text = UpdateHome.Address;
            homeCityTextbox.Text = UpdateHome.City;
            homeStateTextbox.Text = UpdateHome.State;
            homeZipTextbox.Text = UpdateHome.Zip;
            //  Note: Could add OwnerID field here but users should not need it
            updateHomePanel.Visibility = Visibility.Visible;
        }

        private void LoadRECoPanel()
        {
            //companyIdTextBox.Text = UpdateReco.CompanyID.ToString();
            companyNameTextbox.Text = UpdateReco.CompanyName;
            companyPhoneTextbox.Text = UpdateReco.Phone;
            updateRealEstateCoPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Update Selected Home For Sale. Allows updating existing Agent (Person and Agent types) and Home (Home type).
        /// </summary>
        private void LoadHomeSalesPanel()
        {
            SetDatePickerDefaults();

            //  Attached requirements: Update a selected HomeForSale with new/existing Agent, or a new Home.
            //hfsSaleIdTextBox.Text = UpdateHomeSale.SaleID.ToString();
            string TestMessage = UpdateHomeSale.SaleID.ToString();  //  might have to be a Property with get/set
            string homeForSaleAddressZip = $"{ UpdateHome.Address }, { UpdateHome.Zip }";
            hfsHomeIdTextbox.Text = homeForSaleAddressZip;
            //hfsSoldDateTextBox.Text = UpdateHomeSale.SoldDate.ToString();
            hfsSoldDatePicker.SelectedDate = UpdateHomeSale.SoldDate;
            hfsSaleAmountTextbox.Text = UpdateHomeSale.SaleAmount.ToString();
            hfsBuyerNameTextbox.Text = $"{ UpdateBuyerPerson.FirstName } {UpdateBuyerPerson.LastName }";

            var hfsExistingBuyersList = (from p in MainWindow.peopleCollection
                                         from hs in MainWindow.homeSalesCollection
                                         where p.PersonID == hs.BuyerID
                                         select p).ToList();

            hfsExistingBuyersCombobox.ItemsSource = hfsExistingBuyersList;

            //hfsMarketDateTextbox.Text = UpdateHomeSale.MarketDate.ToString();   //  might have to manage datetime formatting
            hfsMarketDatePicker.SelectedDate = UpdateHomeSale.MarketDate;
            hfsHomeIdTextbox.Text = $"{ UpdateAgentPerson.FirstName } { UpdateAgentPerson.LastName }";

            var hfsExistingAgentsList = (from p in MainWindow.peopleCollection
                                         from hs in MainWindow.homeSalesCollection
                                         where p.PersonID == hs.AgentID
                                         select p).ToList();

            hfsExistingAgentsCombobox.ItemsSource = hfsExistingAgentsList;

            companyNameTextbox.Text = UpdateReco.CompanyID.ToString();
        }

        private void SetDatePickerDefaults()
        {
            DateTime date1 = new DateTime(2020, 1, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2020, 1, 11, 0, 0, 0);
            TimeSpan fortnight = date2.Subtract(date1);
            hfsSoldDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.Now.Subtract(fortnight)));
        }

        /// <summary>
        /// Update HomeForSale as Sold. Allows updating SoldDate, SaleAmount, and associate an existing Buyer of type Buyer and of type Person.
        /// </summary>
        private void LoadHomeSoldPanel()
        {
            SetDatePickerDefaults();
            
            //  Attached requirements: Only SoldDate, SaleAmount, and Buyer information can be edited
            //hfsSaleIdTextBox.Text = UpdateHomeSale.SaleID.ToString();
            string TestMessage = UpdateHomeSale.SaleID.ToString();  //  might have to be a Property with get/set
            string homeForSaleAddressZip = $"{ UpdateHome.Address }, { UpdateHome.Zip }";
            
            hfsHomeIdTextbox.IsReadOnly = true;
            hfsHomeIdTextbox.Text = homeForSaleAddressZip;

            //hfsSoldDateTextBox.Text = UpdateHomeSale.SoldDate.ToString();
            hfsSoldDatePicker.SelectedDate = UpdateHomeSale.SoldDate;
            hfsSaleAmountTextbox.Text = UpdateHomeSale.SaleAmount.ToString();
            
            //  BUYER INFO
            //hfsBuyerIdTextbox.IsReadOnly = true;
            string buyerFirstLastname = $"{ UpdateBuyerPerson.FirstName } {UpdateBuyerPerson.LastName }";
            hfsBuyerNameTextbox.Text = buyerFirstLastname;

            var hfsExistingBuyersList = (from p in MainWindow.peopleCollection
                                         from hs in MainWindow.homeSalesCollection
                                         where p.PersonID == hs.BuyerID
                                         select p).ToList();

            hfsExistingBuyersCombobox.ItemsSource = hfsExistingBuyersList;
            var selectedBuyerIndex = hfsExistingBuyersList.FindIndex(p => p.PersonID == UpdateBuyerPerson.PersonID);
            hfsExistingBuyersCombobox.SelectedIndex = selectedBuyerIndex;


            //  MARKET DATE INFO
            //hfsMarketDateTextbox.Text = UpdateHomeSale.MarketDate.ToString();
            hfsMarketDatePicker.SelectedDate = UpdateHomeSale.MarketDate;

            hfsHomeIdTextbox.IsReadOnly = true;
            hfsHomeIdTextbox.Text = $"{ UpdateAgentPerson.FirstName } { UpdateAgentPerson.LastName }";

            var hfsExistingAgentsList = (from p in MainWindow.peopleCollection
                                         from hs in MainWindow.homeSalesCollection
                                         where p.PersonID == hs.AgentID
                                         select p).ToList();

            hfsExistingAgentsCombobox.ItemsSource = hfsExistingAgentsList;

            //hfsCompanyIdTextbox.IsReadOnly = true;
            companyNameTextbox.Text = UpdateReco.CompanyID.ToString();

            //  Update button: Will save changes via LogicBroker
            updateChangedHfsFields.Visibility = Visibility.Visible;
        }

        private void LoadAgentPanel()
        {
            //updateAgentAgentIdTextbox.Text = UpdateAgent.AgentID.ToString();
            UpdateAgentAgentPersonNameTextbox.Text = $"{UpdatePerson.FirstName} {UpdatePerson.LastName}";
            //updateAgentCompanyIdTextbox.Text = UpdateAgent.CompanyID.ToString();
            UpdateAgentCommissionTextbox.Text = UpdateAgent.CommissionPercent.ToString();
            var listOfHomesalesAgents = (from hs in MainWindow.homeSalesCollection
                                         from a in MainWindow.peopleCollection
                                         where a.PersonID == hs.AgentID
                                         select a).ToList();
            //  TODO: Fix the output so it displays data in the combobox instead of entity wrappers
            listOfExistingAgentsCombobox.ItemsSource = listOfHomesalesAgents;
        }

        private void ShowCloseButtonOnly()
        {
            updateHomePanel.Visibility = Visibility.Collapsed;
            updateRealEstateCoPanel.Visibility = Visibility.Collapsed;
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
                        LoadHomePanel();
                        break;
                    }
                case "REALESTATECOMPANY":
                    {
                        LoadRECoPanel();
                        break;
                    }
                case "HOMESALE":
                    {
                        LoadHomeSalesPanel();
                        break;
                    }
                case "HOMESOLD":
                    {
                        LoadHomeSoldPanel();
                        break;
                    }
                case "OWNER":
                    {
                        break;
                    }
                case "BUYER":
                    {
                        break;
                    }
                case "AGENT":
                    {
                        LoadAgentPanel();
                        break;
                    }
                default:
                    {
                        ShowCloseButtonOnly();
                        break;
                    }
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
            this.Close();
        }

        private void UpdateChangedRecoFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            if (LogicBroker.SaveEntity<RealEstateCompany>(UpdateReco))
            {
                DisplayStatusMessage("Changes saved!");
            }
            else
            {
                DisplayStatusMessage("Unable to save changes. Fill all required fields.");
            }

        }

        private void UpdateChangedHomeFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            if (LogicBroker.SaveEntity<Home>(UpdateHome))
            {
                DisplayStatusMessage("Changes saved!");
            }
            else
            {
                DisplayStatusMessage("Unable to save changes. Fill all required fields.");
            }

        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        private void UpdateChangedHfsFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateHomeSale != null) {
                if (UpdateType.Trim().ToUpper() == "HOMESOLD")
                {
                    //  the only field changes that matter are:
                    //      SoldDate
                    //      SaleAmount
                    //      Buyer instance (from comboBox)
                    //      Person instance of the Buyer that was selected
                    var soldDate = hfsSoldDatePicker.SelectedDate.Value;
                    Decimal saleAmount = Decimal.Parse(hfsSaleAmountTextbox.Text);
                    Person buyerPerson = hfsExistingBuyersCombobox.SelectedItem as Person;
                    Buyer selectedBuyer = (from hs in MainWindow.homeSalesCollection
                                           where hs.BuyerID == buyerPerson.PersonID
                                           select hs.Buyer).FirstOrDefault();
                    if (buyerPerson == null)
                    {
                        DisplayStatusMessage("Buyer Person could not be selected.");
                    }
                    else
                    {
                        UpdateHomeSale.SoldDate = soldDate;
                        UpdateHomeSale.SaleAmount = saleAmount;
                        UpdateHomeSale.BuyerID = selectedBuyer.BuyerID;
                        if (LogicBroker.UpdateEntity<HomeSale>(UpdateHomeSale)) 
                        {
                            DisplayStatusMessage("Home Sale saved to database!");
                        }
                        else
                        {
                            DisplayStatusMessage("No changes were saved.");
                        }
                    }

                }
                else
                {
                    DisplayStatusMessage("Cannot update HomeSale if it is missing.");
                }

            }
            else
            {
                //  UpdateType is "HOMESALE"
            }
        }

        private void UpdateChangedAgentFieldsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListOfExistingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateHomeForSaleFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            //  TODO: After user changes HomeForSale information and clicks this button the in memory object(s) must be updated

        }
    }
}
