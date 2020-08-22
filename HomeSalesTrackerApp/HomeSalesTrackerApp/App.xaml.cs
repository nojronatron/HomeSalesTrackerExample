using HomeSalesTrackerApp.Helpers;
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
        public static bool _databaseInitLoaded = false;
        public static bool DatabaseInitLoaded { get; set; }
        public Logger HSTLogger { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  portions of code are thanks to a Gist by Ronnie Overby at https://gist.github.com/ronnieoverby/7568387 
            HSTLogger = new Logger();
            if (HSTLogger.IsEnabled) {
                AppDomain.CurrentDomain.UnhandledException += (sig, exc) =>
                    LogUnhandledException((Exception)exc.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                DispatcherUnhandledException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "TaskScheduler.UnobservedTaskException");

                HSTLogger.Data("Application Startup", "Initialize Database called");
                try
                {
                    if (DatabaseInitLoaded = LogicBroker.InitDatabase())
                    {
                        HSTLogger.Data("Application Startup", "Initialize Database completed. Launching UI.");
                        new MainWindow().Show();
                    }
                    else
                    {
                        HSTLogger.Data("Application Startup", "Application Startup failed to load file data.");
                    }
                    HSTLogger.Flush();
                }
                catch (Exception ex)
                {
                    HSTLogger.Data("WindowLoading Exception thrown", ex.Message);
                    HSTLogger.Flush(); _ = MessageBox.Show($"While launching, the application was unable to load the backup files. Application will now close.", "Unable to load file data.", MessageBoxButton.OK);
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (LogicBroker.BackUpDatabase())
            {
                HSTLogger.Data("App Exit", "Backup of files complete.");
            }
            else
            {
                HSTLogger.Data("App Exit", "Unable to backup to file system.");
                MessageBox.Show("Backup failed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            HSTLogger.Flush();
        }

        private void LogUnhandledException(Exception exception, string @event)
        {
            HSTLogger.Data("Unhandled Exception Catcher", "Next log entry will have exception and atEvent.");
            HSTLogger.Data(exception.ToString(), @event);
            HSTLogger.Flush();
        }
    }
}
