using HomeSalesTrackerApp.Report_Models;

using System.Collections.Generic;
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

            var query = (from person in MainWindow.peopleCollection
                         join agent in MainWindow.homeSalesCollection on person.PersonID equals agent.AgentID
                         join homesale in MainWindow.homeSalesCollection on agent.AgentID equals homesale.AgentID
                         where homesale.Agent != null
                         join reco in MainWindow.reCosCollection on homesale.CompanyID equals reco.CompanyID
                         select new AgentsReportModel
                         {
                             AgentID = homesale.AgentID,
                             Commission = homesale.Agent.CommissionPercent,
                             EMail = person.Email,
                             FirstName = person.FirstName,
                             HomesOnMarket = homesale.Agent.HomeSales.Where(x => x.Buyer == null).Count(),
                             LastName = person.LastName,
                             Phone = person.Phone,
                             RealEstateCompany = reco.CompanyName ?? "Agent no longer active",
                             TotalCommissionsPaid = homesale.Agent.CommissionPercent * homesale.Agent.HomeSales.Where(hs => hs.Buyer != null).Sum(hs => hs.SaleAmount),
                             TotalHomesSold = homesale.Agent.HomeSales.Where(x => x.Buyer != null).Count(),
                             TotalSales = homesale.Agent.HomeSales.Where(hs => hs.AgentID == person.PersonID && hs.Buyer != null).Sum(hs => hs.SaleAmount)
                         }).Distinct();

            agentsList = query.OrderBy(re => re.RealEstateCompany).ThenBy(ln => ln.LastName).ThenBy(fn => fn.FirstName).ToList();

            AgentsList = agentsList;
        }
    }
}
