using Microsoft.VisualStudio.TestTools.UnitTesting;
using HSTDataLayer;
using System;
using System.Text;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class PersonTests
    {
        private Person unahOno = new Person()
        {
            FirstName = "Unah",
            LastName = "Ono",
            Phone = "425-555-1212",
            Email = "Unah.Ono@amail.com"
        };

        private Person debbieDublee = new Person()
        {
            FirstName = "Debbie",
            LastName = "Dublee",
            Phone = "206-555-1212",
            Email = "Debbie.Dublee@bmail.com"
        };

        private Person treyTriplett = new Person()
        {
            FirstName = "Trey",
            LastName = "Triplett",
            Phone = "509-555-1212",
            Email = "Trey.Triplett@cmail.com"
        };

        [TestMethod()]
        public void ToStringTest()
        {
            StringBuilder actualResult = new StringBuilder(unahOno.ToString());
            Console.WriteLine(actualResult);
            
            Assert.IsNotNull(actualResult, "See additional output.");
        }

        [TestMethod()]
        public void EqualityTest()
        {
            Person secondPerson = new Person()
            {
                FirstName = "Unah",
                LastName = "Ono",
                Phone = "425-555-1212",
                Email = "Unah.Ono@amail.com"
            };
            Console.WriteLine($"p1: { unahOno.ToString() }\nsecondPerson: { secondPerson.ToString() }");

            bool expectedResult = true;
            bool actualResult = unahOno.Equals(secondPerson);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityTest()
        {
            bool expectedResult = false;
            bool actualResult = unahOno.Equals(debbieDublee);

            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod()]
        public void EqualityLeftRightTest()
        {
            Person secondPerson = new Person()
            {
                FirstName = "Unah",
                LastName = "Ono",
                Phone = "425-555-1212",
                Email = "Unah.Ono@amail.com"
            };
            Console.WriteLine($"p1: { unahOno.ToString() }\nsecondPerson: { secondPerson.ToString() }");

            bool expectedResult = true;
            bool actualResult = (unahOno == secondPerson);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void InequalityLeftRightTest()
        {
            bool expectedResult = true;
            bool actualResult = (unahOno != debbieDublee);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}