using HSTDataLayer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace HomeSalesTrackerApp.Tests
{
    [TestClass()]
    public class PeopleCollectionTests
    {
        private Person unahOno = new Person()
        {
            PersonID = 1,
            FirstName = "Unah",
            LastName = "Ono",
            Phone = "425-555-1212",
            Email = "Unah.Ono@amail.com"
        };

        private Person debbieDublee = new Person()
        {
            PersonID = 2,
            FirstName = "Debbie",
            LastName = "Dublee",
            Phone = "206-555-1212",
            Email = "Debbie.Dublee@bmail.com"
        };

        private Person treyTriplett = new Person()
        {
            PersonID = 3,
            FirstName = "Trey",
            LastName = "Triplett",
            Phone = "509-555-1212",
            Email = "Trey.Triplett@cmail.com"
        };

        private Agent agentOne = new Agent()
        {
            AgentID = 1,
            CompanyID = 4,
            CommissionPercent = 0.02m,
            Person = new Person()
            {
                PersonID = 1,
                FirstName = "Unah",
                LastName = "Ono",
                Phone = "425-555-1212",
                Email = "Unah.Ono@amail.com"
            }
        };

        private Agent agentTwo = new Agent()
        {
            AgentID = 2,
            CompanyID = 1,
            CommissionPercent = 0.02m
        };

        private Agent agentThree = new Agent()
        {
            AgentID = 3,
            CommissionPercent = 0.03m
        };
        private Buyer buyerOne = new Buyer()
        {
            BuyerID = 1,
            CreditRating = 735
        };

        private Buyer buyerThree = new Buyer()
        {
            BuyerID = 3
        };

        private Buyer buyerFive = new Buyer()
        {
            BuyerID = 5,
            CreditRating = 700
        };

        private Owner ownerOne = new Owner()
        {
            OwnerID = 1,
            PreferredLender = "ABC Mortgage Company"
        };

        private Owner ownerThree = new Owner()
        {
            OwnerID = 3,
            PreferredLender = "Allied Mortgage Company"
        };

        private Owner ownerFive = new Owner()
        {
            OwnerID = 5
        };

        [TestMethod()]
        public void CollectionOfPerson()
        {
            var people = new PeopleCollection<Person>();
            var expectedResult = true;
            var actualResult = (people.GetType().Name == "Person");
            Console.WriteLine($"CollectionOfPerson: expectedResult={ expectedResult }; actualResult={ actualResult }");
            Assert.AreEqual(expectedResult, actualResult);
        }

        //[TestMethod()]
        //public void CollectionOfOwner()
        //{
        //    var owners = new PeopleCollection<Owner>();
        //    var expectedResult = true;
        //    var actualResult = (owners.GetType().Name == "Owner");
        //    Console.WriteLine($"CollectionOfPerson: expectedResult={ expectedResult }; actualResult={ actualResult }");
        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod()]
        //public void CollectionOfAgent()
        //{
        //    var agents = new PeopleCollection<Agent>();
        //    agents.Add(agentOne);
        //    var expectedResult = true;
        //    var actualResult = agents[0].GetType().Name == "Agent";
        //    Console.WriteLine($"CollectionOfPerson: expectedResult={ expectedResult }; actualResult={ actualResult }");
        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod()]
        //public void CollectionOfBuyer()
        //{
        //    var buyers = new PeopleCollection<Buyer>();
        //    var expectedResult = true;
        //    var actualResult = (buyers.GetType().Name == "Buyer");
        //    Console.WriteLine($"CollectionOfPerson: expectedResult={ expectedResult }; actualResult={ actualResult }");
        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod()]
        //public void AddTest()
        //{
        //    var peopleCollection = new PeopleCollection<Person>();
        //    peopleCollection.Add(unahOno);
        //    peopleCollection.Add(ownerOne);
        //    peopleCollection.Add(agentOne);
        //    peopleCollection.Add(buyerOne);
        //    var expectedResult = 4;
        //    var actualResult = peopleCollection.Count;
        //    Console.WriteLine($"CollectionOfPerson: expectedResult={ expectedResult }; actualResult={ actualResult }");
        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod()]
        //public void GetTest()
        //{
        //    var peopleCollection = new PeopleCollection<Person>();
        //    peopleCollection.Add(unahOno);
        //    peopleCollection.Add(ownerOne);
        //    peopleCollection.Add(agentOne);
        //    peopleCollection.Add(buyerOne);
        //    var actualPerson = peopleCollection.Get(3);
        //    //  should be Trey Tripplet

        //    var expectedPerson = new Person()
        //    {
        //        PersonID = 3,
        //        FirstName = "Trey",
        //        LastName = "Triplett",
        //        Phone = "509-555-1212",
        //        Email = "Trey.Triplett@cmail.com"
        //    };

        //    var expectedResult = true;
        //    var actualResult = expectedPerson.Equals(actualPerson);

        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod()]
        //public void GetEnumeratorTest()
        //{
        //    var peopleCollection = new PeopleCollection<Person>();
        //    peopleCollection.Add(unahOno);
        //    peopleCollection.Add(ownerOne);
        //    peopleCollection.Add(agentOne);
        //    peopleCollection.Add(buyerOne);
        //    var actualPerson = (from pc in peopleCollection
        //                        where pc.PersonID == 2
        //                        select pc).FirstOrDefault();
        //    var expectedPerson = new Person()
        //    {
        //        PersonID = 2,
        //        FirstName = "Debbie",
        //        LastName = "Dublee",
        //        Phone = "206-555-1212",
        //        Email = "Debbie.Dublee@bmail.com"
        //    };
        //    var expectedResult = true;
        //    var actualResult = actualPerson.Equals(expectedPerson);
        //    Assert.AreEqual(expectedResult, actualResult);
        //}

        //[TestMethod()]
        //public void MoveNextTest()
        //{
        //    int actualCounter = 0;
        //    var peopleCollection = new PeopleCollection<Person>();
        //    peopleCollection.Add(unahOno);
        //    peopleCollection.Add(ownerOne);
        //    peopleCollection.Add(agentOne);
        //    foreach (var p in peopleCollection)
        //    {
        //        if (p != null)
        //        {
        //            actualCounter++;
        //        }
        //    }
        //    int expectedCounter = 3;
        //    Assert.AreEqual(expectedCounter, actualCounter);
        //}
    }
}
