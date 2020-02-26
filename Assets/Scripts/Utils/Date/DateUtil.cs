using System.IO;
using System;
using UnityEngine;


namespace Gululu.Util{
    
    public class DateUtil
    {
        public static double ConvertToUnixOfTime(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static long GetTimeStamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now - origin;
            return Convert.ToInt64(diff.TotalSeconds);
        }
        public static int ConvertToDay(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (int)Math.Floor(diff.TotalDays);
        }

        public static string formatBirthday(string birthday)
        {
            DateTime dateTime = Convert.ToDateTime(birthday);
            return dateTime.ToString("yyyy-MM-dd");
        }

    }
}

