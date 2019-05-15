using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Newtonsoft.Json;

namespace Desktop.Model
{
    class Pinlocation
    {
        public short LID { get; set; }
        public bool Type { get; set; }
        public Point Location{ get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public short Rid { get; set; }


        public static ObservableCollection<Pinlocation> GetLocationList(short routeId)
        {
            var tempData = WebConnect.GetData("PinLocation/GetLocations/" + routeId);
            var results = JsonConvert.DeserializeObject<IEnumerable<Pinlocation>>(tempData);
            var locationList = new ObservableCollection<Pinlocation>();
            foreach (var result in results)
            {
                var temp = new Pinlocation()
                {
                    LID = result.LID,
                    Rid = result.Rid,
                    Message = result.Message,
                    Description = result.Description,
                    Location = result.Location,
                    Type = result.Type
                };
                locationList.Add(temp);
            }
            //could make error
            return locationList;
        }
    }
}
