using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Windows.Input;
using Desktop.Model;

namespace Desktop.ViewModels
{
    class TrainVM : BaseViewModelMain
    {
        private int _routeSelected = 0;
        private short _routeId;

        #region Properties

        #region DataStore

        public ObservableCollection<Station> Stations { get; set; }
        public ObservableCollection<Route> Routes { get; set; }
        public ObservableCollection<Train> Trains { get; set; }

        #endregion

        #region Model

        public short TrainId { get; set; }

        public short RouteId
        {
            get => _routeId;
            set
            {
                _routeId = value;
                GetStation();
            }
        }

        [Required(ErrorMessage = "Start Station Required")]
        public short StartStationId { get; set; }

        [Required(ErrorMessage = "End Station Required")]
        [Compare("StartStationId",ErrorMessage ="Both Start And End Same")]
        public short EndStationId { get; set; }

        [Required]
        [StingOnlyValidation]
        public string Name {
            get => GetValue(() => Name);
            set => SetValue(() => Name, value);
        }

        public string Description { get; set; }

        #endregion

        #region Control

        //*****************************************************************************
        public int RouteSelected
        {
            get => _routeSelected;
            set
            {
                _routeSelected = value;
                if (_routeSelected < 0)
                    _routeSelected = 0;
                GetTrains();
            }
        }

        public bool SelectStationVisibility { get; set; } = false;

        public static int Errors { get; set; }

        #endregion

        #region Commands

        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand DataGridSelectionChangeCommand { get; private set; }
        public ICommand LocationCommand { get; private set; }

        #endregion

        #endregion

        public TrainVM()
        {
            Routes = Route.GetRouteList();
            AddCommand = new CommandBase(AddTrain, CheckValid);
            UpdateCommand = new CommandBase(Update, CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
        }

        #region Privet

        private void GetTrains()
        {
            Trains?.Clear();
            SelectStationVisibility = false;
            if (RouteSelected != 0)
            {
                Trains = Train.GetTrainByRouteId(Routes[RouteSelected].RID);
                Trains.RemoveAt(0);
                SelectStationVisibility = true;
            }
        }
        private void GetStation()
        {
            
            //SelectStationVisibility = false;
            if (RouteSelected != 0)
            {
                Stations = Station.GetStationByRouteId(Routes[RouteSelected].RID);
                //SelectStationVisibility = true;
            }
        }

        private void ClearViewProperties()
        {
            RouteSelected = RouteId = StartStationId = EndStationId = 0;
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
                    Sstation = StartStationId,
                    Estation = EndStationId,
                    Description = Description
                };
            return null;
        }

        private void OnDataGridSelectionChange(object station)
        {
            if (station != null)
            {
                Train data = (Train)station;
                RouteId = data.RID;
                TrainId = data.TID;
                Name = data.Name;
                StartStationId = data.Sstation;
                EndStationId = data.Estation;
                Description = data.Description;
                return;
            }
            ClearViewProperties();
        }

        #endregion

        #region Actions

        public bool AddTrain()
        {
            var data = GetViewData();
            data.TID = 0;
            var response = WebConnect.PostData("Trains/AddTrain", data);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public bool Update()
        {
            var data = GetViewData();
            if(data.TID!=0)
            {
                var response = WebConnect.UpdateDate("Trains/" + data.TID, data);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        #endregion

        #region OvrideMethods

        protected override bool CheckValid()
        {
            if (Errors == 0)
                return true;
            else
                return false;
        }

        protected override void Reset()
        {
            ClearViewProperties();
            Trains?.Clear();
        }


        #endregion

    }
}