using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer;
using System;
using System.Text;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class HomeTests
    {
        private Home everettHome = new Home()
        {
            Address = "123 First Ave N",
            City = "Everett",
            State = "WA",
            Zip = "982071111",
            OwnerID = -1
        };

        private Home tacomaHome = new Home()
        {
            Address = "3546 Secondary Way E",
            City = "Tacoma",
            State = "WA",
            Zip = "984032222",
            OwnerID = -2
        };

        private Home woodsworthHome = new Home()
        {
            Address = "6 Hindley Road",
            City = "Woodsworth",
            State = "OR",
            Zip = "955553333",
            OwnerID = -2
        };

        [TestMethod()]
        public void ToStringTest()
        {
            StringBuilder actualResult = new StringBuilder(woodsworthHome.ToString());
            Console.WriteLine(actualResult);

            Assert.IsNotNull(actualResult, "See additional output.");
        }
        
        [TestMethod()]
        public void EqualityTest()
        {
            bool expectedResult = true;
            Home everettHomeCopy = new Home()
            {
                Address = "123 First Ave N",
                City = "Everett",
                State = "WA",
                Zip = "982071111",
                OwnerID = -1
            };
            bool actualResult = everettHome.Equals(everettHomeCopy);
            Assert.AreEqual(expectedResult, actualResult);
        }
        
        [TestMethod()]
        public void InequalityTest()
        {
            bool expectedResult = false;
            bool actualResult = everettHome.Equals(tacomaHome);
            Assert.AreEqual(expectedResult, actualResult);
        }
 
        [TestMethod()]
        public void EqualityLeftRightTest()
        {
            bool expectedResult = true;
            Home everettHomeCopy = new Home()
            {
                Address = "123 First Ave N",
                City = "Everett",
                State = "WA",
                Zip = "982071111",
                OwnerID = -1
            };
            bool actualResult = ( everettHome == everettHomeCopy);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityLeftRightTest()
        {
            bool expectedResult = true;
            bool actualResult = ( everettHome != tacomaHome);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}