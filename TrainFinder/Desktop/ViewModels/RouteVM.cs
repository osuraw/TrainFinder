using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows.Input;
using Desktop.Model;

namespace Desktop.ViewModels
{
    public class RouteVM : BaseViewModelMain
    {
        #region Propety

        public ObservableCollection<Route> RoutesList { get; set; }
        public ObservableCollection<Station> Stations { get; set; }
        public ObservableCollection<Train> Trains { get; set; }

        //***********************************************************************
        
        public short RouteId
        {
            get { return GetValue(() => RouteId);}
            set { SetValue(()=>RouteId,value);}
        }

        [Required(ErrorMessage = "Name Must Not Empty")]
        [StingOnlyValidation]
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Distance Must Not Empty")]
        [NumberOnlyValidation]
        public string Distance
        {
            get { return GetValue(() => Distance); }
            set { SetValue(() => Distance, value); }

        }

        public string Description { get; set; }

        //**********************************************************************

        public byte RouteSelectIndex { get; set; } = 0;
        public bool Validation { get; set; } = true;
        public static int Errors { get; set; }
        #endregion

        #region Icommand

        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public bool CanUpdate { get; set; }

        #endregion
        
        #region Methods

        public RouteVM()
        {
            RoutesList = Route.GetRouteList();
            AddCommand = new CommandBase(AddRoute,CheckValid);
            UpdateCommand = new CommandBase(Update, CheckValid);
            ResetCommand = new CommandBase(action:Reset,canExecute:AlwaysTrue);
        }

        public void LoadTableContent(int index)
        {
            UpdateViewsProperties(index);
            Stations = Station.GetStationByRouteId(RouteId);
            Stations.RemoveAt(0);
            Trains = Train.GetTrainByRouteId(RouteId);
            Trains.RemoveAt(0);
            OnPropertyChanged("Route");
        }

        public bool AddRoute()
        {
            var route = GetViewData();
            if (route == null)
            {
                Validation = false;
                return false;
            }

            route.RID = 0;
            var response = WebConnect.PostData("Route/CreateRoute", route);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var ree = response.Content.ReadAsStringAsync();
                route.RID = Int16.Parse(ree.Result);
                RoutesList.Add(route);
                UpdateViewsProperties(0);
                return true;
            }

            return false;
        }

        public bool Update(int index)
        {
            var route = GetViewData();
            if(route.RID!=0)
            {
                var response = WebConnect.PostData("Route/CreateRoute", route);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    RoutesList[index] = route;
                    OnPropertyChanged("Route");
                    return true;
                }
            }
            return false;
        }
        
        private void UpdateViewsProperties(int index)
        {
            var data = RoutesList[index];
            RouteId = data.RID;
            Name = index != 0 ? data.Name : "";
            Description = data.Description;
            Distance = index != 0 ? data.Distance.ToString("##.##"): "";
        }

        private    Route GetViewData()
        {
            if (CheckValid())
                return new Route()
                {
                    RID = RouteId,
                    Name = Name,
                    Distance = float.Parse(Distance),
                    Description = Description
                };
            return null;
        }

        protected override bool CheckValid()
        {
            if (Errors == 0)
                return true;
            else
                return false;
        }
        
        protected override void Reset()
        {
            UpdateViewsProperties(0);
            RouteSelectIndex = 0;
        }
       
        #endregion
    }
}

//not use due to foreign key delete exception
//public bool DeleteRoute(int id)
//{
//    var Route = GetViewData();
//    var response = WebConnect.DeleteData("Route/DeleteRoute/" + RoutesList[id].RID);
//    if (response.StatusCode == HttpStatusCode.OK)
//    {
//        RoutesList.Remove(Route);
//        Reset();
//        return true;
//    }

//    return false;
//}