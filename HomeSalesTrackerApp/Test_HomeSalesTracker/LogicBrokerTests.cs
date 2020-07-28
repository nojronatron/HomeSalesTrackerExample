using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HSTDataLayer.Tests
{
    [TestClass()]
    public class LogicBrokerTests
    {
        //  Do not worry about these tests until done with datalayer development and testing
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
    }
}