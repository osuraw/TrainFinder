using System.ComponentModel;
using System.Diagnostics;
using Desktop.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Desktop.Model
{
    public class Route 
    {
        #region Entity

        public Route()
        {
        }

        public short RID { get; set; }
        public int Sstation { get; set; }
        public int Estation { get; set; }
        public float Distance { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        #endregion
        
        #region static methods 
        public static ObservableCollection<Route> GetRouteList()
        {
            var tempData = WebConnect.GetData("Route/GetRouteList");
            var results = JsonConvert.DeserializeObject<IEnumerable<Route>>(tempData);
            //---------------------------------------------------------------------
            var routes = new ObservableCollection<Route>() {new Route {RID = 0,Name = "Select Route"}};
            foreach (var result in results)
            {
                var temp = new Route
                {
                    RID = result.RID,
                    Name = result.Name,
                    Distance = result.Distance,
                    Sstation = result.Sstation,
                    Estation = result.Estation,
                    Description = result.Description
                };
                routes.Add(temp);
            }

            return routes;
        }
        #endregion
    }
}