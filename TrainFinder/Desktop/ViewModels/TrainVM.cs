using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Desktop.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desktop.ViewModels
{
    class TrainVm : BaseViewModelMain
    {
        private int _routeSelected;
        private short _routeId;
        private short _startStationId;
        private short _endStationId;
        private bool _stationCheck=false;

        #region DataStore

        public ObservableCollection<Station> Stations { get; set; }
        public ObservableCollection<Station> Stations1 { get; set; }
        public ObservableCollection<Route> Routes { get; set; }
        public ObservableCollection<Train> Trains { get; set; }

        #endregion

        #region Model

        public short TrainId { get; set; }

        [Required(ErrorMessage = "Route Required")]
        public short RouteId
        {
            get => _routeId;
            set
            {
                _routeId = value;
                    GetStation();
            }
        }

        [Compare(nameof(EndStationId), ErrorMessage = "Both Start And End Same")]
        public short StartStationId
        {
            get => _startStationId;
            set
            {
                _startStationId = value;
                CheckStation();
                OnPropertyChanged(nameof(StartStationId));
            }
        }

        [Compare(nameof(StartStationId),ErrorMessage = "Both Start And End Same")]
        public short EndStationId
        {
            get => _endStationId;
            set
            {
                _endStationId = value;
                CheckStation();
                OnPropertyChanged(nameof(EndStationId));
            }
        }

        [Required]
        public string Name
        {
            get => GetValue(() => Name);
            set => SetValue(() => Name, value);
        }

        public string Description
        {
            get => GetValue(() => Description);
            set => SetValue(() => Description, value);
        }

        #endregion

        #region Control

        //*****************************************************************************
        public int RouteSelected
        {
            get => _routeSelected;
            set
            {
                _routeSelected = value;
                if (RouteSelected != 0)
                    GetTrains();
            }
        }

        public bool SelectStationVisibility { get; set; }

        public static int Errors { get; set; }

        #endregion

        public TrainVm()
        {
            LoadData();
            AddCommand = new CommandBase(action: AddTrain, canExecute: CheckValid);
            UpdateCommand = new CommandBase(action: Update, canExecute: CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
        }

        #region Methods

        private async void LoadData()
        {
            Routes = await Route.GetRouteList();
            OnPropertyChanged(nameof(Routes));
        }

        private async void GetTrains()
        {
            Trains?.Clear();
            if (RouteSelected != 0)
            {
                try
                {
                    Trains = await Train.GetTrainByRouteId(Routes[RouteSelected].RID);
                    Stations1 = await Station.GetStationByRouteId(Routes[RouteSelected].RID);
                }
                catch (Exception)
                {
                    DialogDisplayHelper.DisplayMessageBox("Non Of Trains Registered With This Route", "Informative");
                    RouteSelected = 0;
                    OnPropertyChanged(nameof(RouteSelected));
                    return;
                }

                Trains.RemoveAt(0);
                foreach (var train in Trains)
                {
                    train.StartStation = Stations1.First(s => s.SID.ToString() == train.StartStation).Name;
                    train.EndStation = Stations1.First(s => s.SID.ToString() == train.EndStation).Name;
                }

                OnPropertyChanged(nameof(Trains));
            }
        }

        private async void GetStation()
        {
            Stations?.Clear();
            SelectStationVisibility = true;
            StartStationId = EndStationId = 0;
            OnPropertyChanged(nameof(SelectStationVisibility));
            if (RouteId!=0)
            {
                try
                {
                    Stations = await Station.GetStationByRouteId(Routes[RouteId].RID);
                }
                catch (HttpRequestException)
                {
                    DialogDisplayHelper.DisplayMessageBox("No Station Found With this Route", "Informative");
                    return;
                }
                OnPropertyChanged(nameof(Stations));
            }
        }

        private void UpdateViewsProperties()
        {
            ClearValidation();
            RouteId = 0;
            TrainId = 0;
            StartStationId = EndStationId = 0;
            OnPropertyChanged(nameof(RouteId));
            Name = Description = "";
        }

        private Train GetViewData()
        {
            if (CheckValid())
                return new Train()
                {
                    TID = TrainId,
                    RID = RouteId,
                    Name = Name,
                    StartStation = StartStationId.ToString(),
                    EndStation = EndStationId.ToString(),
                    Description = Description
                };
            return null;
        }

        private void CheckStation()
        {
            if(EndStationId==StartStationId&&StartStationId!=0&&EndStationId!=0)
            {
                DialogDisplayHelper.DisplayMessageBox("Both Start And End Stations Are Same", "Informative",
                    boxIcon: MessageBoxImage.Hand);
                _stationCheck = false;
            }

            _stationCheck = true;

        }


        #endregion

        #region Commands

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DataGridSelectionChangeCommand { get; }

        #endregion

        #region CommandActions

        public void AddTrain()
        {
            var train = GetViewData();
            if (train.TID != 0)
            {
                DialogDisplayHelper.DisplayMessageBox("All Ready Exist", "Informative",boxIcon:MessageBoxImage.Hand);
                return;
            }

            var response = WebConnect.PostData("Trains/AddTrain", train);
            if (response.StatusCode != HttpStatusCode.Created)
                return;

            DialogDisplayHelper.DisplayMessageBox("Action Completed", "Informative");
            if (RouteSelected == train.RID)
            {
                var data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                train.TID = Convert.ToInt16(data["TID"].ToString());
                train.StartStation = Stations.First(s => s.SID == Convert.ToInt16(train.StartStation)).Name;
                train.EndStation = Stations.First(s => s.SID == Convert.ToInt16(train.EndStation)).Name;
                Trains.Add(train);
                OnPropertyChanged(nameof(Trains));
            }

            UpdateViewsProperties();
        }

        public void Update()
        {
            var train = GetViewData();
            if (train.TID == 0)
                return;
            var response = WebConnect.UpdateDate("Trains/" + train.TID, train);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                DialogDisplayHelper.DisplayMessageBox("Update Completed", "Informative");
                if (RouteSelected != train.RID)
                    return;
                var index = Trains.First(t => t.TID == train.TID);
                index.Name = train.Name;
                index.EndStation = Stations.First(s => s.SID == Convert.ToInt16(train.EndStation)).Name;
                index.StartStation = Stations.First(s => s.SID == Convert.ToInt16(train.StartStation)).Name;
                index.Description = train.Description;
                ObservableCollection<Train> temp = new ObservableCollection<Train>(Trains);
                Trains.Clear();
                Trains = temp;
                OnPropertyChanged(nameof(Trains));
            }
        }

        protected void Reset()
        {
            UpdateViewsProperties();
            Trains?.Clear();
            Stations?.Clear();
            RouteSelected = 0;
            OnPropertyChanged(nameof(RouteSelected));
            SelectStationVisibility = false;
            OnPropertyChanged(nameof(SelectStationVisibility));
        }

        protected bool CheckValid()
        {
            if (Errors == 0 && !string.IsNullOrWhiteSpace(Name) && _stationCheck &&RouteId != 0)
                return true;
            return false;
        }

        private void OnDataGridSelectionChange(object station)
        {
           // var dd = new Dispatcher
            if (station != null)
            {
                var data = (Train) station;
                RouteId = data.RID;
                TrainId = data.TID;
                Name = data.Name;

                StartStationId = Stations1.First(s => s.Name == data.StartStation).SID;
                EndStationId = Stations1.First(s => s.Name == data.EndStation).SID;

                Description = data.Description;
                return;
            }

            UpdateViewsProperties();
        }

        #endregion
    }
}