using System;

namespace Toolbox
{
    public static class Time
    {
        public static int ConvertToUnixTimestamp(DateTime dateTime)
        {
            return (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }
    }

}

