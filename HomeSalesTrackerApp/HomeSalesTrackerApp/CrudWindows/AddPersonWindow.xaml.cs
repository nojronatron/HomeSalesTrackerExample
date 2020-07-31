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
            var newOwner = new Owner()
            {
                PreferredLender = this.preferredLenderTextbox.Text.Trim()
            };

            var newPerson = CreateNewPerson();
            newPerson.Owner = newOwner;
            UpdatePersonCollection(newPerson);
            LogicBroker.SaveEntity<Person>(newPerson);
            DisplayStatusMessage("Added new Owner.");
        }

        private static void UpdatePersonCollection(Person newPerson)
        {
            //  TODO: Add validation
            MainWindow.peopleCollection.Add(newPerson);
            AddHomeWindow ahw = new AddHomeWindow();
            ahw.RefreshOwnersComboBox();
        }

        private void addBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            string credRating = this.creditRatingTextbox.Text.Trim();
            var newBuyer = new Buyer()
            {
                CreditRating = int.Parse(credRating)
            };
            
            var newPerson = CreateNewPerson();
            newPerson.Buyer = newBuyer;
            UpdatePersonCollection(newPerson);

            DisplayStatusMessage("Added new Buyer.");
        }

        private void addAgentButton_Click(object sender, RoutedEventArgs e)
        {
            string commission = this.commissionTextbox.Text.Trim();
            var newAgent = new Agent()
            {
                CommissionPercent = Decimal.Parse(commission)
            };

            var newPerson = CreateNewPerson();
            newPerson.Agent = newAgent;
            UpdatePersonCollection(newPerson);

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

    }
}
