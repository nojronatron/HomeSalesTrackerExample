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
using HSTDataLayer;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window
    {
        private Agent newAgent = null;
        private Buyer newBuyer = null;
        private Owner newOwner = null;
        private Person newPerson = null;
        public string AddType { get; set; }
        public AddPersonWindow()
        {
            InitializeComponent();
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            //  TODO: Add validation
            newOwner = new Owner()
            {
                PreferredLender = this.preferredLenderTextbox.Text.Trim()
            };

            newPerson = CreateNewPerson();
            newPerson.Owner = newOwner;
            //UpdatePersonCollection(newPerson);
            //LogicBroker.SaveEntity<Person>(newPerson);
            DisplayStatusMessage("Added new Owner.");
        }

        private static void UpdatePersonCollection(Person newPerson)
        {
            //  TODO: Add validation
            //MainWindow.peopleCollection.Add(newPerson);
            //  TODO: Refresh the People Collection with data from DB
            MainWindow.InitPeopleCollection();
            AddHomeWindow ahw = new AddHomeWindow();
            ahw.RefreshOwnersComboBox();
        }

        private void addBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            string credRating = this.creditRatingTextbox.Text.Trim();
            newBuyer = new Buyer()
            {
                CreditRating = int.Parse(credRating)
            };
            
            newPerson = CreateNewPerson();
            newPerson.Buyer = newBuyer;
            //UpdatePersonCollection(newPerson);

            DisplayStatusMessage("Added new Buyer.");
        }

        private void addAgentButton_Click(object sender, RoutedEventArgs e)
        {
            string commission = this.commissionTextbox.Text.Trim();
            newAgent = new Agent()
            {
                CommissionPercent = Decimal.Parse(commission)
            };

            newPerson = CreateNewPerson();
            newPerson.Agent = newAgent;
            //UpdatePersonCollection(newPerson);

            DisplayStatusMessage("Added new Agent.");
        }

        private Person CreateNewPerson()
        {
            //  TODO: Add valiations including nulls and type/length/formatting
            Person newPerson = new Person()
            {
                FirstName = this.fNameTextbox.Text.Trim(),
                LastName = this.lNameTextbox.Text.Trim(),
                Phone = this.phoneTextbox.Text.Trim(),
                Email = this.emailTextbox.Text.Trim(),
                //  Owner = newOwner
            };
            return newPerson;
        }

        private void DisplayStatusMessage(string message)
        {
            this.statusBarText.Text = message;
        }

        private void AddPersonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //  TODO: add person window closing: does it need to do anything else besidese just close?
            var userResponse = MessageBox.Show("Save changes?", "Changes not saved!", MessageBoxButton.YesNo);
            if (userResponse == MessageBoxResult.Yes)
            {
                SaveToEntities();
            }
            this.Close();
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SaveToEntities();
            MainWindow.InitPeopleCollection();
            //  TODO: init HomesCollection, HomeSalesCollection, and RECOsCollection
        }

        private void SaveToEntities()
        {
            if (newAgent != null)
            {
                LogicBroker.SaveEntity<Person>(newPerson);
            }
            if (newBuyer != null)
            {
                LogicBroker.SaveEntity<Person>(newPerson);
            }
            if (newOwner != null)
            {
                LogicBroker.SaveEntity<Person>(newPerson);
            }
        }
    }
}
