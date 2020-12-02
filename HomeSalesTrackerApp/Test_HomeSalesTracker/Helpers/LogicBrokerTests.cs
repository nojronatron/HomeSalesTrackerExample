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
            var expectedPerson = new Person()
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "jsmith@j.com",
                Phone = "1112223333"
            };

            int personID = 1;
            Person actualPerson = LogicBroker.GetPerson(personID);

            bool areEqual = expectedPerson.Equals(actualPerson);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetPersonTest()
        {
            string firstName = "John";
            string lastName = "Smith";

            var expectedPerson = new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = "jsmith@j.com",
                Phone = "1112223333"
            };

            Person actualPerson = LogicBroker.GetPerson(firstName, lastName);

            bool areEqual = expectedPerson.Equals(actualPerson);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetHomeTest()
        {
            var expectedHome = new Home()
            {
                Address = "23 Oak St.",
                City = "Johnsonville",
                State = "CA",
                Zip = "955551111"
            };

            int homeID = 1;
            Home actualHome = LogicBroker.GetHome(homeID);

            bool areEqual = expectedHome.Equals(actualHome);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetHomeTest1()
        {
            var address = "23 Oak St.";
            var zip = "955551111";

            var expectedHome = new Home()
            {
                Address = address,
                City = "Johnsonville",
                State = "CA",
                Zip = zip
            };

            Home actualHome = LogicBroker.GetHome(address, zip);

            bool areEqual = expectedHome.Equals(actualHome);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetHomeSaleTest()
        {
            var expectedHomeSale = new HomeSale()
            {
                HomeID = 1,
                SoldDate = new DateTime(2010, 03, 15, 00, 00, 00),
                AgentID = 1,
                SaleAmount = 335000m,
                BuyerID = 1,
                MarketDate = new DateTime(2015, 03, 01, 00, 00, 00),
                CompanyID = 2
            };

            int homesaleID = 1;
            var actualHomeSale = LogicBroker.GetHomeSale(homesaleID);

            bool areEqual = expectedHomeSale.Equals(actualHomeSale);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetHomeSaleTest1()
        {
            var marketDate = new DateTime(2015, 03, 01, 00, 00, 00);
            var saleAmount = 335000m;

            var expectedHomeSale = new HomeSale()
            {
                SaleID = 1,
                HomeID = 1,
                SoldDate = new DateTime(2010, 03, 15, 00, 00, 00),
                AgentID = 1,
                SaleAmount = saleAmount,
                BuyerID = 1,
                MarketDate = marketDate,
                CompanyID = 2
            };

            var actualHomeSale = LogicBroker.GetHomeSale(marketDate, saleAmount);

            bool areEqual = expectedHomeSale.Equals(actualHomeSale);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetReCompanyTest()
        {
            int companyID = 1;
            var expectedRECo = new RealEstateCompany()
            {
                CompanyID = companyID,
                CompanyName = "ABC Real Estate"
            };

            var actualRECo = LogicBroker.GetReCompany(companyID);

            bool areEqual = expectedRECo.Equals(actualRECo);
            Assert.IsTrue(areEqual);
        }

        [TestMethod()]
        public void GetReCompanyTest1()
        {
            int companyID = 1;
            string companyName = "ABC Real Estate";

            var expectedRECo = new RealEstateCompany()
            {
                CompanyID = companyID,
                CompanyName = companyName
            };

            var actualRECo = LogicBroker.GetReCompany(companyName);

            bool areEqual = expectedRECo.Equals(actualRECo);
            Assert.IsTrue(areEqual);
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
