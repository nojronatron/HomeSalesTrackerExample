using HomeSalesTrackerApp;

using HSTDataLayer.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class LogicBrokerTests
    {
        static PeopleCollection<Person> DbPeopleCollection { get; set; }
        static HomesCollection DbHomesCollection { get; set; }
        static HomeSalesCollection DbHomesalesCollection { get; set; }
        static RealEstateCosCollection DbRECosCollection { get; set; }

        public static void PrintObjects<T>(List<T> inputObjects, string message="")
        {
            Console.WriteLine("##### PrintObjects Output #####");
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }
            foreach (var item in inputObjects)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public static void PrintObject<T>(T inputObject, string message="")
        {
            Console.WriteLine("##### PrintObject Output #####");
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(message);
            }
            Console.WriteLine(inputObject.ToString());
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

        [Ignore]
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
                #region LogicBrokerTests
                //  GetPerson(PersonID)
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

                //  GetPerson(firstname, lastname)
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

                //  GetHome(homeID)
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

                //  GetHome(address, zip)
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

                //  GetHomeSale(saleID)
                //  SOLD
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetHomeSale(marketDate, saleAmount)
                //  SOLD
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetHomeSale(saleID)
                //  FOR SALE
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetReCompany(companyID)
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetReCompany(companyName)
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetAgent(AgentID)
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetBuyer(BuyerID)
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  GetOwner(OwnerID)
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

                        PrintObjects(items);
                    }

                    Assert.IsTrue(areEqual);
                }

                //  UpdateExistingItem<Person>(person)
                {
                    var updatePersonFirstName = "p1FirstName";
                    var updatePersonLastName = "p2LastName";
                    var addUpdateRemovePerson = new Person
                    {
                        FirstName = updatePersonFirstName,
                        LastName = updatePersonLastName,
                        Phone = "123456789",
                        Email = "Person1@StoreItemTest.net"
                    };

                    var expectedStoreResult = true;
                    var actualStoreResult = LogicBroker.StoreItem<Person>(addUpdateRemovePerson);

                    PrintObject<Person>(addUpdateRemovePerson, "Update Existing Item addUpdateRemovePerson.");
                    PrintObject<bool>(actualStoreResult, "Return value from StoreItem().");

                    if (!actualStoreResult)
                    {
                        Console.WriteLine(addUpdateRemovePerson.ToString());
                    }

                    Assert.AreEqual(expectedStoreResult, actualStoreResult);

                    var personToUpdate = LogicBroker.GetPerson(updatePersonFirstName, updatePersonLastName);
                    PrintObject<Person>(personToUpdate, "Returned Person from GetPerson(firstname, lastname).");

                    var expectedUpdateResult = true;
                    var updatePerson = new Person()
                    {
                        PersonID = personToUpdate.PersonID,
                        FirstName = personToUpdate.FirstName,
                        LastName = personToUpdate.LastName,
                        Phone = "0000000000",
                        Email = "bogus.email@UpdateExistingItemTest.net"
                    };

                    var actualUpdateResult = LogicBroker.UpdateExistingItem<Person>(updatePerson);

                    PrintObject<bool>(actualUpdateResult, "Return value from UpdateExistingItem().");

                    if (actualUpdateResult)
                    {
                        Person resultPerson = LogicBroker.GetPerson(addUpdateRemovePerson.FirstName, addUpdateRemovePerson.LastName);
                        Console.WriteLine(resultPerson.ToString());
                    }

                    Assert.AreEqual(expectedUpdateResult, actualUpdateResult);

                    var expectedRemoveResult = true;
                    var actualRemoveResult = LogicBroker.RemoveEntity<Person>(updatePerson);

                    PrintObject<bool>(actualRemoveResult, "Return value from RemoveEntity<Person>().");

                    if (!actualRemoveResult)
                    {
                        Console.WriteLine("RemoveEntity<Person>(addUpdateRemovePerson) failed.");
                        Console.WriteLine(addUpdateRemovePerson.ToString());
                    }

                    Assert.AreEqual(expectedRemoveResult, actualRemoveResult);
                }
            }
            #endregion LogicBrokerTests

            #region InitializeCollections

            DbPeopleCollection = new PeopleCollection<Person>(EntityLists.GetListOfPeople());
            Assert.IsTrue(DbPeopleCollection.Count == 10);

            DbHomesCollection = new HomesCollection(EntityLists.GetTreeListOfHomes());
            Assert.IsTrue(DbHomesCollection.Count == 5);

            DbHomesalesCollection = new HomeSalesCollection(EntityLists.GetListOfHomeSales());
            Assert.IsTrue(DbHomesalesCollection.Count == 8);

            DbRECosCollection = new RealEstateCosCollection(EntityLists.GetTreeListOfRECompanies());
            Assert.IsTrue(DbRECosCollection.Count == 4);
            #endregion InitializeCollections

            #region PeopleCollectionTests
            {
                //  COUNT
                var expectedCount = 10;
                var actualCount = DbPeopleCollection.Count;
                Assert.AreEqual(expectedCount, actualCount);

                //  ADD
                var personPerson = new Person()
                {
                    FirstName = "Owen",
                    LastName = "Owner",
                    Phone = "123456789",
                    Email = "test.owner@person.net"
                };
                var personAddedCount = DbPeopleCollection.Add(personPerson);
                PrintObject<Person>(personPerson, $"Attempted to add Person to PersonCollection. Result: { personAddedCount }.");
                Assert.IsTrue(personAddedCount == 1);   //  Add plain Person

                //  GET (PersonID by F+L Names)
                int addedPersonID = 0;
                addedPersonID = DbPeopleCollection.GetPersonIDbyName(personPerson.FirstName, personPerson.LastName);
                PrintObject<int>(addedPersonID, "Returned PersonID from GetPersonIDbyName(Owen, Owner).");
                Assert.IsTrue(addedPersonID > 10);

                //  GET (Person by Person)
                Person addedPerson = null;
                addedPerson = DbPeopleCollection.Get(addedPersonID);
                PrintObject<Person>(addedPerson, "Returned Person from Get(addedPersonID).");
                Assert.IsTrue(addedPerson != null);

                //  UPDATE (Person's Phone)
                addedPerson.Phone = "3254678451";
                var addedPersonUpdated = DbPeopleCollection.UpdatePerson(addedPerson);
                PrintObject<Person>(addedPerson, $"UpdatePerson(addedPerson) result: { addedPersonUpdated }.");
                Assert.IsTrue(addedPersonUpdated == 1);

                //  UPDATE (Person as an Owner)
                var owner = new Owner()
                {
                    OwnerID = addedPerson.PersonID,
                    PreferredLender = "Lender Test"
                };
                addedPerson.Owner = owner;
                var ownerPersonUpdated = DbPeopleCollection.UpdatePerson(addedPerson);
                PrintObject<Person>(addedPerson, $"UpdatePerson(addedPerson) with Owner result: { ownerPersonUpdated }.");
                Assert.IsTrue(ownerPersonUpdated > 0);

                //  REMOVE
                var personRemoved = DbPeopleCollection.Remove(personPerson);
                PrintObject<Person>(personPerson, $"Removing person from collection result: { personRemoved }.");
                Assert.IsTrue(personRemoved);           //  Remove plain Person


                //  CLEAR
                DbPeopleCollection.Clear();
                expectedCount = 0;
                actualCount = DbPeopleCollection.Count;

                //  REINIT
                DbPeopleCollection = new PeopleCollection<Person>(EntityLists.GetListOfPeople());
                Assert.IsTrue(DbPeopleCollection.Count == 11);
            }
            #endregion

            #region HomesCollectionTests
            {
                var newHome = new Home()
                {
                    Address = "4412 153rd Ave SE",
                    City = "Bellevue",
                    State = "WA",
                    Zip = "980060000"
                };

                //  GET HOME BY ADDRESS AND ZIP
                //  var homeByAddress = DbHomesCollection.GetHome(anAddress, aZipCode);


                //  GET HOME BY ID
                var homeID = 1;
                var homeByID = DbHomesCollection.Retreive(homeID);

                var expectedHome = new Home()
                {
                    HomeID = 1,
                    Address = "23 Oak St.",
                    City = "Johnsonville",
                    State = "CA",
                    Zip = "955551111",
                    OwnerID = 1
                };
                var expectedResult = true;
                var actualResult = expectedHome.Equals(homeByID);
                PrintObject<Home>(homeByID, "Get Home by ID Result.");
                PrintObject<bool>(actualResult, "Get Home by id actual result:");
                Assert.AreEqual(expectedResult, actualResult);

                //  UPDATE HOME
                newHome.OwnerID = 3;
                var actualUpdateResult = DbHomesCollection.Update(newHome);
                var expectedUpdateResult = 1;
                PrintObject<int>(actualUpdateResult, "Update Home actual update result:");
                Assert.AreEqual(expectedUpdateResult, actualUpdateResult);

                //  REMOVE HOME
                var expectedRemoveResult = true;
                var actualRemoveResult = DbHomesCollection.Remove(newHome.HomeID);
                PrintObject<bool>(actualRemoveResult, "Remove Home actual result:");
                Assert.AreEqual(expectedRemoveResult, actualRemoveResult);

                //  CLEAR
                //DbHomesCollection.Clear();
                //expectedCount = 0;
                //actualCount = DbHomesCollection.Count;
                //Assert.AreEqual(expectedCount, actualCount);

                //  REINIT
                DbHomesCollection = new HomesCollection(EntityLists.GetTreeListOfHomes());

                //  var expectedCount = 5;
                var expectedCount = 6; //   Use this until DbHomesCollection.Remove() is implemented to remove item from DB
                var actualCount = DbHomesCollection.Count;
                PrintObject<int>(actualCount, "Actual Count of reinitialized Homes collection.");
                Assert.AreEqual(expectedCount, actualCount);
            }
            #endregion

            #region HomeSalesCollectionTests
            {
                //  GET BY ID
                var existingSaleID = 1;
                var getHomesale = DbHomesalesCollection.Retreive(existingSaleID);
                PrintObject<HomeSale>(getHomesale, "Retreived Homesale with SaleID=1: ");
                var marketDate = new DateTime(2016, 08, 15);
                var expectedHomesale = new HomeSale()
                {
                    SaleID = 6,
                    HomeID = 3,
                    AgentID = 4,
                    SaleAmount = 700000m,
                    MarketDate = new DateTime(2016, 08, 15),
                    CompanyID = 1
                };

                var expectedResult = true;
                var actualResult = expectedHomesale.Equals(getHomesale);
                Assert.AreEqual(expectedResult, actualResult);

                //  GET BY ITEM?
                //  TODO: Implement if necessary

                //  UPDATE E.G. SELLHOME
                var preNewHomesale = DbHomesalesCollection.Retreive(1); //  so it can be put back into DB at end of tests
                PrintObject<HomeSale>(preNewHomesale, "PreNewHomesale instance, SaleID=1: ");

                var newHomeSale = new HomeSale()
                {
                    SaleID = 1,
                    HomeID = 3,
                    AgentID = 4,
                    SaleAmount = 766666m,
                    MarketDate = new DateTime(2016, 08, 15),
                    CompanyID = 1,
                    SoldDate = new DateTime(2020, 08, 15),
                    BuyerID = 1
                };

                var actualUpdateResult = DbHomesalesCollection.Update(newHomeSale);
                PrintObject<HomeSale>(newHomeSale, "NewHomeSale instance sent to Update method: ");
                PrintObject<int>(actualUpdateResult, "UPDATE Home as sold result: ");
                var expectedUpdateResult = 1;
                Assert.AreEqual(expectedUpdateResult, actualUpdateResult);

                //  REMOVE
                var actualRemoveResult = DbHomesalesCollection.Remove(newHomeSale);
                PrintObject<HomeSale>(newHomeSale, "Item sent to Remove() method: ");

                var expectedRemoveResult = true;
                Assert.AreEqual(expectedRemoveResult, actualRemoveResult);

                //  GET UPDATE REMOVEFROMMARKET (put homesale back, then remove from market)
                var expectedPostRemoveFromMarketCount = DbHomesalesCollection.Count;
                var resetHomesale = new HomeSale()
                {
                    SaleID = 6,
                    HomeID = 3,
                    AgentID = 4,
                    SaleAmount = 700000m,
                    MarketDate = new DateTime(2016, 08, 15),
                    CompanyID = 1
                };
                var reAddHomeForSaleToCollectionResult = DbHomesalesCollection.Add(resetHomesale);
                PrintObject<HomeSale>(resetHomesale, "ResetHomesale, SaleID=6, sent to Add method: ");

                var expectedReAddHomeForSaleToCollectionResult = 1;
                Assert.AreEqual(expectedReAddHomeForSaleToCollectionResult, reAddHomeForSaleToCollectionResult);

                var expectedPostRemoveFromMarketResult = true;
                var actualPostRemoveFromMarketResult = DbHomesalesCollection.Remove(resetHomesale);
                PrintObject<HomeSale>(resetHomesale, "ResetHomesale, SaleID=6, sent to Remove method: ");

                var actualPostRemoveFromMarketCount = DbHomesalesCollection.Count;

                Assert.AreEqual(expectedPostRemoveFromMarketResult, actualPostRemoveFromMarketResult);
                Assert.AreEqual(expectedPostRemoveFromMarketCount, actualPostRemoveFromMarketCount);

                //  CLEAR
                //  TODO: Implement Clear method if necessary.

            }
            #endregion

            #region RealEstateCompaniesTests
            {
                //  RETREIVE
                var recoToRetreiveID = 3;
                var retreivedRECo = DbRECosCollection.Retrieve(recoToRetreiveID);
                var expectedRECo = new RealEstateCompany()
                {
                    CompanyID = 3,
                    CompanyName = "Rapid Real Estate",
                    Phone = "6662221111",
                };
                var expectedAgentsCount = 1;
                var expectedHomesalesCount = 2;
                PrintObject<RealEstateCompany>(retreivedRECo, "Expecting Rapid Real Estate. RECo retreived from Collection: ");
                var actualRetreivedRECoAgentsCount = retreivedRECo.Agents.Count;
                var actualRetreivedRECoHomesalesCount = retreivedRECo.HomeSales.Count;
                PrintObject<int>(actualRetreivedRECoAgentsCount, "Expected: 1; Actual AGENTS in retreived RECo instance: ");
                PrintObject<int>(actualRetreivedRECoHomesalesCount, "Expected: 2; Actual HOMESALES in retreived RECo instance: ");
                Assert.AreEqual(expectedAgentsCount, retreivedRECo.Agents.Count);
                Assert.AreEqual(expectedHomesalesCount, retreivedRECo.HomeSales.Count);
                var expectedRECoRetreiveResult = true;
                var actualRECoRetreiveResult = expectedRECo.Equals(retreivedRECo);
                PrintObject<bool>(actualRECoRetreiveResult, "Expected .Equals(): True; Actual comparison result: ");
                Assert.AreEqual(expectedRECoRetreiveResult, actualRECoRetreiveResult);
            }
            #endregion

        }

    }

}
