using System;

namespace Reega.Pola
{
    public class Utils
    {
        public static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime dtDateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddMilliseconds(unixtime);
        }
    }
}
