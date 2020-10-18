using HSTDataLayer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace HomeSalesTrackerApp.Tests
{
    [TestClass()]
    public class HomeSalesCollectionTests
    {
        private static HomeSale alpha = new HomeSale()
        {
            SaleID = 1,
            HomeID = 1,
            SoldDate = new DateTime(2010, 3, 15, 0, 0, 0),
            AgentID = 3,
            SaleAmount = 335_000m,
            BuyerID = 1,
            MarketDate = new DateTime(2015, 3, 1),
            CompanyID = 2
        };

        private static HomeSale bravo = new HomeSale()
        {
            SaleID = 2,
            HomeID = 2,
            SoldDate = new DateTime(2000, 4, 1, 0, 0, 0),
            AgentID = 2,
            SaleAmount = 240_000m,
            BuyerID = 3,
            MarketDate = new DateTime(2000, 3, 29),
            CompanyID = 3
        };

        private static HomeSale charlie = new HomeSale()
        {
            SaleID = 3,
            HomeID = 3,
            SoldDate = new DateTime(2014, 6, 13, 0, 0, 0),
            AgentID = 1,
            SaleAmount = 550_000m,
            BuyerID = 5,
            MarketDate = new DateTime(2014, 6, 1),
            CompanyID = 4
        };

        private static HomeSale foxtrot = new HomeSale()
        {
            SaleID = 6,
            HomeID = 3,
            AgentID = 4,
            SaleAmount = 700_000m,
            MarketDate = new DateTime(2016, 8, 15, 0, 0, 0),
            CompanyID = 1
        };

        [TestMethod()]
        public void CanInitialize()
        {
            var homeSalesCollection = new HomeSalesCollection();

            var expectedResult = true;
            var actualResult = (homeSalesCollection != null);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void CanAdd()
        {
            var homeSalesCollection = new HomeSalesCollection();
            homeSalesCollection.Add(alpha);
            homeSalesCollection.Add(bravo);
            homeSalesCollection.Add(charlie);

            var expectedResult = 3;
            var actualResult = homeSalesCollection.Count;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void CanRetrieveItem()
        {
            var homeSalesCollection = new HomeSalesCollection();
            homeSalesCollection.Add(alpha);

            var expectedResult = alpha.Equals(alpha);
            var actualResult = (homeSalesCollection[0].Equals(alpha));

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void CanUpdateExistingItem()
        {
            var homeSalesCollection = new HomeSalesCollection();
            homeSalesCollection.Add(alpha);

            var alphaUpdate = new HomeSale()
            {
                SaleID = 1,
                HomeID = 1,
                SoldDate = new DateTime(2010, 5, 15, 0, 0, 0),
                AgentID = 3,
                SaleAmount = 335_000m,
                BuyerID = 1,
                MarketDate = new DateTime(2015, 3, 1),
                CompanyID = 2
            };

            homeSalesCollection.Update(alphaUpdate);

            var expectedResult = true;
            var actualResult = (homeSalesCollection[0].Equals(alphaUpdate));

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void CanCountTotalHomesSold()
        {
            var homeSalesCollection = new HomeSalesCollection();
            homeSalesCollection.Add(alpha);
            homeSalesCollection.Add(bravo);
            homeSalesCollection.Add(charlie);
            homeSalesCollection.Add(foxtrot);

            int expectedResult = 3;
            int actualResult = homeSalesCollection.Count;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}