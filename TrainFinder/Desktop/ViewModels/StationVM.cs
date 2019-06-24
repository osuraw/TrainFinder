using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using Desktop.CustomAttributes;
using Desktop.Helpers;
using Desktop.Model;

namespace Desktop.ViewModels
{
    class StationVm : BaseViewModelMain
    {
        private short _routeIdSelectCmb1 = 0;

       
        #region DataContaners

        public ObservableCollection<Station> Stations { get; set; }

        public ObservableCollection<Route> RoutesList { get; set; }

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
        [PhoneNumber]
        public string Telephone
        {
            get { return GetValue(() => Telephone); }
            set { SetValue(() => Telephone, value); }
        }

        [Required]
        public string Locations
        {
            get { return GetValue(() => Locations); }
            set { SetValue(() => Locations, value); }
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
        public ICommand PrintCommand { get; private set; }

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
        
        public StationVm()
        {
            LoadData();
            AddCommand = new CommandBase(action:AddStation,canExecute: CheckValid);
            UpdateCommand = new CommandBase(action:Update, canExecute:CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            PrintCommand = new CommandBase(action: Print.PrintDocument, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
            LocationCommand = new CommandBase(action: SetLocation, canExecute: AlwaysTrue);
        }
        
        #region Action

        public void AddStation()
        {
            var station = GetViewData();
            if (station.SID !=0 )
            {
                DialogDisplayHelper.DisplayMessageBox("All Ready Exist", "Informative");
            }
            else
            {
                var response = WebConnect.PostData("Stations/AddStation", station);
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
                }
                else
                {
                    DialogDisplayHelper.DisplayMessageBox("Action Completed", "Informative");
                    if (station.RID == RouteIdSelectCmb1)
                    {
                        Stations.Add(station);
                        OnPropertyChanged(nameof(Stations));
                    }
                    var ree = response.Content.ReadAsStringAsync();
                    station.SID = short.Parse(ree.Result);
                    StationId = short.Parse(ree.Result);
                }
                UpdateViewsProperties();
            }
        }

        public void Update()
        {
            var station = GetViewData();
            var response = WebConnect.UpdateDate("Stations/" + station.SID, station);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                DialogDisplayHelper.DisplayMessageBox("Update Completed", "Informative");
                var index = Stations.First(s => s.SID == station.SID);
                index.Name = station.Name;
                index.Location = station.Location;
                index.Address = station.Address;
                index.Distance = station.Distance;
                index.Telephone = station.Telephone;
                ObservableCollection<Station> temp = new ObservableCollection<Station>(Stations);
                Stations.Clear();
                Stations = temp;
                OnPropertyChanged(nameof(Stations));
            }
            else
                DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
            UpdateViewsProperties();
        }

        private void OnDataGridSelectionChange(object station)
        {
            if (station != null)
            {
                Station data = (Station)station;
                StationId = data.SID;
                RouteId = data.RID;
                Name = data.Name;
                Distance = data.Distance.ToString("##.##");
                Telephone = data.Telephone;
                Address = data.Address;
                Locations = data.Location;
                OnPropertyChanged(nameof(Locations));
                return;
            }

            UpdateViewsProperties();
        }

        private void SetLocation(object location)
        {
            Locations = location?.ToString();
            OnPropertyChanged(nameof(Locations));
        }

        private void Reset()
        {
            UpdateViewsProperties();
            Stations?.Clear();
            RouteIdSelectCmb1 = 0;
            OnPropertyChanged(nameof(RouteIdSelectCmb1));
        }

        private bool CheckValid()
        {
            if (Errors == 0 && RouteId != 0 && !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Locations)
                && !string.IsNullOrWhiteSpace(Distance)
                && !string.IsNullOrWhiteSpace(Telephone))
                return true;
            return false;
        }

        #endregion

        #region PrivateHelpers

        private Station GetViewData()
        {
            if (CheckValid())
                return new Station
                {
                    RID = RouteId,
                    Name = Name,
                    Distance = float.Parse(Distance),
                    Address = Address,
                    Telephone = Telephone,
                    SID = StationId,
                    Location = Locations
                };
            return null;
        }

        private async void OnRouteIdSelectCmb1Change()
        {
            if (RouteIdSelectCmb1 != 0)
            {
                Stations?.Clear();
                try
                {
                    Stations = await Station.GetStationByRouteId(RoutesList[RouteIdSelectCmb1].RID);
                }
                catch (HttpRequestException)
                {
                    DialogDisplayHelper.DisplayMessageBox("No Station Found With this Route","Informative");
                    return;
                }
                Stations.RemoveAt(0);
                OnPropertyChanged(nameof(Stations));
            }
        }

        private void UpdateViewsProperties()
        {
            ClearValidation();
            RouteId = 0;
            StationId=RouteId = 0;
            Name = Distance=Description = Telephone = Address = Locations = "";
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Description));
        }

        private async void LoadData()
        {
            try
            {
                RoutesList = await Route.GetRouteList();
                OnPropertyChanged(nameof(RoutesList));
            }
            catch (HttpRequestException)
            {
                DialogDisplayHelper.DisplayMessageBox("No Route Found", "Informative");
                return;
            }
            
        }

        #endregion
    }
}