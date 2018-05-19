using System;
using System.Globalization;

namespace Evant.Helpers
{
    public static class DateTimeExtensions
    {
        public static Tuple<DateTime, DateTime> Week()
        {
            var date = DateTime.Now;

            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int offset = fdow - date.DayOfWeek;
            DateTime fdowDate = date.AddDays(offset);
            DateTime ldowDate = fdowDate.AddDays(6);

            return Tuple.Create(fdowDate, ldowDate);
        }

        public static Tuple<DateTime, DateTime> Month()
        {
            var date = DateTime.Now;

            DateTime fdomDate = new DateTime(date.Year, date.Month, 1);
            DateTime ldomDate = fdomDate.AddMonths(1).AddDays(-1);

            return Tuple.Create(fdomDate, ldomDate);
        }

    }
}
