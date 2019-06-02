using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows.Input;
using Desktop.Model;

namespace Desktop.ViewModels
{
    class StationVm : BaseViewModelMain
    {
        private short _routeIdSelectCmb1 = 0;

        #region Propperties

        #region DataContaners

        public ObservableCollection<Station> Stations { get; set; }

        public static ObservableCollection<Route> RoutesList { get; set; }

        public Station Station { get; set; }

        #endregion

        #region Model

        [Required]
        [Range(1, 5)]
        public short RouteId
        {
            get { return GetValue(() => RouteId); }
            set { SetValue(() => RouteId, value); }
        }

        public short StationId
        {
            get { return GetValue(() => StationId); }
            set { SetValue(() => StationId, value); }
        }

        [Required]
        [StingOnlyValidation]
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }

        [Required]
        [NumberOnlyValidation]
        public string Distance
        {
            get { return GetValue(() => Distance); }
            set { SetValue(() => Distance, value); }
        }

        [Required]
        [Phone]
        public string Telephone
        {
            get { return GetValue(() => Telephone); }
            set { SetValue(() => Telephone, value); }
        }

        [Required]
        public string Location
        {
            get { return GetValue(() => Location); }
            set { SetValue(() => Location, value); }
        }

        public string Address { get; set; }

        public string Description { get; set; }

        #endregion

        #region Command

        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand DataGridSelectionChangeCommand { get; private set; }
        public ICommand LocationCommand { get; private set; }

        #endregion

        #region Control

        public short RouteIdSelectCmb1
        {
            get => _routeIdSelectCmb1;
            set
            {
                _routeIdSelectCmb1 = value;
                OnRouteIdSelectCmb1Change();
            }
        }

        public static int Errors { get; set; }

        #endregion

        #endregion

        #region Methods

        public StationVm()
        {
            RoutesList = Route.GetRouteList();
            AddCommand = new CommandBase(AddStation, CheckValid);
            UpdateCommand = new CommandBase(Update, CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
            LocationCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
        }

        #region Action

        public bool AddStation()
        {
            var station = GetViewData();
            station.SID = 0;
            var response = WebConnect.PostData("Stations/AddStation", station);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var ree = response.Content.ReadAsStringAsync();
                station.SID = Int16.Parse(ree.Result);
                StationId = Int16.Parse(ree.Result);
                return true;
            }

            return false;
        }

        public bool Update()
        {
            var station = GetViewData();
            var response = WebConnect.UpdateDate("Stations/" + station.SID, station);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                ClearViewProperties();
                return true;
            }

            return false;
        }

        #endregion

        #region OverideBaseVM

        protected bool CheckValid()
        {
            if (Errors == 0)
                return true;
            return false;
        }

        protected void Reset()
        {
            ClearViewProperties();
            if (Station != null)
                Stations.Clear();
        }

        #endregion

        #region PrivateHelpers

        private Station GetViewData()
        {
            if (CheckValid())
                return new Station()
                {
                    RID = RouteId,
                    Name = Name,
                    Distance = float.Parse(Distance),
                    Address = Address,
                    Telephone = Telephone,
                    SID = StationId
                    //Location=Location
                };
            return null;
        }

        private void OnRouteIdSelectCmb1Change()
        {
            Stations?.Clear();
            if (RouteIdSelectCmb1 != 0)
            {
                Stations = Station.GetStationByRouteId(RoutesList[RouteIdSelectCmb1].RID);
                Stations.RemoveAt(0);
                OnPropertyChanged(nameof(Stations));
            }
        }

        private void OnDataGridSelectionChange(object station)
        {
            if (station != null)
            {
                Station data = (Station)station;
                RouteId = data.RID;
                Name = data.Name;
                Distance = data.Distance.ToString("##.##");
                Telephone = data.Telephone;
                Address = data.Address;
                Location = data.Locations;
                Description = data.Description;
                return;
            }

            ClearViewProperties();
        }

        private void ClearViewProperties()
        {
            RouteIdSelectCmb1 = RouteId = 0;
            Name = Distance = Telephone = Address = Location = Description = "";
        }

        #endregion

        #endregion
    }
}


//public void GetStation(int id = 0)
//{
//Stations = Station.GetStationByRouteId(id);
//Stations.RemoveAt(0);
//}