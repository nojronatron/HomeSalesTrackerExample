using HomeSalesTrackerApp.Factory;
using HomeSalesTrackerApp.Helpers;
using HSTDataLayer;

using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window, IObserver<NotificationData>
    {
        private bool IsButtonClose = false;
        private Agent NewAgent = null;
        private Buyer NewBuyer = null;
        private Owner NewOwner = null;
        private Person NewPerson = null;
        private RealEstateCompany ExistingRECo = null;
        private Logger logger = null;
        private CollectionMonitor collectionMonitor = null;
        //private PeopleCollection<Person> _peopleCollection { get; set; }
        //private RealEstateCosCollection _reCosCollection { get; set; }
        public string AddType { get; set; }

        public AddPersonWindow()
        {
            InitializeComponent();
            logger = new Logger();
            //_peopleCollection = CollectionFactory.GetPeopleCollectionObject();
            //_reCosCollection = CollectionFactory.GetRECosCollectionObject();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveOwnerInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string preferredLenderText = this.PreferredLenderTextbox.Text.Trim();

            if (preferredLenderText.Length == 0)
            {
                DisplayStatusMessage("Preferred Lender will be blank (no preferred lender).");
                preferredLenderText = string.Empty;
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

            if (phone.Length <= 10)
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

                if (((App)Application.Current)._peopleCollection.Add(NewPerson) > 0)
                {
                    DisplayStatusMessage("Person created!");
                    result = true;
                }
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
            var recosList = (from re in ((App)Application.Current)._recosCollection
                             select re).ToList();
            ExistingRECoComboBox.ItemsSource = recosList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            collectionMonitor = ((App)Application.Current)._recosCollection.collectionMonitor;
            collectionMonitor.Subscribe(this);
            LoadRECoComboBox();

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
            if (CreateNewPerson())
            {

                if (NewPerson.PersonID < 0)
                {
                    DisplayStatusMessage($"Unable to Save. Close Window and try again.");
                    logger.Data("CreateThisPersonButton <Person> Warning: ", $"{ this.NewPerson?.ToString() }");
                    this.NewPerson = null;
                    logger.Data("CreateThisPersonButton <Agent> Warning: ", $"{ this.NewAgent?.ToString() }");
                    this.NewAgent = null;
                    logger.Data("CreateThisPersonButton <Buyer> Warning: ", $"{ this.NewBuyer?.ToString() }");
                    this.NewBuyer = null;
                    logger.Data("CreateThisPersonButton <Owner> Warning: ", $"{ this.NewOwner?.ToString() }");
                    this.NewOwner = null;
                    logger.Flush();
                    return;
                }

                if (NewAgent != null)
                {
                    NewAgent.AgentID = NewPerson.PersonID;
                    itemsCount += ((App)Application.Current)._peopleCollection.UpdateAgent(NewAgent);
                }

                if (NewBuyer != null)
                {
                    NewBuyer.BuyerID = NewPerson.PersonID;
                    itemsCount += ((App)Application.Current)._peopleCollection.UpdateBuyer(NewBuyer);
                }

                if (NewOwner != null)
                {
                    NewOwner.OwnerID = NewPerson.PersonID;
                    itemsCount += ((App)Application.Current)._peopleCollection.UpdateOwner(NewOwner);
                }

                if (itemsCount > 0)
                {
                    DisplayStatusMessage($"{ AddType } saved! Click Close button to exit this window.");
                }

            }
            else
            {
                IsButtonClose = false;
            }

        }

        private void ExistingRECosCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRECo = (sender as ComboBox).SelectedItem as RealEstateCompany;
            ExistingRECo = ((App)Application.Current)._recosCollection.Where(re => re.CompanyID == selectedRECo.CompanyID).FirstOrDefault();
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

        #region collectionmonitor
        
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
            if (value.ChangeCount > 0 && value.DataType.Contains("RECo"))
            {
                LoadRECoComboBox();
                notificationMessage = "Received an update to the Real Estate Companies combobox.";
            }

            else
            {
                notificationMessage = "Received a message with no applicable changes.";
            }

        }

        public void OnError(Exception error)
        {
            //  TODO: Set a logger to record this info
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
