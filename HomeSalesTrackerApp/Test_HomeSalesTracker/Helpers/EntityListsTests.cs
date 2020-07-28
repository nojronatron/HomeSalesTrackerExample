using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSTDataLayer.Helpers.Tests
{
    [TestClass()]
    public class EntityListsTests
    {
        private static readonly Person lyle = new Person()
        {
            PersonID = 5,
            FirstName = "Lyle",
            LastName = "Hutton",
            Phone = "1113332222"
        };
        private static readonly Person anna = new Person()
        {
            PersonID = 7,
            FirstName = "Anna",
            LastName = "Larson",
            Phone = "5551114444",
            Email = "alarson@a.com"
        };

        private static readonly Home woodsworthHome = new Home()
        {
            HomeID = 4,
            Address = "6 Hindley Road",
            City = "Woodsworth",
            State = "OR",
            Zip = "955553333",
            OwnerID = 7
        };

        private static readonly Home charlestonHome = new Home()
        {
            HomeID = 5,
            Address = "123 8th Ave.",
            City = "Charleston",
            State = "OR",
            Zip = "922223333",
            OwnerID = 5
        };

        private static readonly Owner lyleOwner = new Owner()
        {
            OwnerID = 5,
            PreferredLender = "Bank of Nova Scotia",
            Person = lyle
        };

        private static readonly Owner annaOwner = new Owner()
        {
            OwnerID = 7,
            PreferredLender = "Fine Financial",
            Person = anna
        };

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            HSTContextInitializer.InitDB();
            HSTContextInitializer.LoadDataIntoDatabase();
        }

        //[TestMethod()]
        //public void GetListOfOwnerPeopleTest()
        //{
        //    //  TODO: This test is just for experimentation at this time but should be converted
        //    var personList = new List<Person>();
        //    personList.Add(lyle);
        //    personList.Add(anna);

        //    var ownerList = new List<Owner>();
        //    ownerList.Add(lyleOwner);
        //    ownerList.Add(annaOwner);
            
        //    var peopleOwners = EntityLists.GetListOfOwnerPeople(personList, ownerList);
            
        //    PrintEntities(personList, "People in personList");
        //    PrintEntities(ownerList, "Owners in ownerList");
        //    PrintEntities(peopleOwners, "People that are owners list");

        //    Assert.Fail("See additional output.");
        //}

        [TestMethod()]
        public void GetListOfPeopleLyleAnnaTest()
        {
            bool expectedResult = true;

            List<Person> actualPeopleList = EntityLists.GetListOfPeople();
            bool lyleFound = actualPeopleList.Contains(lyle);
            bool annaFound = actualPeopleList.Contains(anna);
            bool actualResult = (lyleFound == true && annaFound == true);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetListOfPeopleCountTest()
        {
            int expectedCount = 10;

            List<Person> actualPeopleList = EntityLists.GetListOfPeople();
            int actualCount = actualPeopleList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetListOfOwnersCountTest()
        {
            int expectedCount = 4;

            List<Owner> actualOwnersList = EntityLists.GetListOfOwners();
            int actualCount = actualOwnersList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetListOfHomesCount()
        {
            int expectedCount = 5;

            List<Home> actualHomesList = EntityLists.GetListOfHomes();
            int actualCount = actualHomesList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetListOfHomesLastTwoValidTest()
        {
            //  Regression test: hindley and charleston home addresses were corrupted in previous revisions of the code
            bool expectedMatchResult = true;

            List<Home> homes = EntityLists.GetListOfHomes();
            Home woodsworthHomeFromDB = homes.Find(x => x.City == "Woodsworth");
            Home charlestonHomeFromDB = homes.Find(x => x.City == "Charleston");

            //  visual validation (if needed)
            Console.WriteLine($"Expected Woodsworth Home address: { woodsworthHome.Address } Zip: { woodsworthHome.Zip } \n");
            Console.WriteLine($"Woodsworth Home (from DB) address: { woodsworthHomeFromDB.Address } Zip: { woodsworthHomeFromDB.Zip } \n");
            Console.WriteLine($"Expected Charleston Home address: { charlestonHome.Address } Zip: { charlestonHome.Zip } \n");
            Console.WriteLine($"Charleston Home (from DB) address: { charlestonHomeFromDB.Address } Zip: { charlestonHomeFromDB.Zip } ");

            //  explicit logic to determine sameness without implementing additional App code just for a test
            bool actualWoodsworthAddressesMatch = (woodsworthHome.Address == woodsworthHomeFromDB.Address);
            bool actualWoodsworthHomeZipcodesMatch = (woodsworthHome.Zip == woodsworthHomeFromDB.Zip);
            bool actualCharlestonAddressesMatch = (charlestonHome.Address == charlestonHomeFromDB.Address);
            bool actualCharlestonZipcodesMatch = (charlestonHome.Zip == charlestonHomeFromDB.Zip);
            bool actualMatchResult = (actualWoodsworthHomeZipcodesMatch == true && actualWoodsworthAddressesMatch == true &&
                                      actualCharlestonZipcodesMatch == true && actualCharlestonAddressesMatch == true);

            Assert.AreEqual(expectedMatchResult, actualMatchResult);
        }

        [TestMethod()]
        public void GetListOfRECompaniesCountTest()
        {
            int expectedCount = 4;

            List<RealEstateCompany> actualRECosList = EntityLists.GetListOfRECompanies();
            int actualCount = actualRECosList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetListOfAgentsCountTest()
        {
            int expectedCount = 5;

            List<Agent> actualAgentsList = EntityLists.GetListOfAgents();
            int actualCount = actualAgentsList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetListOfBuyersCountTest()
        {
            int expectedCount = 4;

            List<Buyer> actualBuyersList = EntityLists.GetListOfBuyers();
            int actualCount = actualBuyersList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod()]
        public void GetListOfHomeSalesCountTest()
        {
            int expectedCount = 8;

            List<HomeSale> actualHomesalesList = EntityLists.GetListOfHomeSales();
            int actualCount = actualHomesalesList.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        private static void PrintEntities<T>(List<T> list, string message)
        {
            Console.WriteLine("###################################");
            Console.WriteLine(message);
            foreach (T item in list)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("###################################\n");
        }
    }
}
