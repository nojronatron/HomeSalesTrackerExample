using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer;
using System;
using System.Collections.Generic;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class LogicBrokerTests
    {
        public static void PrintObject<T>(List<T> inputObjects)
        {
            foreach (var item in inputObjects)
            {
                Console.WriteLine(item.ToString());
            }
        }

        [TestMethod()]
        public void InitDatabaseTest()
        {
            var result = false;

            if (LogicBroker.InitDatabase())
            {
                Console.WriteLine("Database initialization completed successfully");
                result = true;
            }
            else
            {
                Console.WriteLine("Database initialization failed.");
            }

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void LoadDatabaseDataTest()
        {
            var result = false;

            if (LogicBroker.LoadData())
            {
                Console.WriteLine("Database data loaded successfully.");
                result = true;
            }
            else
            {
                Console.WriteLine("Database data load failed.");
            }

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void OneBigTest()
        {
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

                int homeID = 1;
                Home actualHome = LogicBroker.GetHome(homeID);

                bool areEqual = expectedHome.Equals(actualHome);
                Assert.IsTrue(areEqual);
            }

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

            {
                int homesaleID = 6;

                var expectedHomeSale = new HomeSale()
                {
                    SaleID = homesaleID,
                    HomeID = 3,
                    SoldDate = new DateTime(2014, 06, 13),
                    AgentID = 1,
                    SaleAmount = 550_000m,
                    BuyerID = 5,
                    MarketDate = new DateTime(2014, 06, 01),
                    CompanyID = 4
                };

                var actualHomeSale = LogicBroker.GetHomeSale(homesaleID);

                bool areEqual = expectedHomeSale.Equals(actualHomeSale);
                if (!areEqual)
                {
                    var items = new List<HomeSale>()
                    {
                        expectedHomeSale, actualHomeSale
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                int homesaleID = 6;
                var marketDate = new DateTime(2015, 03, 01);
                var saleAmount = 335_000m;

                var expectedHomeSale = new HomeSale()
                {
                    SaleID = homesaleID,
                    HomeID = 1,
                    SoldDate = new DateTime(2010, 03, 15),
                    AgentID = 1,
                    SaleAmount = saleAmount,
                    BuyerID = 1,
                    MarketDate = marketDate,
                    CompanyID = 2
                };

                var actualHomeSale = LogicBroker.GetHomeSale(marketDate, saleAmount);

                bool areEqual = expectedHomeSale.Equals(actualHomeSale);
                if (!areEqual)
                {
                    var items = new List<HomeSale>()
                    {
                        expectedHomeSale, actualHomeSale
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                var saleID = 1;

                var expectedHomeForSale = new HomeSale()
                {
                    SaleID = saleID,
                    HomeID = 3,
                    SoldDate = null,
                    AgentID = 4,
                    SaleAmount = 700_000m,
                    MarketDate = new DateTime(2016, 08, 15),
                    CompanyID = 1
                };

                var actualHomeForSale = LogicBroker.GetHomeSale(saleID);

                bool areEqual = expectedHomeForSale.Equals(actualHomeForSale);
                if (!areEqual)
                {
                    var items = new List<HomeSale>()
                    {
                        expectedHomeForSale, actualHomeForSale
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                int companyID = 3;

                var expectedRECo = new RealEstateCompany()
                {
                    CompanyID = companyID,
                    CompanyName = "Rapid Real Estate",
                    Phone = "6662221111"
                };

                var actualRECo = LogicBroker.GetReCompany(companyID);

                bool areEqual = expectedRECo.Equals(actualRECo);
                if (!areEqual)
                {
                    var items = new List<RealEstateCompany>()
                    {
                        expectedRECo, actualRECo
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                int companyID = 3;
                var companyName = "Rapid Real Estate";

                var expectedRECo = new RealEstateCompany()
                {
                    CompanyID = companyID,
                    CompanyName = companyName,
                    Phone = "6662221111"
                };

                var actualRECo = LogicBroker.GetReCompany(companyName);

                bool areEqual = expectedRECo.Equals(actualRECo);
                if (!areEqual)
                {
                    var items = new List<RealEstateCompany>()
                    {
                        expectedRECo, actualRECo
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                var agentID = 4;

                var expectedAgent = new Agent()
                {
                    AgentID = agentID,
                    CompanyID = 1,
                    CommissionPercent = 0.03m
                };

                var actualAgent = LogicBroker.GetAgent(agentID);

                bool areEqual = expectedAgent.Equals(actualAgent);
                if (!areEqual)
                {
                    var items = new List<Agent>()
                    {
                        expectedAgent, actualAgent
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                var buyerID = 7;

                var expectedBuyer = new Buyer()
                {
                    BuyerID = buyerID,
                    CreditRating = 780
                };

                var actualBuyer = LogicBroker.GetBuyer(buyerID);

                bool areEqual = expectedBuyer.Equals(actualBuyer);
                if (!areEqual)
                {
                    var items = new List<Buyer>()
                    {
                        expectedBuyer, actualBuyer
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                var ownerID = 7;

                var expectedOwner = new Owner()
                {
                    OwnerID = ownerID,
                    PreferredLender = "Unique Mortgaging"
                };

                var actualOwner = LogicBroker.GetOwner(ownerID);

                bool areEqual = expectedOwner.Equals(actualOwner);
                if (!areEqual)
                {
                    var items = new List<Owner>()
                    {
                        expectedOwner, actualOwner
                    };

                    PrintObject(items);
                }

                Assert.IsTrue(areEqual);
            }

            {
                var addUpdateRemovePerson = new Person
                {
                    FirstName = "p1FirstName",
                    LastName = "p2LastName",
                    Phone = "123456789",
                    Email = "Person1@StoreItemTest.net"
                };

                var expectedStoreResult = true;
                var actualStoreResult = LogicBroker.StoreItem<Person>(addUpdateRemovePerson);

                if (!actualStoreResult)
                {
                    Console.WriteLine(addUpdateRemovePerson.ToString());
                }
                Assert.AreEqual(expectedStoreResult, actualStoreResult);

                var expectedUpdateResult = true;
                var actualUpdateResult = LogicBroker.UpdateExistingItem<Person>(new Person
                {
                    FirstName = addUpdateRemovePerson.FirstName,
                    LastName = addUpdateRemovePerson.LastName,
                    Phone = "0000000000",
                    Email = "bogus.email@UpdateExistingItemTest.net"
                });

                if (actualUpdateResult)
                {
                    Person resultPerson = LogicBroker.GetPerson(addUpdateRemovePerson.FirstName, addUpdateRemovePerson.LastName);
                    Console.WriteLine(resultPerson.ToString());
                }
                Assert.AreNotEqual(expectedUpdateResult, actualUpdateResult);

                var expectedRemoveResult = true;
                var actualRemoveResult = LogicBroker.RemoveEntity<Person>(addUpdateRemovePerson);
                if (!actualRemoveResult)
                {
                    Console.WriteLine("RemoveEntity<Person>(addUpdateRemovePerson) failed.");
                    Console.WriteLine(addUpdateRemovePerson.ToString());
                }
                Assert.AreEqual(expectedRemoveResult, actualRemoveResult);
            }

        }

    }

}
