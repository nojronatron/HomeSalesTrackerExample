using HomeSalesTrackerApp.Helpers;

using HSTDataLayer;

using System;
using System.Threading.Tasks;
using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool DoDbBackup { get; set; }
        private Logger HSTLogger { get; set; }
        public static bool DatabaseInitLoaded { get; private set; }
        public PeopleCollection<Person> _peopleCollection {get;set;}
        public HomesCollection _homesCollection { get; set; }
        public HomeSalesCollection _homeSalesCollection { get; set; }
        public RealEstateCosCollection _recosCollection { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //  portions of code are thanks to a Gist by Ronnie Overby at https://gist.github.com/ronnieoverby/7568387 
            HSTLogger = new Logger();
            if (HSTLogger.IsEnabled)
            {
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
                        DoDbBackup = true;
                        _peopleCollection = Factory.CollectionFactory.GetPeopleCollectionObject();
                        _homesCollection = Factory.CollectionFactory.GetHomesCollectionObject();
                        _homeSalesCollection = Factory.CollectionFactory.GetHomeSalesCollectionObject();
                        _recosCollection = Factory.CollectionFactory.GetRECosCollectionObject();
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
                    LogInnerExceptionMessages(ex, "WindowLoading InnerException");
                    HSTLogger.Flush();
                    _ = MessageBox.Show($"While launching, the application was unable to load the backup files. Application will now close.", "Unable to load file data.", MessageBoxButton.OK);
                    DoDbBackup = false;
                    App.Current.Shutdown();
                }
            }
        }

        private void LogInnerExceptionMessages(Exception e, string title)
        {
            if (e.InnerException != null)
            {
                LogInnerExceptionMessages(e.InnerException, title);
            }
            HSTLogger.Data(title, e.Message);
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
            //HSTLogger.Data(exception.ToString(), @event);
            LogInnerExceptionMessages(exception, @event);
            HSTLogger.Flush();
        }
    }
}
