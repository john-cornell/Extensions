using System;
using System.Collections.Generic;


public static class DateTimeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inclusiveOfBounds">If true, will also return true if date is equal to min or max</param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime me, DateTime first, DateTime second, bool inclusiveOfBounds)
    {
        return inclusiveOfBounds ?
            me.Ticks >= Math.Min(first.Ticks, second.Ticks) &&
                me.Ticks <= Math.Max(first.Ticks, second.Ticks)
            : me.Ticks > Math.Min(first.Ticks, second.Ticks) &&
                me.Ticks < Math.Max(first.Ticks, second.Ticks);
    }

    public static IEnumerable<DateTime> EnumerateTo(this DateTime me, DateTime to)
    {
        DateTime from = me;

        if (from > to)
        {
            DateTime temp = from;
            from = to;
            to = temp;
        }

        List<DateTime> dates = new List<DateTime>();
        DateTime cursor = from;

        while (cursor <= to)
        {
            yield return cursor;

            cursor = cursor.AddDays(1);
        }
    }

    public static DateTime GetMonthStart(this DateTime me)
    {
        return new DateTime(me.Year, me.Month, 1);
    }

    public static DateTime StartOfWeek(this DateTime me, DayOfWeek startOfWeek)
    {
        int diff = me.DayOfWeek - startOfWeek;
        if (diff < 0)
        {
            diff += 7;
        }

        return me.AddDays(-1 * diff).Date;
    }

    public static DateTime NextDayOfWeek(this DateTime me, DayOfWeek dayOfWeek)
    {
        int diff = dayOfWeek - me.DayOfWeek;
        if (diff <= 0)
        {
            diff += 7;
        }

        return me.AddDays(diff).Date;
    }
    public enum PrintableDateTimeOptions
    {
        HideTodaysDate = 1 << 0,
        ConvertToLocal = 1 << 1

    }

    public static string GetPrintableDateTime(this DateTime me, PrintableDateTimeOptions options)
    {
        DateTime working = (options & PrintableDateTimeOptions.ConvertToLocal) > 0 ? me.ToLocalTime() : me;

        if ((options & PrintableDateTimeOptions.HideTodaysDate) == 0 || working.Date != DateTime.Now.Date)
        {
            return $"{working.ToString("MMM dd")} {working.ToString("HH:mm")}";
        }
        else
        {
            return $"{working.ToString("HH:mm")}";
        }
    }

    private static TimeSpan GetUnixEpochTimeSpan(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Unspecified) throw new InvalidTimeZoneException("The Date Time has a kind set to Unspecified.  This needs to be set to Local time or Utc Time.");
        return dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
    }

    public static double GetUnixEpoch(this DateTime dateTime)
    {
        var unixTime = GetUnixEpochTimeSpan(dateTime);

        return unixTime.TotalSeconds;
    }

    public static double GetMinuteBasedUnixEpoch(this DateTime dateTime)
    {
        var unixTime = GetUnixEpochTimeSpan(dateTime);

        return unixTime.TotalMinutes;
    }

    public static DateTime StripSeconds(this DateTime me)
    {
        return me.AddSeconds(-me.Second).AddMilliseconds(-me.Millisecond);
    }

    
}

