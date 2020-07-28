using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer;
using System;
using System.Text;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class RealEstateCompanyTests
    {
        private RealEstateCompany AbcRE = new RealEstateCompany()
        {
            CompanyName = "ABC Real Estate",
            Phone = "6663331111"
        };

        private RealEstateCompany BcdRE = new RealEstateCompany()
        {
            CompanyName = "BCD Real Estate",
            Phone = "6664441111"
        };

        
        [TestMethod()]
        public void ToStringTest()
        {
            StringBuilder actualResult = new StringBuilder(AbcRE.ToString());
            Console.WriteLine(actualResult);

            Assert.IsNotNull(actualResult, "See additional output.");
        }

        [TestMethod()]
        public void EqualityTest()
        {
            bool expectedResult = true;
            RealEstateCompany BcdReCopy = new RealEstateCompany()
            {
                CompanyName = "BCD Real Estate",
                Phone = "6664441111"
            };
            bool actualResult = BcdRE.Equals(BcdReCopy);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityTest()
        {
            bool expectedResult = false;
            bool actualResult = AbcRE.Equals(BcdRE);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void EqualityLeftRightTest()
        {
            bool expectedResult = true;
            RealEstateCompany BcdReCopy = new RealEstateCompany()
            {
                CompanyName = "BCD Real Estate",
                Phone = "6664441111"
            };
            bool actualResult = (BcdRE == BcdReCopy);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityLeftRightTest()
        {
            bool expectedResult = true;
            bool actualResult = (AbcRE != BcdRE);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}