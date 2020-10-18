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

            //var vRealEstateCompanies = (from re in MainWindow.reCosCollection
            //                            select re).ToList();

            //foreach (var item in vRealEstateCompanies)
            //{
            //    var vRECoSalesAmounts = (from hfs in MainWindow.homeSalesCollection
            //                             where item.CompanyID == hfs.CompanyID &&
            //                             hfs.BuyerID != null
            //                             select hfs.SaleAmount).ToList();

            //    var vRECoHomesForSale = (from hfs in MainWindow.homeSalesCollection
            //                             where hfs.CompanyID == item.CompanyID &&
            //                             hfs.BuyerID == null
            //                             select hfs.SaleAmount).ToList();

            //    var record = new RealEstateCoReportModel()
            //    {
            //        CompanyID = item.CompanyID,
            //        RECoName = item.CompanyName,
            //        TotalAmountForSale = vRECoHomesForSale.Sum(),
            //        TotalHomesForSale = vRECoHomesForSale.Count(),
            //        TotalSales = vRECoSalesAmounts.Sum(),
            //        TotalSoldHomesCount = vRECoSalesAmounts.Count
            //    };

            //    realEstateCoTotals.Add(record);
            //}

            RealEstateCoTotals = realEstateCoTotals.OrderBy(re => re.RECoName).Distinct().ToList();
        }
    }
}
