using System;

namespace Tools
{
    public static class Timer
    {
        public static string GetCurTimeMillis()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        }

        public static string GetCurTimeDays()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}