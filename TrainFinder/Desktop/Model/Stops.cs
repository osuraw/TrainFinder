using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop.Helpers;
using Newtonsoft.Json;

namespace Desktop.Model
{
    public class Stops
    {
        public short TrainId { get; set; }

        public short StationId { get; set; }
        public string TrainName { get; set; }
        public string StationName { get; set; }
        public bool Direction { get; set; }
        public string ArriveTime { get; set; }
        public string DepartureTime { get; set; }

        public static ObservableCollection<Stops> StopList { get; set; }

        #region static methods

        public static async Task<ObservableCollection<Stops>> GetTimeTableBySidOrTid(byte flag, int id)
        {
            await GetTimeTable(flag, id);
            return StopList;
        }

        private static async Task GetTimeTable(byte flag, int id)
        {
            var tempData = await WebConnect.GetData($"TimeTables/GetTimeTableByTypeId?type={flag}&id={id}");
            var results = JsonConvert.DeserializeObject<IEnumerable<Stops>>(tempData);

            StopList = new ObservableCollection<Stops>();
            foreach (var result in results)
            {
                var temp = new Stops
                {
                    TrainId = result.TrainId,
                    StationId = result.StationId,
                    TrainName = result.TrainName,
                    StationName = result.StationName,
                    Direction = result.Direction,
                    ArriveTime = result.ArriveTime.TimeToString(),
                    DepartureTime = result.DepartureTime.TimeToString()
                };
                StopList.Add(temp);
            }
        }

        #endregion
    }

}