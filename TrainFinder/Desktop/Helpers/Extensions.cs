using System;
using Microsoft.Maps.MapControl.WPF;

namespace Desktop.Helpers
{
    public static class  Extensions
    {
        public static Location ToLocation(this object str)
        {
            if (str==null||"".Equals(str.ToString()))
                return null;
            var location = str.ToString().Split(':');
            return new Location(Convert.ToDouble(location[0]), Convert.ToDouble(location[1]));
        }

        public static string LocationToString(this Location location)
        {
            return location.Latitude.ToString("###.####") + ":" + location.Longitude.ToString("###.####");
        }
    }
}
