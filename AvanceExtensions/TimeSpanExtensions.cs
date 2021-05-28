using System;

public static class TimeSpanExtensions
{
    public enum Format { Standard, DaysOnlyIfAvailable }

    public static String Verbose(this TimeSpan timeSpan, Format format = Format.Standard)
    {
        int days = (int) timeSpan.TotalDays;
        string dayPlural = days > 1 ? "s" : "";
        
        if (days == 0 || format == Format.Standard)
        {
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;

            
            string hourPlural = hours > 1 ? "s" : "";
            string minPlural = minutes > 1 ? "s" : "";

            if (days > 0) return $"{days} day{dayPlural} {hours} hr{hourPlural} {minutes} min{minPlural}";
            if (hours > 0) return $"{hours} hr{hourPlural} {minutes} min{minPlural}";
            return $"{minutes} min{minPlural}";
        }
        else
        {
            return $"{days} Day{dayPlural}";
        }
    }

    public static TimeSpan RoundUpTo(this TimeSpan timeSpan, int n)
    {
        return TimeSpan.FromMinutes(n * Math.Ceiling(timeSpan.TotalMinutes / n));
    }
}
