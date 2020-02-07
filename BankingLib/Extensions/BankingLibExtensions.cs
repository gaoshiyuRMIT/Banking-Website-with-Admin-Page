using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

using BankingLib.Models;

namespace BankingLib.Extensions
{
    public static class StringExtensions
    {
        const string dtFormat = "dd/MM/yyyy hh:mm:ss tt";
        public static DateTime? ToDateTime(this string dts)
        {
            if(string.IsNullOrEmpty(dts.Trim()))
                return null;
            try {
                return DateTime.ParseExact(dts, dtFormat, CultureInfo.InvariantCulture);
            } catch (FormatException) {
                return null;
            }
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime SpecifySecond(this DateTime dt, int newSecond)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour,
                dt.Minute, newSecond);
        }
    }

    public static class BillPayPeriodExtensions
    {
        public static string ToFriendlyString(this BillPayPeriod bp)
        {
            if (bp == BillPayPeriod.OnceOff)
                return "Once Off";
            return Enum.GetName(typeof(BillPayPeriod), bp);
        }

        public static int? ToMonth(this BillPayPeriod bp)
        {
            if (bp == BillPayPeriod.OnceOff)
                return null;
            if (bp == BillPayPeriod.Monthly)
                return 1;
            if (bp == BillPayPeriod.Quarterly)
                return 3;
            return 12;
        }
    }
}
