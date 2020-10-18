using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeSalesTrackerApp.Helpers.Tests
{
    [TestClass()]
    public class LoggerTests
    {
        [TestMethod()]
        public void SingleEntry()
        {
            var log = new Helpers.Logger();

            log.Data("Test Entry", "lkj wer asdf oiu zxcv ,mnb lkj asdf wer");
            log.Flush();

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void MultipleEntry()
        {
            var log = new Helpers.Logger();

            log.Data("Test Entry", "lkj wer asdf oiu zxcv ,mnb lkj asdf wer");
            log.Data("Test Entry", "wer asdf oiu zxcv ,mnb lkj asdf wer lkj ");
            log.Data("Test Entry", "asdf werlkj wer asdf oiu zxcv ,mnb lkj ");
            log.Data("Test Entry", "lkj wer  ,mnb lkj asdf wer asdf oiu zxcv");
            log.Data("Test Entry", "zxcv ,mnb lkj asdf wer lkj wer asdf oiu ");

            log.Flush();

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void Initialize()
        {
            bool expectedResult = true;
            var log = new Helpers.Logger();

            bool actualResult = log.IsEnabled;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}