using HomeSalesTrackerApp.DisplayModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddPersonView.xaml
    /// </summary>
    public partial class AddPersonView : UserControl
    {
        //  create an event to handle when a new Person has been created
        public static event EventHandler<string> RaiseNewPersonCreatedEvent;
        public AgentModel NewAgent { get; set; }
        public BuyerModel NewBuyer { get; set; }
        public OwnerModel NewOwner { get; set; }
        public AddPersonView()
        {
            InitializeComponent();
            AddPersonViewModel.PropertyChanged += AddPersonViewModel_PropertyChanged;
        }

        private void AddPersonViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.ToString())
            {
                case "Agent":
                    {
                        this.AgentStackpanel.Visibility = Visibility.Visible;
                        this.RecoCombobox.ItemsSource = ((App)Application.Current)._recosCollection.Where(re => re.CompanyName != null).ToList();
                        this.RecoCombobox.SelectedIndex = 0;
                        NewAgent = new AgentModel();
                        break;
                    }
                case "Buyer":
                    {
                        NewBuyer = new BuyerModel();
                        break;
                    }
                case "Owner":
                    {
                        NewOwner = new OwnerModel();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void SavePersonButton_Click(object sender, RoutedEventArgs e)
        {
            //  trigger
            RaiseNewPersonCreatedEvent?.Invoke(this, e.OriginalSource.ToString());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
