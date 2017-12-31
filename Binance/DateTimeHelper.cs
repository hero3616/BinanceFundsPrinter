using System;

namespace Binance
{
    public class DateTimeHelper
    {
        public static DateTime UnixTimeToDateTime(double unixtime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }
    }
}
