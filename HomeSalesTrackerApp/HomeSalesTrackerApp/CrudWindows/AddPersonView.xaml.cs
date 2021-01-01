using System;
using System.Windows;
using System.Windows.Controls;

namespace HomeSalesTrackerApp.CrudWindows
{
    /// <summary>
    /// Interaction logic for AddPersonView.xaml
    /// </summary>
    public partial class AddPersonView : UserControl
    {
        //  create an event to handle when a new Person has been created

        public static event EventHandler<string> RaiseNewPersonCreatedEvent;
        public AddPersonView()
        {
            InitializeComponent();
        }

        private void SavePersonButton_Click(object sender, RoutedEventArgs e)
        {
            //  trigger
            RaiseNewPersonCreatedEvent?.Invoke(this, e.OriginalSource.ToString());
        }
    }
}
