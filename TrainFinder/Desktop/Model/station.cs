
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desktop.Model
{
    using System.Collections.Generic;

    public  class Station 
    {
        //private Location _location;

        public short SID { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public short RID { get; set; }
        public string Location { get; set; }

        public static ObservableCollection<Station> Stations { get; set; }

        #region static methods

        public static async Task<ObservableCollection<Station>> GetStationByRouteId(int id=0)
        {
            await GetStationBy(id);
            return Stations;
        }

        private static async Task GetStationBy(int id)
        {
            var tempData =await WebConnect.GetData("Stations/GetStationInRoute/" + id);
            var results = JsonConvert.DeserializeObject<IEnumerable<Station>>(tempData);
            var stations = new ObservableCollection<Station> { new Station() { Name = "Select Station" } };
            foreach (var result in results)
            {
                var temp = new Station
                {
                    SID = result.SID,
                    Name = result.Name,
                    Distance = result.Distance,
                    Location = result.Location,
                    Address = result.Address,
                    Telephone = result.Telephone,
                    RID = result.RID
                };
                stations.Add(temp);
            }
            //could make error
           Stations = stations;
        }

        #endregion

    }
}
