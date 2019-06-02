using System.Data.Entity;
using System.Linq;
using pro_web_a.Models;

namespace pro_web_a.Controllers
{
    public static class Helpers
    {
        private static readonly ProjectDB _context = new ProjectDB();

        /// <summary>
        /// Find next station in route
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="direction">true : up-word false : down-word </param>
        /// <param name="currentStation"></param>
        /// <returns></returns>
        public static int FindNextStation(short trainId, bool direction, int currentStation = 0)
        {
            
            var data = _context.StopAts.Where(s => s.TID == trainId && s.Direction == direction).Include(s => s.station)
                .OrderBy(s => s.station.Distance).Select(s => s.station).ToList();
            if (!direction)
            {
                data.Reverse();
            }
            if (currentStation == -1)
            {
                return data[1].SID;
            }

            var nextIndex = data.FindIndex(d => d.SID == currentStation) + 1;
            return data.Count >= nextIndex ? data[nextIndex].SID : -1;
        }
    }
}