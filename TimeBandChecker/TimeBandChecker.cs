using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBandChecker
{
    public class TimeBandChecker
    {

        public List<string> Reconcile(List<string> adamsList, List<string> ipsosList)
        {
            var reconciliator = new Reconciliator(adamsList, ipsosList);
            var reconciled = reconciliator.Reconcile();
            var finalRecords = ReconciledAsString(reconciled);

            return finalRecords;
        }

        private List<string> ReconciledAsString(List<Reconciled> reconciled)
        {
            var results = new List<string>();
            foreach (var record in reconciled)
            {
                results.Add($"{record.AdamsRow}|{record.IpsosRow}|{record.Status}");
            }

            return results;
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

    internal static class DateTimeExtensions
    {
        private static readonly GregorianCalendar _gc = new GregorianCalendar();
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
