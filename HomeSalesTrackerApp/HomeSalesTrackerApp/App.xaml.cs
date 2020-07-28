using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool databaseLoadCompleted = false;
        public static bool DatabaseLoadCompleted { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  TODO: Call DB init and data loading routine/s here
            //LogicBroker.InitDatabase();
            try
            {
                DatabaseLoadCompleted = LogicBroker.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ ex.Message }", "An Exception has been thrown.");

            }
            //DatabaseLoadCompleted = false;
            
            //  TODO: Call any other pre-UI method/s here

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //  TODO: Call to flush DB contents to XML files here
            //LogicBroker.BackUpDatabase();

            //  TODO: Call any other cleanup method/s here

        }
    }
}
