using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer;
using System;
using System.Text;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class HomeSaleTests
    {
        private HomeSale johnsonvilleCaSale = new HomeSale()
        {
            HomeID = 1,
            SoldDate = new DateTime(2010, 03, 15, 00, 00, 00),
            AgentID = 3,
            SaleAmount = 335000.0000m,
            BuyerID = 1,
            MarketDate = new DateTime(2015, 3, 1),
            CompanyID = 2
        };

        private HomeSale MannerWaSale = new HomeSale()
        {
            HomeID = 2,
            SoldDate = new DateTime(2000, 04, 01, 00, 00, 00),
            AgentID = 2,
            SaleAmount = 240000.0000m,
            BuyerID = 3,
            MarketDate = new DateTime(2000, 3, 29),
            CompanyID = 3
        };

        private HomeSale johnsonvilleCaSaleCopy = new HomeSale()
        {
            HomeID = 1,
            SoldDate = new DateTime(2010, 03, 15, 00, 00, 00),
            AgentID = 3,
            SaleAmount = 335000.0000m,
            BuyerID = 1,
            MarketDate = new DateTime(2015, 3, 1),
            CompanyID = 2
        };

        [TestMethod()]
        public void ToStringTest()
        {
            StringBuilder actualResult = new StringBuilder(johnsonvilleCaSale.ToString());
            Console.WriteLine(actualResult);

            Assert.IsNotNull(actualResult, "See additional output.");
        }

        [TestMethod()]
        public void EqualityTest()
        {
            bool expectedResult = true;
            bool actualResult = johnsonvilleCaSale.Equals(johnsonvilleCaSaleCopy);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityTest()
        {
            bool expectedResult = false;
            bool actualResult = johnsonvilleCaSale.Equals(MannerWaSale);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void EqualityLeftRightTest()
        {
            bool expectedResult = true;
            bool actualResult = (johnsonvilleCaSale == johnsonvilleCaSaleCopy);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityLeftRightTest()
        {
            bool expectedResult = true;
            bool actualResult = (johnsonvilleCaSale != MannerWaSale);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}