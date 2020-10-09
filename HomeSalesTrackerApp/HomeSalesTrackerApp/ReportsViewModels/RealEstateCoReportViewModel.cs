using HomeSalesTrackerApp.Report_Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class RealEstateCoReportViewModel
    {
        public ObservableCollection<RealEstateCoReportModel> RealEstateCoTotals  { get; set;}

        public RealEstateCoReportViewModel()
        {
            LoadRealEstateCoTotals();
        }
        public void LoadRealEstateCoTotals()
        {
            ObservableCollection<RealEstateCoReportModel> realEstateCoTotals = new ObservableCollection<RealEstateCoReportModel>();

            var vRealEstateCompanies = (from re in MainWindow.reCosCollection
                                        select re).ToList();

            foreach (var item in vRealEstateCompanies)
            {
                var vRECoSalesAmounts = (from hfs in MainWindow.homeSalesCollection
                                         where item.CompanyID == hfs.CompanyID &&
                                         hfs.BuyerID != null
                                         select hfs.SaleAmount).ToList();

                var vRECoHomesForSale = (from hfs in MainWindow.homeSalesCollection
                                         where hfs.CompanyID == item.CompanyID &&
                                         hfs.BuyerID == null
                                         select hfs.SaleAmount).ToList();

                var record = new RealEstateCoReportModel()
                {
                    CompanyID = item.CompanyID,
                    RECoName = item.CompanyName,
                    TotalAmountForSale = vRECoHomesForSale.Sum(),
                    TotalHomesForSale = vRECoHomesForSale.Count(),
                    TotalSales = vRECoSalesAmounts.Sum(),
                    TotalSoldHomesCount = vRECoSalesAmounts.Count
                };

                realEstateCoTotals.Add(record);
            }

            RealEstateCoTotals = realEstateCoTotals;
        }
    }
}
