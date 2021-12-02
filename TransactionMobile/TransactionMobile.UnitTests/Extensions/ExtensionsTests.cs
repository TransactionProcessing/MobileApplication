using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TransactionMobile.UnitTests.Extensions
{
    using Shouldly;
    using TransactionMobile.Extensions;

    public class ExtensionsTests
    {
        [Theory]
        [InlineData(1,2021,null)]
        [InlineData(2, 2021, null)]
        [InlineData(2, 2024, null)] // Test a leap year
        [InlineData(3, 2021, null)]
        [InlineData(4, 2021, null)]
        [InlineData(5, 2021, null)]
        [InlineData(6, 2021, null)]
        [InlineData(7, 2021, null)]
        [InlineData(8, 2021, null)]
        [InlineData(9, 2021, null)]
        [InlineData(10, 2021, null)]
        [InlineData(11, 2021, null)]
        [InlineData(12, 2021, null)]
        [InlineData(1, 2021, 3)]
        [InlineData(2, 2021, 3)]
        [InlineData(2, 2024, 3)] // Test a leap year
        [InlineData(3, 2021, 3)]
        [InlineData(4, 2021, 3)]
        [InlineData(5, 2021, 3)]
        [InlineData(6, 2021, 3)]
        [InlineData(7, 2021, 3)]
        [InlineData(8, 2021, 3)]
        [InlineData(9, 2021, 3)]
        [InlineData(10, 2021, 3)]
        [InlineData(11, 2021, 3)]
        [InlineData(12, 2021, 3)]
        public void GenerateDatePeriods_GenerateDates_NoHistoricalMonths_DatesReturned(Int32 month, Int32 year, Int32? historicalMonths)
        {
            DateTime currentDate = new DateTime(year, month, 1);

            List<(String displayText, DateTime startDate, DateTime endDate)> generatedDates = null;
            if (historicalMonths.HasValue == false)
            {
                generatedDates = currentDate.GenerateDatePeriods();
            }
            else
            {
                generatedDates = currentDate.GenerateDatePeriods(historicalMonths.Value);
            }

            generatedDates.ShouldNotBeNull();
            generatedDates.ShouldNotBeEmpty();
        }
    }
}
