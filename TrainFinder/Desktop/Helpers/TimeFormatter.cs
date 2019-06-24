using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Helpers
{
    public static class TimeFormatter
    {
        public static int ToTime(this String str)
        {
            var data = DateTime.Parse(str).ToShortTimeString();
            var time = TimeSpan.Parse(data.Split(' ')[0]);
            if (str.Contains("PM"))
                time = time.Add(new TimeSpan(12,0,0));
            return Convert.ToInt32(time.TotalSeconds);
        }

        public static string TimeToString(this string timeInSeconds)
        {
            var time = " AM";
            var time1 = TimeSpan.FromSeconds(Convert.ToDouble(timeInSeconds));
            if (time1.Hours > 12)
            {
                time1=time1.Subtract(new TimeSpan(12, 0, 0));
                time = " PM";
            }
            StringBuilder stringBuilder = new StringBuilder(time1.ToString("g"));
            return stringBuilder.Append(time).ToString();
        }

        public static string RemovedDate(this string dateTime)
        {
            return DateTime.Parse(dateTime).ToShortTimeString();
        }
    }
}
