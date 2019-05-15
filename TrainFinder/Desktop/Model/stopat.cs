using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Desktop.Model
{
    public class Stopat
    {
        public short TID { get; set; }
        public short SID { get; set; }
        public bool Direction { get; set; }
        public float Atime { get; set; }
        public float Dtime { get; set; }

        #region static methods

        public static List<Stopat> GetTimeTableBySidOrTid(byte flag, int id)
        {
            var tempData = WebConnect.GetData($"TimeTables/GetTimeTableByTypeId?type={flag}&id={id}");
            var results = JsonConvert.DeserializeObject<IEnumerable<Stopat>>(tempData);

            var stopats = new List<Stopat>();
            foreach (var result in results)
            {
                var temp = new Stopat
                {
                    TID = result.TID,
                    SID = result.SID,
                    Direction = result.Direction,
                    Atime = result.Atime,
                    Dtime = result.Dtime
                };
                stopats.Add(temp);
            }

            return stopats;
        }

        #endregion

        public static bool AddRecordToTimeTable(Stopat[] data)
        {
           HttpResponseMessage response = WebConnect.PostData("TimeTables", data);
           if (response.StatusCode==HttpStatusCode.Created)
           {
               return true;
           }

           return false;
        }

        public static bool UpdateRecords(short? sid, short? tid, Stopat[] data)
        {
            HttpResponseMessage response = WebConnect.UpdateDate($"TimeTables/{sid}/{tid}", data);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }
    }
}