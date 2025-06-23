using System;

namespace UISample.Utility
{
    public static class TimeExtension
    {
        public const int SecondsInHour = 3600;
        public const int HoursInrDay = 24;
        public const int DaysInWeek = 7;
        
        public static string ToHHMMSS(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}