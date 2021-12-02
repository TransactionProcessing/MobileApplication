using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionMobile.Extensions
{
    public static class Extensions
    {
        public static List<(String displayText, DateTime startDate, DateTime endDate)> GenerateDatePeriods(this DateTime currentDate, Int32 numberHistoricalMonths = 0)
        {
            List<(String displayText, DateTime startDate, DateTime endDate)> datePeriods = new List<(String displayText, DateTime startDate, DateTime endDate)>();

            Int32 daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            // Create the current range
            datePeriods.Add(($"{currentDate:MMMM yyyy}",
                            new DateTime(currentDate.Year, currentDate.Month, 1),
                            new DateTime(currentDate.Year, currentDate.Month, daysInMonth)));
            
            for (Int32 i = 1; i <= numberHistoricalMonths; i++)
            {
                DateTime nextdate = currentDate.AddMonths(i * -1);
                daysInMonth = DateTime.DaysInMonth(nextdate.Year, nextdate.Month);
                datePeriods.Add(($"{nextdate:MMMM yyyy}",
                                     new DateTime(nextdate.Year, nextdate.Month, 1),
                                     new DateTime(nextdate.Year, nextdate.Month, daysInMonth)));
            }

            return datePeriods;
        }
    }
}
