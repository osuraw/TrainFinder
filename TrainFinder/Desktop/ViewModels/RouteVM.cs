using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Desktop.Model;

namespace Desktop.ViewModels
{
    public class RouteVm : BaseViewModelMain
    {
        private byte _routeSelectIndex = 0;

        #region Propety

        public ObservableCollection<Route> RoutesList { get; set; }
        public ObservableCollection<Station> Stations { get; set; }
        public ObservableCollection<Train> Trains { get; set; }

        //***********************************************************************

        public short RouteId
        {
            get { return GetValue(() => RouteId); }
            set { SetValue(() => RouteId, value); }
        }

        [Required(ErrorMessage = "Name Must Not Empty")]
        [StingOnlyValidation]
        public string Name
        {
            get
            {
                return GetValue(() => Name);
            }
            set
            {
                SetValue(() => Name, value);
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Distance Must Not Empty")]
        [NumberOnlyValidation]
        public string Distance
        {
            get { return GetValue(() => Distance); }
            set
            {
                SetValue(() => Distance, value);
            }

        }

        public string Description { get; set; }

        //**********************************************************************

        public byte RouteSelectIndex
        {
            get => _routeSelectIndex;
            set
            {
                _routeSelectIndex = value;
                OnPropertyChanged(nameof(RouteSelectIndex));
            }
        }

        public static int Errors { get; set; }

        #endregion

        #region Icommand

        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public bool CanUpdate { get; set; }

        #endregion

        public RouteVm()
        {
            LoadData();
            AddCommand = new CommandBase(action:AddRoute,canExecute: CheckValid);
            UpdateCommand = new CommandBase(action:Update, canExecute:CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
        }

        #region Methods

        public async Task LoadTableContent(int index)
        {
            UpdateViewsProperties(index);
            try
            {
                Stations = await Station.GetStationByRouteId(RouteId);
                Stations.RemoveAt(0);
                OnPropertyChanged(nameof(Stations));
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                Trains = await Train.GetTrainByRouteId(RouteId);
                Trains.RemoveAt(0);
                foreach (var train in Trains)
                {
                    train.StartStation = Stations.First(s => s.SID.ToString() == train.StartStation).Name;
                    train.EndStation = Stations.First(s => s.SID.ToString() == train.EndStation).Name;
                }
                OnPropertyChanged(nameof(Trains));
            }
            catch (Exception )
            {
                return;
            }
        }

        private async void LoadData()
        {
            RoutesList = await Route.GetRouteList();
            OnPropertyChanged(nameof(RoutesList));
        }
        
        private void UpdateViewsProperties(int index)
        {
            ClearValidation();
            if(RoutesList==null)
                return;
            var data = RoutesList[index];
            RouteId = data.RID;
            Name = index != 0 ? data.Name : "";
            Description = index != 0 ? data.Description:"";
            Distance = index != 0 ? data.Distance.ToString("##.##") : "";
            OnPropertyChanged(nameof(Description));
            Stations?.Clear();
            Trains?.Clear();
        }

        private Route GetViewData()
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

        private bool CheckValid()
        {
            if (Errors == 0 && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Distance))
                return true;
            return false;
        }

        #endregion

        #region CommandActions

        public void AddRoute()
        {
            var route = GetViewData();
            if (route.RID != 0)
            {
                DialogDisplayHelper.DisplayMessageBox("Already Exist","Informative");
                return;
            }
            var response = WebConnect.PostData("Route/CreateRoute", route);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var ree = response.Content.ReadAsStringAsync();
                route.RID = short.Parse(ree.Result);
                RoutesList.Add(route);
                DialogDisplayHelper.DisplayMessageBox("Action Completed", "Informative");
                UpdateViewsProperties(0);
                return ;
            }
            DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
            return ;
        }

        public void Update()
        {
            var route = GetViewData();
            if (route.RID != 0)
            {
                var response = WebConnect.UpdateDate("Route/UpdateRoute", route);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    DialogDisplayHelper.DisplayMessageBox("Update Completed", "Informative");
                    RoutesList[RouteSelectIndex] = route;
                    OnPropertyChanged("Route");
                    UpdateViewsProperties(0);
                    RouteSelectIndex = 0;
                    return ;
                }
            }
            DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
            return ;
        }

        protected void Reset()
        {
            Stations?.Clear();
            Trains?.Clear();
            UpdateViewsProperties(0);
            RouteSelectIndex = 0;
        }

        #endregion
    }
}

//userbility Error internal selected route combobox

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


