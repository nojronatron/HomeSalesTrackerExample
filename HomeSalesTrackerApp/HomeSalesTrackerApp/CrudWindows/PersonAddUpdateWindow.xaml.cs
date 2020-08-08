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
            agentIdTextbox.Text = UpdateAgent.AgentID.ToString();
            fNameTextbox.Text = UpdatePerson.FirstName.ToString();
            lNameTextbox.Text = UpdatePerson.LastName.ToString();
            phoneTextbox.Text = UpdatePerson.Phone.ToString();
            emailTextbox.Text = UpdatePerson.Email.ToString();
            //  query the recoCollection to get RE Company Name

            if (UpdateAgent.CompanyID == null)
            {
                agentRecoTextbox.Text = "Agent no longer active";
            }
            else
            {
                RealEstateCompany tempRECo = (from reco in MainWindow.reCosCollection
                                              where reco.CompanyID == UpdateAgent.CompanyID
                                              select reco).FirstOrDefault();


                agentRecoTextbox.Text = tempRECo.CompanyName.ToString();
            }

            agentCommissionTextbox.Text = UpdateAgent.CommissionPercent.ToString();

            RefreshAgentsComboBox();
            
            closeButton.Visibility = Visibility.Visible;
        }
        
        private void addAgentButton_Click(object sender, RoutedEventArgs e)
        {
            //  If NEW AGENT is necessary then this button will allow creating new Agent Data

        }


        private void existingAgentsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs sceArgs)
        {
            DisplayStatusMessage("Updated list of Agents.");
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
                        // LoadOwnerPanel();
                        break;
                    }
                case "BUYER":
                    {
                        //  LoadBuyerPanel();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            IsButtonClose = false;
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            IsButtonClose = true;
        }
    }
}
