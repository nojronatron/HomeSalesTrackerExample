using HomeSalesTrackerApp.Helpers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddHomeView.xaml
    /// </summary>
    public partial class AddHomeView : UserControl, IObserver<NotificationData>
    {
        private string NotificationMessage { get; set; }
        public AddHomeView()
        {
            InitializeComponent();
        }

        private void RefreshOwnersCombobox()
        {
            var existingOwnersList = (from p in ((App)Application.Current)._peopleCollection
                                      where p.Owner != null
                                      select p).ToList();
            OwnersList.ItemsSource = existingOwnersList;
            OwnersList.SelectedIndex = -1;
        }

        public void OnCompleted()
        {
            ;
        }

        public void OnError(Exception error)
        {
            ;
        }

        public void OnNext(NotificationData value)
        {
            if (value.ChangeCount > 0)
            {
                if (value.DataType.Contains("Owner") || value.DataType.Contains("Person"))
                {
                    RefreshOwnersCombobox();
                    NotificationMessage = "Received an update to the Owners Combo Box.";
                }
            }

            else
            {
                NotificationMessage = "Received a message with no applicable changes.";
            }
        }
    }
}
