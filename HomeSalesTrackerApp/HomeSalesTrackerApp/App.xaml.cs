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
        public static bool databaseLoadCompleted = false;
        public static bool DatabaseLoadCompleted { get; set; }
        public Logger logger { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  portions of code are thanks to a Gist by Ronnie Overby at https://gist.github.com/ronnieoverby/7568387 
            logger = new Logger();
            if (logger.IsEnabled) {
                AppDomain.CurrentDomain.UnhandledException += (sig, exc) =>
                    LogUnhandledException((Exception)exc.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

                DispatcherUnhandledException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "Application.Current.DispatcherUnhandledException");

                TaskScheduler.UnobservedTaskException += (sig, exc) =>
                    LogUnhandledException(exc.Exception, "TaskScheduler.UnobservedTaskException");

                try
                {
                    if (DatabaseLoadCompleted = LogicBroker.LoadData()) 
                    {
                        new MainWindow().Show();
                    }
                    else
                    {
                        logger.Data("AppExit", "Application Startup failed to load file data.");
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"While launching, the application was unable to load the backup files. Application will now close.", "Unable to load file data.", MessageBoxButton.OK);

                }
                finally
                {
                    logger.Flush();
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (LogicBroker.BackUpDatabase())
            {
                logger.Data("App Exit", "Backup of files complete.");
            }
            else
            {
                logger.Data("App Exit", "Unable to backup to file system.");
                MessageBox.Show("Backup failed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            logger.Flush();
        }

        private void LogUnhandledException(Exception exception, string @event)
        {
            logger.Data(exception.ToString(), @event);
            logger.Flush();
        }
    }
}
