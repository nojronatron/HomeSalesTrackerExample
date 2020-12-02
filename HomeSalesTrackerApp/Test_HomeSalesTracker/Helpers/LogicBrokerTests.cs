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
    public class LogicBrokerTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            if (LogicBroker.InitDatabase())
            {
                Console.WriteLine("InitDatabase method returned \"True\".");
                if (LogicBroker.LoadData())
                {
                    Console.WriteLine("LoadData method returned \"True\".");
                }
                else
                {
                    Console.WriteLine("LoadData method returned \"False\".");
                }

            }
            else
            {
                Console.WriteLine("InitDatabase method returned \"False\".");
            }

        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (LogicBroker.BackUpDatabase())
            {
                Console.WriteLine("BackUpDatabase method returned \"True\".\nCheck the XML files to verify.");
            }
            else
            {
                Console.WriteLine("BackUpDatabase method returned \"False\".");
            }

        }

        [TestMethod()]
        public void GetPersonIntTest()
        {
            int personID = 0;
            LogicBroker.GetPerson(personID);
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPersonTest()
        {
            string firstName = "";
            string lastName = "";
            LogicBroker.GetPerson(firstName, lastName);
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeSaleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeSaleTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetReCompanyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetReCompanyTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAgentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetBuyerTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetOwnerTest()
        {
            Assert.Fail();
        }


        [TestMethod()]
        public void StoreItemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateExistingItemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveEntityTest()
        {
            Assert.Fail();
        }

    }

}
