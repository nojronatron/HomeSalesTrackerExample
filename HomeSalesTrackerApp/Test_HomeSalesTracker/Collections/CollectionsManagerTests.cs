using Microsoft.VisualStudio.TestTools.UnitTesting;
using HomeSalesTrackerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSTDataLayer;

namespace HomeSalesTrackerApp.Helpers.Tests
{
    [TestClass()]
    public class CollectionsManagerTests
    {
        [TestMethod()]
        public void CollectionsQueryTests()
        {
            HSTContextInitializer.InitDB();
            HSTContextInitializer.LoadDataIntoDatabase();

            CollectionsManager cm = new CollectionsManager();
            
            if (cm.InitializeCollections())
            {
                var agents = GetAllAgents();
                var expectedResult = 5;
                var actualResult = agents.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var agentPeople = GetAllAgentPeople();
                expectedResult = 5;
                actualResult = agentPeople.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var agentPeopleWithHomeSales = GetAllAgentPeopleWithAtLeastOneHomesale();
                expectedResult = 5;
                actualResult = agentPeopleWithHomeSales.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var agentPeopleNoHomesales = GetAllAgentPeopleWithNoHomeSaleCount();
                expectedResult = 1;
                actualResult = agentPeopleNoHomesales.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var agentsWithRECos = GetAllAgentsWithRECos();
                expectedResult = 4;
                actualResult = agentsWithRECos.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var agentsNoRECos = GetAllAgentsNoRECos();
                expectedResult = 1;
                actualResult = agentsNoRECos.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var buyers = GetAllBuyers();
                expectedResult = 4;
                actualResult = buyers.Count;
                Assert.AreEqual(expectedResult, actualResult);
                
                var buyerPeople = GetAllBuyerPeople();
                expectedResult = 4;
                actualResult = buyerPeople.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var owners = GetAllOwners();
                expectedResult = 4;
                actualResult = buyerPeople.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var ownerPeople = GetAllOwnerPeople();
                expectedResult = 4;
                actualResult = ownerPeople.Count;
                Assert.AreEqual(expectedResult, actualResult);

                var people = GetAllPeople();
                expectedResult = 10;
                actualResult = people.Count;
                Assert.AreEqual(expectedResult, actualResult);

            }
            else
            {
                Assert.IsTrue(false, "cm.InitializeCollections() failed");
            }
        }

        public List<Person> GetAllPeople()
        {
            var peopleList = (from p in CollectionsManager.peopleCollection
                              select p).ToList();
            return peopleList;
        }

        public List<Agent> GetAllAgents()
        {
            var agentList = (from hs in CollectionsManager.homeSalesCollection
                             where hs.Agent != null
                             select hs.Agent).ToList();
            agentList.Distinct();
            return agentList;
        }

        public List<Person> GetAllAgentPeople()
        {
            var agentPersonList = (from hs in CollectionsManager.homeSalesCollection
                                   from p in CollectionsManager.peopleCollection
                                   where hs.AgentID == p.PersonID
                                   select p).ToList();
            agentPersonList.Distinct();
            return agentPersonList;
        }

        public List<Person> GetAllAgentPeopleWithAtLeastOneHomesale()
        {
            var agentPeopleWithSalesList = (from hs in CollectionsManager.homeSalesCollection
                                            from p in CollectionsManager.peopleCollection
                                            where hs.AgentID == p.PersonID &&
                                            hs.Agent.HomeSales.Count > 0
                                            select p).ToList();
            return agentPeopleWithSalesList;
        }

        public List<Person> GetAllAgentPeopleWithNoHomeSaleCount()
        {
            var allAgentPeopleWithNoHomeSales = (from hs in CollectionsManager.homeSalesCollection
                                                 from p in CollectionsManager.peopleCollection
                                                 where hs.AgentID == p.PersonID &&
                                                 hs.Agent.HomeSales.Count < 1
                                                 select p).ToList();
            return allAgentPeopleWithNoHomeSales;
        }

        public List<Person> GetAllOwnerPeople()
        {
            var ownerPeopleList = (from p in CollectionsManager.peopleCollection
                                   from h in CollectionsManager.homesCollection
                                   where p.PersonID == h.OwnerID
                                   select p).ToList();
            ownerPeopleList.Distinct();
            return ownerPeopleList;
        }

        public List<Agent> GetAllAgentsWithRECos()
        {
            var agentsWithRECosList = (from hs in CollectionsManager.homeSalesCollection
                                       from re in CollectionsManager.reCosCollection
                                       where hs.Agent.CompanyID == re.CompanyID
                                       select hs.Agent).ToList();
            return agentsWithRECosList;
        }

        public List<Agent> GetAllAgentsNoRECos()
        {
            var agentsNoRECosList = (from hs in CollectionsManager.homeSalesCollection
                                     where hs.Agent.CompanyID == null
                                     select hs.Agent).ToList();
            return agentsNoRECosList;
        }

        public List<Person> GetAllBuyerPeople()
        {
            var buyerPeople = (from p in CollectionsManager.peopleCollection
                               from hs in CollectionsManager.homeSalesCollection
                               where p.PersonID == hs.BuyerID
                               select p).ToList();
            buyerPeople.Distinct();
            return buyerPeople;
        }

        public List<Buyer> GetAllBuyers()
        {
            var buyersList = (from b in CollectionsManager.peopleCollection
                              from hs in CollectionsManager.homeSalesCollection
                              where b.PersonID == hs.BuyerID
                              select hs.Buyer).ToList();
            return buyersList;
        }

        public List<Owner> GetAllOwners()
        {
            var ownerList = (from h in CollectionsManager.homesCollection
                             where h.Owner != null
                             select h.Owner).ToList();
            return ownerList;
        }

    }
}