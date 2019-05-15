
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;

namespace Desktop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Station:INotifyPropertyChanged
    {
        private Location _location;

        public short SID { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public short RID { get; set; }
        public string Description { get; set; }
        public Location Location
        {
            get => _location;
            set
            {
                if (value == null) return;
                _location = value;
                OnPropertyChanged("Locations");
            }
        }
        public string Locations => Location != null ? "Lo: " + $"{Location.Longitude:0.00}" + ", La: " + $"{Location.Latitude:0.00}" : "";


        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        #endregion

        #region static methods

        public static ObservableCollection<Station> GetStationByRouteId(int id = 0)
        {
            var tempData = WebConnect.GetData("Stations/GetStationInRoute/" + id);
            var results = JsonConvert.DeserializeObject<IEnumerable<Station>>(tempData);
            var stations = new ObservableCollection<Station> {new Station() {Name = "Select Station"}};
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
            return stations;
        }

        #endregion

    }
}
