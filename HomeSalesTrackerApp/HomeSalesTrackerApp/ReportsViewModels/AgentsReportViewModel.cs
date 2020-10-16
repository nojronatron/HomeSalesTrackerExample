using HomeSalesTrackerApp.Report_Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class AgentsReportViewModel
    {
        public List<AgentsReportModel> AgentsList { get; set; }

        public AgentsReportViewModel()
        {
            Load();
        }

        private void Load()
        {
            var agentsList = new List<AgentsReportModel>();

            var agentRecords = (from hs in MainWindow.homeSalesCollection
                            where hs.AgentID >= 0
                            select hs.Agent).Distinct();

            var homeSalesRecords = (from a in agentRecords
                                    from hs in MainWindow.homeSalesCollection
                                    where hs.SaleAmount > 0
                                    select hs);

            agentsList = (from agentRecord in agentRecords
                         join agentPerson in (from a in agentRecords from p in MainWindow.peopleCollection where p.PersonID == a.AgentID select p)
                         on agentRecord.AgentID equals agentPerson.PersonID
                         select new AgentsReportModel
                         {
                             AgentID = agentRecord.AgentID,
                             Commission = agentRecord.CommissionPercent,
                             EMail = agentPerson.Email ?? string.Empty,
                             FirstName = agentPerson.FirstName,
                             LastName = agentPerson.LastName,
                             Phone = agentPerson.Phone,
                             HomesOnMarket = MainWindow.homeSalesCollection.Where(hs => hs.Buyer == null && hs.AgentID == agentRecord.AgentID).Count(),
                             RealEstateCompany = agentRecord.CompanyID != null ?
                                MainWindow.reCosCollection.Where(re => re.CompanyID == agentRecord.CompanyID).FirstOrDefault().CompanyName :
                                "Agent no longer active",
                             TotalCommissionsPaid = homeSalesRecords.Where(a => a.AgentID == agentRecord.AgentID).Sum(hs => hs.SaleAmount) * agentRecord.CommissionPercent,
                             TotalHomesSold = homeSalesRecords.Where(a => a.AgentID == agentRecord.AgentID).Count(),
                             TotalSales = homeSalesRecords.Where(a => a.AgentID == agentRecord.AgentID).Sum(hs => hs.SaleAmount)
                         }).ToList();

            AgentsList = agentsList;
        }
    }
}
