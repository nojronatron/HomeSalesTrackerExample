using HomeSalesTrackerApp.ReportsViewModels;

using System;
using System.ComponentModel;
using System.Windows;

namespace HomeSalesTrackerApp
{
    /// <summary>
    /// Interaction logic for AgentsResultsReport.xaml
    /// </summary>
    public partial class AgentsResultsReport : Window, IDisposable
    {
        //  Other managed resource this class uses
        private Component component = new Component();

        public AgentsResultsReport()
        {
            InitializeComponent();
            var agentsReportViewModel = new AgentsReportViewModel();
            DataContext = agentsReportViewModel;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    component.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
