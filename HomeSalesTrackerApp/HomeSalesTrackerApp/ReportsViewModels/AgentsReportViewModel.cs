using HomeSalesTrackerApp.Report_Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace HomeSalesTrackerApp.ReportsViewModels
{
    public class AgentsReportViewModel
    {
        public ObservableCollection<AgentsReportModel> AgentsList { get; set; }

        public AgentsReportViewModel()
        {
            Load();
        }

        private void Load()
        {
            var agentsList = new ObservableCollection<AgentsReportModel>();

            //  TODO: write queries to collect the required data
            var agentRecords = (from hs in MainWindow.homeSalesCollection
                            where hs.AgentID >= 0
                            select hs.Agent).Distinct();

            var agentPeople = (from a in agentRecords
                               from p in MainWindow.peopleCollection
                               where p.PersonID == a.AgentID
                               select p);

            var homeSalesRecords = (from a in agentRecords
                                    from hs in MainWindow.homeSalesCollection
                                    where hs.AgentID == a.AgentID &&
                                    hs.SaleAmount > 0
                                    select hs);

            var recos = (from re in MainWindow.reCosCollection
                         where re.CompanyID >= 0
                         select re).Distinct();

            //  TODO: set the collected data's properties into the viewmodel and add instances to the collection
            AgentsReportModel agent = null;
            foreach (var item in agentRecords)
            {
                var recoName = recos.Where(x => x.CompanyID == item.CompanyID).FirstOrDefault();
                agent = new AgentsReportModel();
                agent.AgentID = item.AgentID;
                agent.Commission = item.CommissionPercent;
                agent.EMail = agentPeople.Where(p => p.PersonID == item.AgentID).FirstOrDefault().Email ?? string.Empty;
                agent.FirstName = agentPeople.Where(p => p.PersonID == item.AgentID).FirstOrDefault().FirstName;
                agent.LastName = agentPeople.Where(p => p.PersonID == item.AgentID).FirstOrDefault().LastName;
                agent.Phone = agentPeople.Where(p => p.PersonID == item.AgentID).FirstOrDefault().Phone;
                if (recoName != null)
                {
                    agent.RealEstateCompany = recos.Where(x => x.CompanyID == item.CompanyID).FirstOrDefault().CompanyName ?? "Agent no longer active";
                }
                agent.TotalCommissionsPaid = homeSalesRecords.Where(hs => hs.AgentID == item.AgentID).Sum(hs => hs.SaleAmount) * item.CommissionPercent;
                agent.TotalHomesSold = item.HomeSales.Count;
                agent.TotalSales = item.HomeSales.Sum(hs => hs.SaleAmount);

                agentsList.Add(agent);
            }

            AgentsList = agentsList;
        }
    }
}
