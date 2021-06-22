using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeBandChecker_Test
{
    [TestClass]
    public class TimebandsTest
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

        //ROS TESTS
        [TestMethod]
        public void ValidROSValue()
        {
            var timebandStart = "9AM";
            var timebandEnd = "10am";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsROS(timebandStart, timebandEnd, ipsos);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidUnderROSValue()
        {
            var timebandStart = "10:30AM";
            var timebandEnd = "11:00am";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsROS(timebandStart, timebandEnd, ipsos);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InvalidOverROSValue()
        {
            var timebandStart = "9AM";
            var timebandEnd = "9:30am";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsROS(timebandStart, timebandEnd, ipsos);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InvalidUnderROSValue()
        {
            var timebandStart = "11:30AM";
            var timebandEnd = "12:30pm";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsROS(timebandStart, timebandEnd, ipsos);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WithinRangeButNotROS()
        {
            var timebandStart = "9AM";
            var timebandEnd = "10:30am";
            var ipsos = "10:22";

            var checker = new TimeBandChecker.TimeBandChecker();
            var result = checker.IsROS(timebandStart, timebandEnd, ipsos);
            Assert.IsFalse(result);
        }
    }
}
