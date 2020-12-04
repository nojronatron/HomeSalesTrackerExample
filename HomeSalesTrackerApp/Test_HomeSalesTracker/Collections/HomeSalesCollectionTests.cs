using HSTDataLayer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;

namespace HomeSalesTrackerApp.Tests
{
    [TestClass()]
    public class HomeSalesCollectionTests
    {
        private static HomeSale johnsonvilleHomeSold = new HomeSale()
        {
            HomeID = 1,
            SoldDate = new DateTime(2020, 3, 15),
            AgentID = 3,
            SaleAmount = 1_335_000m,
            BuyerID = 1,
            MarketDate = new DateTime(2020, 3, 1),
            CompanyID = 2
        };

        private static HomeSale UpdateJohnsonvilleHomeSold = new HomeSale()
        {
            HomeID = 1,
            SoldDate = new DateTime(2020, 3, 16),
            AgentID = 3,
            SaleAmount = 1_335_000m,
            BuyerID = 1,
            MarketDate = new DateTime(2020, 3, 1),
            CompanyID = 2
        };

        private static HomeSale mannerHomeSold = new HomeSale()
        {
            HomeID = 2,
            SoldDate = new DateTime(2020, 4, 1),
            AgentID = 2,
            SaleAmount = 1_240_000m,
            BuyerID = 3,
            MarketDate = new DateTime(2020, 3, 29),
            CompanyID = 3
        };

        private static HomeSale larimountHomeSold = new HomeSale()
        {
            HomeID = 3,
            SoldDate = new DateTime(2020, 6, 13),
            AgentID = 1,
            SaleAmount = 1_550_000m,
            BuyerID = 5,
            MarketDate = new DateTime(2020, 6, 1),
            CompanyID = 4
        };

        private static HomeSale woodsworthHomeForSale = new HomeSale()
        {
            HomeID = 4,
            AgentID = 4,
            SaleAmount = 1_700_000m,
            MarketDate = new DateTime(2020, 8, 15),
            CompanyID = 1
        };

        private static HomeSale charlestonHomeForSale = new HomeSale()
        {
            HomeID = 5,
            AgentID = 1,
            SaleAmount = 1_600_000m,
            MarketDate = new DateTime(2020, 07, 01),
            CompanyID = 4
        };

        private static List<HomeSale> initialCollection = new List<HomeSale>()
        {
            mannerHomeSold,
            larimountHomeSold,
            charlestonHomeForSale
        };

        private static HomeSalesCollection _homeSalesCollection;
        private static HSTDataModel _context;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext testContext)
        {
            //  trigger database Seed
            //DropCreateDatabaseAlways<HSTDataModel> dropCreateDatabaseAlways = new DropCreateDatabaseAlways<HSTDataModel>();
            //_context = new HSTDataModel(dropCreateDatabaseAlways);
            _context = new HSTDataModel();
            var dbHomeSales = new List<HomeSale>();

            foreach(var homeSale in _context.HomeSales)
            {
                dbHomeSales.Add(homeSale);
            }

            _homeSalesCollection = new HomeSalesCollection(dbHomeSales);
            Console.WriteLine("TextFixtureSetup completed.");
        }

        //[ClassCleanup]
        //public static void TestFixtureTeardown()
        //{
            
        //}

        //[TestInitialize]
        //public void Setup()
        //{
        //}

        #region TESTS
        [TestMethod()]
        public void InitializeWithItemsTest()
        {
            var expectedResult = 3;
            int actualResult;
            var homeSalesCollection = new HomeSalesCollection(initialCollection);
            actualResult = homeSalesCollection.Count;
            PrintOutput<int>(actualResult, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "local homeSalesCollection.Count");
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void AddHomeSoldTest()
        {
            int preAddCount = 0;
            int postAddCount = 0;
            var expectedResult = 1;
            preAddCount = _homeSalesCollection.Count;
            PrintOutput<int>(_homeSalesCollection.Count, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection Count");
            _homeSalesCollection.Add(johnsonvilleHomeSold);
            PrintOutput<int>(_homeSalesCollection.Count, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection Count");
            postAddCount = _homeSalesCollection.Count;

            int actualResult = postAddCount - preAddCount;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void AddHomeForSaleTest()
        {
            int preAddCount = 0;
            int postAddCount = 0;
            var expectedResult = 1;

            PrintOutput<int>(_homeSalesCollection.Count, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection Count");
            preAddCount = _homeSalesCollection.Count;
            _homeSalesCollection.Add(charlestonHomeForSale);
            postAddCount = _homeSalesCollection.Count;
            PrintOutput<int>(_homeSalesCollection.Count, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection Count");

            int actualResult = postAddCount - preAddCount;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void GetSoldHomeTest()
        {
            var expectedResult = new HomeSale()
            {

            };

            Assert.AreEqual(true, false);
        }

        [TestMethod()]
        public void GetHomeForSaleTest()
        {

            Assert.AreEqual(true, false);
        }

        [TestMethod()]
        public void RetreiveSoldHomeTest()
        {
            var homeSalesCollection = new HomeSalesCollection(initialCollection);
            var retreivedHomeSale = homeSalesCollection.Retreive(mannerHomeSold.SaleID);

            var expectedResult = mannerHomeSold.Equals(mannerHomeSold);
            var actualResult = mannerHomeSold.Equals(retreivedHomeSale);

            PrintOutput<HomeSale>(retreivedHomeSale, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "local homeSalesCollection object");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void RetreiveHomeForSaleTest()
        {
            var homeSalesCollection = new HomeSalesCollection(initialCollection);
            var retreivedHomeSale = homeSalesCollection.Retreive(woodsworthHomeForSale.SaleID);

            var expectedResult = woodsworthHomeForSale.Equals(woodsworthHomeForSale);
            var actualResult = (woodsworthHomeForSale.Equals(retreivedHomeSale));

            PrintOutput<HomeSale>(retreivedHomeSale, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "local homeSalesCollection object");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void UpdateExistingSoldHomeTest()
        {
            int expectedResult = 0;
            int preUpdateCount = -1;
            int postUpdateCount = 0;

            preUpdateCount = _homeSalesCollection.Count;
            _homeSalesCollection.Update(UpdateJohnsonvilleHomeSold);
            postUpdateCount = _homeSalesCollection.Count;

            int actualResult = postUpdateCount - preUpdateCount;

            PrintOutput<int>(preUpdateCount, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection count");
            PrintOutput<int>(postUpdateCount, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection count");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void UpdateExistingHomeForSaleTest()
        {
            int expectedResult = 0;
            int preUpdateCount = -1;
            int postUpdateCount = 0;

            preUpdateCount = _homeSalesCollection.Count;
            _homeSalesCollection.Update(UpdateJohnsonvilleHomeSold);
            postUpdateCount = _homeSalesCollection.Count;

            int actualResult = postUpdateCount - preUpdateCount;

            PrintOutput<int>(preUpdateCount, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection count");
            PrintOutput<int>(postUpdateCount, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "_homeSalesCollection count");
            
            Assert.AreEqual(expectedResult, actualResult);
        }
        #endregion TESTS

        private void PrintOutput<T>(T thing, string function, string argName)
        {
            Console.WriteLine($"***** { function } *****");
            Console.WriteLine($"*** { argName } ***");
            Console.WriteLine(thing.ToString());
            Console.WriteLine();
        }

    }

}
