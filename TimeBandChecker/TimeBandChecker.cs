﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBandChecker
{
    public class TimeBandChecker
    {
        public bool IsWithinTimebandRange(string timebandStart, string timebandEnd, string ipsosTime)
        {
            if (string.IsNullOrWhiteSpace(timebandStart) || string.IsNullOrWhiteSpace(timebandEnd))
                return false;

            bool isValidTime = DateTime.TryParse(TimeFormatter(timebandStart), out DateTime tbStart);
            if (!isValidTime)
                return false;

            isValidTime = DateTime.TryParse(TimeFormatter(timebandEnd), out DateTime tbEnd);
            if(!isValidTime)
                return false;

            isValidTime = DateTime.TryParse(ipsosTime, out DateTime ipsos);
            if (!isValidTime)
                return false;

            return ipsos.TimeOfDay >= tbStart.TimeOfDay && ipsos.TimeOfDay <= tbEnd.TimeOfDay;
        }

        private string TimeFormatter(string time)
        {
            time = time.Trim();
            if (time.Contains('.'))
                time = time.Replace('.', ':');
            if (!time.Contains(":"))
                time = time.Substring(0, time.Length - 2).Trim() + ":00" + time.Substring(time.Length - 2);

            return time;
        }
    }

    public class WeekOfTheMonth
    {
        public int GetWeek(string date)
        {
            var isValidDate = DateTime.TryParseExact(date,
                new string[] { "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy" },
                new CultureInfo("en-US"), DateTimeStyles.None,
                out DateTime dateObject);
            if (!isValidDate)
                return 0;

            return dateObject.GetWeekOfMonth();
        }
    }

    public static class DateTimeExtensions
    {
        private static GregorianCalendar _gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }
        public static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
}