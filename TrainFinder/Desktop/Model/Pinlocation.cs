using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Desktop.Helpers;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;

namespace Desktop.Model
{
    class PinLocation
    {
        public short PinId { get; set; }
        public byte Type { get; set; }
        public string Location{ get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public short RouteId { get; set; }

        public static ObservableCollection<PinLocation> PinLocations { get; set; }

        public static async Task<ObservableCollection<PinLocation>> GetPinLocationList(short routeId)
        {
            await GetLocations(routeId);
            return PinLocations;
        }

        private static async Task GetLocations(short routeId)
        {
            var tempData = await WebConnect.GetData("PinLocation/GetPinLocation?rid=" + routeId);
            var results = JsonConvert.DeserializeObject<IEnumerable<PinLocation>>(tempData);
            var locationList = new ObservableCollection<PinLocation>();
            foreach (var result in results)
            {
                locationList.Add(result);
            }
            PinLocations = locationList;
        }

        public static ObservableCollection<Location> GetLocationList()
        {
            var list = new ObservableCollection<Location>();
            foreach (PinLocation location in PinLocations)
            {
                list.Add(location.Location.ToLocation());
            }
            return list;
        }
    }
}
