using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using Desktop.command;
using Desktop.Model;
using Desktop.webconnect;
using Newtonsoft.Json;

namespace Desktop.ViewModels
{
    public class RouteVM:INotifyPropertyChanged
    {
        #region Propety_constrtors

        private List<route> _routes;
        private ObservableCollection<string> _routesName;
        private List<station> _stations;
        private List<train> _trains;
        private  route _route;
        private bool _canUpdate;
        private RouteCommand _command;
        
        public route Route
        {
            get => _route;
            private set => _route = value;
        }

        public List<route> RoutesList
        {
            get => _routes;
            set => _routes = value;
        }

        public ObservableCollection<string> RoutesName
        {
            get => _routesName;
            set => _routesName = value;
        }

        public List<station> Stations
        {
            get => _stations;
            set => _stations = value;
        }

        public List<train> Trains
        {
            get => _trains;
            set => _trains = value;
        }

        public RouteVM()
        {
            Route = new route(this);
            UpdateCommand = new RouteCommand(this);
            GetRouteList();
        }

        #endregion

        #region Methods

        #region internel

        public void LoadTableContent(int index)
        {
            Route = RoutesList[index];
            OnPropertyChanged("Route");
            GetStation(_route.RID);
            GetTrain(_route.RID);
        }

        public void Reset()
        {
            Route = new route();
            OnPropertyChanged("Route");
        }

        #endregion

        #region To server

        public bool AddRoute()
        {
            _route.RID = 0;
            var response = Webconnect.PostData("Route/CreateRoute", _route);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                RoutesName.Add(_route.Name);
                RoutesList.Add(_route);
                return true;
            }

            return false;
        }

        public bool Update(int index)
        {
            var id = RoutesList[index].RID;
            var response = Webconnect.PostData("Route/CreateRoute", _route);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                RoutesList[index++] = _route;
                RoutesName[index] = _route.Name;
                OnPropertyChanged("Route");
                return true;
            }

            return false;
        }

        public void GetRouteList()
        {
            var tempData = Webconnect.GetData("Route/GetRouteList");
            //null fields in table make exceptions
            var results = JsonConvert.DeserializeObject<IEnumerable<route>>(tempData);
            _routes = new List<route>();
            _routesName = new ObservableCollection<string>();
            _routesName.Add("Select Route");
            foreach (var result in results)
            {
                var temp = new route
                {
                    RID = result.RID,
                    Name = result.Name,
                    Distance = result.Distance,
                    Sstation = result.Sstation,
                    Estation = result.Estation,
                    Description = result.Description
                };
                RoutesList.Add(temp);
                RoutesName.Add(result.Name);
            }

        }

        public void GetStation(int id = 0)
        {
            var tempData = Webconnect.GetData("Stations/GetStationInRoute/" + id);
            var results = JsonConvert.DeserializeObject<IEnumerable<station>>(tempData);
            _stations = new List<station>();
            foreach (var result in results)
            {
                var temp = new station
                {
                    SID = result.SID,
                    Name = result.Name,
                    Distance = result.Distance,
                    Llongitude = result.Llongitude,
                    Address = result.Address,
                    Telephone = result.Telephone,
                };
                Stations.Add(temp);
            }
        }

        private void GetTrain(int id)
        {
            var tempData = Webconnect.GetData("Train/GetTrainInRoute/" + id);
            var results = JsonConvert.DeserializeObject<IEnumerable<train>>(tempData);
            _trains = new List<train>();
            foreach (var result in results)
            {
                var temp = new train
                {
                    TID = result.TID,
                    Name = result.Name,
                    Sstation = result.Sstation,
                    Estation = result.Estation,
                    Description = result.Description
                };
                Trains.Add(temp);
            }
        }

        //not use due to foreign key delete exception
        public bool DeleteRoute(int id)
        {
            var response = Webconnect.DeleteData("Route/DeleteRoute/" + RoutesList[id].RID);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                RoutesName.Remove(_route.Name);
                RoutesList.Remove(_route);
                Reset();
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region Icommand

        public ICommand UpdateCommand
        {
            get;
            private set;
        }


        public bool CanUpdate
        {
            get => Route.IsValid;
            set => _canUpdate = value;
        }

        #endregion
        
        #region Inotify

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
        
    }
}
