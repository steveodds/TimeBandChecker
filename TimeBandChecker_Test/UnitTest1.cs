using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeBandChecker_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ValidTimeTest()
        {
            var timebandStart = "9AM";
            var timebandEnd= "9.30AM";
            var ipsos = "09:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsWithinTimebandRange(timebandStart, timebandEnd, ipsos);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidTimeTestMilitaryTime()
        {
            var timebandStart = "2PM";
            var timebandEnd = "2.30PM";
            var ipsos = "14:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsWithinTimebandRange(timebandStart, timebandEnd, ipsos);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MilitaryTimeTest()
        {
            var timebandStart = "18:00";
            var timebandEnd = "18:45";
            var ipsos = "18:02";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsWithinTimebandRange(timebandStart, timebandEnd, ipsos);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OutsideTimebandTest()
        {
            var timebandStart = "9AM";
            var timebandEnd = "9.30AM";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsWithinTimebandRange(timebandStart, timebandEnd, ipsos);
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void InvalidTimeTest()
        {
            var timebandStart = "random";
            var timebandEnd = "word";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsWithinTimebandRange(timebandStart, timebandEnd, ipsos);
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void NullTimeTest()
        {
            var timebandStart = "9AM";
            var timebandEnd = "";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsWithinTimebandRange(timebandStart, timebandEnd, ipsos);
            Assert.IsFalse(result);
        }

    }
}
