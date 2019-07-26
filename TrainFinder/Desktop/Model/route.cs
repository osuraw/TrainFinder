using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desktop.Model
{
    public class Route 
    {
        #region Entity

        public short RID { get; set; }
        public string Name { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public float Distance { get; set; }
        public string Description { get; set; }
        

        public static ObservableCollection<Route> Routes { get; set; }

        #endregion

        #region static methods 

        public static  async Task<ObservableCollection<Route>> GetRouteList()
        {
            if (Routes==null)
            {
                await GetRouteList1();
            }

            return Routes;
        }

        //private static async Task<ObservableCollection<Route>> GetRouteList1()
        private static async Task GetRouteList1()
        {
            
            //var tempData = WebConnect.GetData("Route/GetRouteList");
            var tempData = await WebConnect.GetData("Route/GetRouteList");
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
                    StartStation = result.StartStation,
                    EndStation = result.EndStation,
                    Description = result.Description
                };
                routes.Add(temp);
            }

            Routes = routes;
            //return routes;
        }
        #endregion
    }
}