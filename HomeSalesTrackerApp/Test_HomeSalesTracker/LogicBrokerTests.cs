using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class LogicBrokerTests
    {
        [TestMethod()]
        public void InitDbTest()
        {
            bool actualResult = false;
            try
            {
                actualResult = LogicBroker.InitDatabase();
            }
            catch
            {
                Console.WriteLine("An Exception occurred when running LogicBroker.InitDatabase()!");
                actualResult = false;
                throw;
            }
            Assert.IsTrue(actualResult);
        }

        [TestMethod()]
        public void LoadDataTest()
        {
            bool result = false;
            result = LogicBroker.LoadData();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void InitLoadDataTest()
        {
            bool actualResult = false;
            bool initDbSucceeded = true;
            try
            {
                initDbSucceeded = LogicBroker.InitDatabase();
            }
            catch
            {
                initDbSucceeded = false;
            }
            bool logicBrokerLoadDataSucceeded = LogicBroker.LoadData();
            actualResult = (initDbSucceeded == true && logicBrokerLoadDataSucceeded == true);
            Assert.IsTrue(actualResult);
        }

        [TestMethod()]
        public void BackUpDatabaseTest()
        {
            bool result = false;
            bool loadDataSucceeded = LogicBroker.LoadData();
            if (loadDataSucceeded)
            {
                result = LogicBroker.BackUpDatabase();
            }
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void AddBuyerToDB()
        {
            bool expectedResult = true;
            bool actualResult = false;

            if (LogicBroker.LoadData())
            {
                var peopleList = new List<Person>();
                GetListOfPeopleEntities(ref peopleList);
                var samHolder = peopleList.Where(p => p.FirstName == "Sam" && p.LastName == "Holder").FirstOrDefault();
                Buyer buyer = new Buyer()
                {
                    CreditRating = 666,
                    BuyerID = 8,
                    Person = samHolder
                };

                //actualResult = LogicBroker.SaveEntity(buyer); //  throws a foreign key exception
                actualResult = LogicBroker.UpdateEntity(buyer); //  returns false but doesn't throw exception
            }

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void UpdateBuyerToDB()
        {
            bool expectedResult = true;
            bool actualResult = false;

            var newPerson = new Person()
            {
                FirstName = "New",
                LastName = "Person",
                Phone = "1234567890",
                Email = ""
            };
            Buyer buyer = new Buyer()
            {
                CreditRating = 777,
                Person = newPerson
            };

            actualResult = LogicBroker.UpdateEntity<Buyer>(buyer);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void AddPersonToDB()
        {
            bool expectedResult = true;
            bool actualResult = false;

            var newPerson = new Person()
            {
                FirstName = "Robert",
                LastName = "Plant",
                Phone = "7074336107"
            };

            actualResult = LogicBroker.UpdateEntity<Person>(newPerson);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void UpdatePersonToDB()
        {
            bool expectedresult = true;
            bool actualResult = false;
            var peopleList = new List<Person>();

            GetListOfPeopleEntities(ref peopleList);

            Person updatedPerson = peopleList.Where(p => p.FirstName == "Robert" && p.LastName == "Plant").FirstOrDefault();
            if (updatedPerson != null)
            {
                updatedPerson.Email = "YouNeedLove@Zep.info";
                actualResult = LogicBroker.UpdateEntity<Person>(updatedPerson);
            }

            Assert.AreEqual(expectedresult, actualResult);
        }

        private static void GetListOfPeopleEntities(ref List<Person> peopleList)
        {
            using (var context = new HSTDataModel())
            {
                foreach (Person p in context.People)
                {
                    peopleList.Add(p);
                }
            }
        }

    }
}