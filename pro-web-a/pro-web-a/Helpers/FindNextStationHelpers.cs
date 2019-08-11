using System.Data.Entity;
using System.Linq;
using pro_web_a.Models;

namespace pro_web_a.Helpers
{
    public static class FindNextStationHelpers
    {
        private static readonly ProjectDB Context = new ProjectDB();

        public static int FindNextStation(short trainId, bool direction, int currentStation = 0)
        {
            var data = Context.StopAts.Where(s => s.TID == trainId && s.Direction == direction).Include(s => s.station)
                .OrderBy(s => s.station.Distance).Select(s => s.station).ToList();
            if (data.Count == 0)
                return -5;
            if (!direction)
            {
                data.Reverse();
            }
            if (currentStation == -1)
            {
                return data[1].SID;
            }

            var nextIndex = data.FindIndex(d => d.SID == currentStation) + 1;
            return data.Count > nextIndex ? data[nextIndex].SID : -1;
        }
    }
}