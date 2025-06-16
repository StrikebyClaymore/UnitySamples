using System;

namespace UISample.Utility
{
    public static class TimeExtension
    {
        public static string ToHHMMSS(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}