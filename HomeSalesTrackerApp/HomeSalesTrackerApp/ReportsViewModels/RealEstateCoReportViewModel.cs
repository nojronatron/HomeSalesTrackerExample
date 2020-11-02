using HomeSalesTrackerApp.Report_Models;

using System.Collections.Generic;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class RealEstateCoReportViewModel
    {
        public List<RealEstateCoReportModel> RealEstateCoTotals { get; set; }

        public RealEstateCoReportViewModel()
        {
            LoadRealEstateCoTotals();
        }
        public void LoadRealEstateCoTotals()
        {

            var realEstateCoTotals = (from hs in MainWindow.homeSalesCollection
                                      join re in MainWindow.reCosCollection on hs.CompanyID equals re.CompanyID
                                      select new RealEstateCoReportModel
                                      {
                                          CompanyID = re.CompanyID,
                                          RECoName = re.CompanyName,
                                          TotalAmountForSale = re.HomeSales.Where(hfs => hfs.SoldDate == null).Sum(hfs => hfs.SaleAmount),
                                          TotalHomesCurrentlyForSale = re.HomeSales.Where(hfs => hfs.SoldDate == null).Count(),
                                          TotalSales = re.HomeSales.Where(hfs => hfs.SoldDate != null).Sum(x => x.SaleAmount),
                                          TotalNumberOfHomesSold = re.HomeSales.Where(sh => sh.SoldDate != null).Count()
                                      });

            RealEstateCoTotals = realEstateCoTotals.OrderBy(re => re.RECoName).Distinct().ToList();
        }

    }
}
