using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeBandChecker_Test
{
    [TestClass]
    public class WeekOfTheMonthTest
    {
        [TestMethod]
        public void BasicDate()
        {
            var WeekMonth = new TimeBandChecker.WeekOfTheMonth();
            var week = WeekMonth.GetWeek("25/06/2021");
            Assert.AreEqual(week, 4);
        }

        [TestMethod]
        public void NoLeadingZeroInMonth()
        {
            var WeekMonth = new TimeBandChecker.WeekOfTheMonth();
            var week = WeekMonth.GetWeek("25/6/2021");
            Assert.AreEqual(week, 4);
        }

        [TestMethod]
        public void LeapYear()
        {
            var WeekMonth = new TimeBandChecker.WeekOfTheMonth();
            var week = WeekMonth.GetWeek("29/02/2020");
            Assert.AreEqual(week, 5);
        }

        [TestMethod]
        public void NonExistentDate()
        {
            var WeekMonth = new TimeBandChecker.WeekOfTheMonth();
            var week = WeekMonth.GetWeek("29/02/2021");
            Assert.AreEqual(week, 0);
        }

        [TestMethod]
        public void SingleDigitDay()
        {
            var WeekMonth = new TimeBandChecker.WeekOfTheMonth();
            var week = WeekMonth.GetWeek("1/06/2021");
            Assert.AreEqual(week, 1);
        }

        [TestMethod]
        public void SignleDigitDayAndMonth()
        {
            var WeekMonth = new TimeBandChecker.WeekOfTheMonth();
            var week = WeekMonth.GetWeek("1/6/2021");
            Assert.AreEqual(week, 1);
        }
    }
}
