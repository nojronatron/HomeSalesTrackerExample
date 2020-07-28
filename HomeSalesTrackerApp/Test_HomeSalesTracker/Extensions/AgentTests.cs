using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class AgentTests
    {
        private Agent unahOno = new Agent()
        {
            //PersonID = 1,
            //FirstName = "Unah",
            //LastName = "Ono",
            //Phone = "425-555-1212",
            //Email = "Unah.Ono@amail.com",
            AgentID = 1,
            CompanyID = 2,
            CommissionPercent = 0.4m
        };

        private Agent debbieDublee = new Agent()
        {
            AgentID = 2,
            CompanyID = null,
            CommissionPercent = 0.5m
        };

        private Agent treyTriplett = new Agent()
        {
            AgentID = 3,
            CompanyID = 1,
            CommissionPercent = 0.6m
        };

        [TestMethod()]
        public void ToStringTest()
        {
            var agentList = new List<Agent>();
            agentList.Add(unahOno);
            agentList.Add(debbieDublee);
            agentList.Add(treyTriplett);

            StringBuilder actualResult = new StringBuilder();
            foreach (var agentPerson in agentList)
            {
                actualResult.AppendLine(agentPerson.ToString());
            }
            Console.WriteLine(actualResult);
            int actualCount = actualResult.Length;
            int expectedCount = 192;    //  all chars counted
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void EqualityTest()
        {
            Agent secondAgent = new Agent()
            {
                //PersonID = 1,
                //FirstName = "Unah",
                //LastName = "Ono",
                //Phone = "425-555-1212",
                //Email = "Unah.Ono@amail.com",
                AgentID = 1,
                CompanyID = 2,
                CommissionPercent = 0.4m
            };
            Console.WriteLine($"p1: { unahOno.ToString() }\nsecondPerson: { secondAgent.ToString() }");

            bool expectedResult = true;
            bool actualResult = unahOno.Equals(secondAgent);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityTest()
        {
            bool expectedResult = false;
            bool actualResult = unahOno.Equals(debbieDublee);

            Assert.AreEqual(expectedResult, actualResult);

        }

        [TestMethod()]
        public void EqualityLeftRightTest()
        {
            Agent secondAgent = new Agent()
            {
                //FirstName = "Unah",
                //LastName = "Ono",
                //Phone = "425-555-1212",
                //Email = "Unah.Ono@amail.com",
                AgentID = 1,
                CompanyID = 2,
                CommissionPercent = 0.4m
            };
            Console.WriteLine($"p1: { unahOno.ToString() }\nsecondPerson: { secondAgent.ToString() }");

            bool expectedResult = true;
            bool actualResult = (unahOno == secondAgent);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityLeftRightTest()
        {
            bool expectedResult = true;
            bool actualResult = (unahOno != debbieDublee);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}