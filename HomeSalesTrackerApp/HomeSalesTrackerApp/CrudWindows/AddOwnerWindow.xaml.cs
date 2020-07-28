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
    /// Interaction logic for AddOwnerWindow.xaml
    /// </summary>
    public partial class AddOwnerWindow : Window
    {
        public Owner NewOwner = new Owner();

        public AddOwnerWindow(Owner new_owner)
        {
            InitializeComponent();
            NewOwner = new_owner;
        }

        private void addOwnerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void menuRefresh_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
