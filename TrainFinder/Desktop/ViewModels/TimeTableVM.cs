using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using Desktop.Helpers;
using Desktop.Model;

namespace Desktop.ViewModels
{
    class TimeTableVm : BaseViewModelMain
    {
        #region PrivetFields

        private bool _trainTicked;
        private bool _stationTicked;
        private int _routeSelected;
        private int _routeSelected1;
        private int _listDataIndex;
        private bool _checked1;
        private bool _checked2;
        private ObservableCollection<Train> _trains;
        private ObservableCollection<Station> _stations;
        private bool _routeEnable = true;
        private bool _isEnable = true;

        #endregion

        #region DataLists

        public ObservableCollection<dynamic> ListData { get; set; }

        public ObservableCollection<Route> RouteList { get; set; }

        public ObservableCollection<Station> Stations
        {
            get => _stations;
            set
            {
                _stations = value;
                OnPropertyChanged(nameof(Stations));
                OnPropertyChanged(nameof(StationId));
            }
        }

        public ObservableCollection<Train> Trains
        {
            get => _trains;
            set
            {
                _trains = value;
                OnPropertyChanged(nameof(Trains));
                OnPropertyChanged(nameof(TrainId));
            }
        }

        public ObservableCollection<Stops> TimeTable { get; set; }

        #endregion

        #region ControlProperties

        //selected index
        public int RouteSelected
        {
            get => _routeSelected;
            set
            {
                _routeSelected = value;
                OnPropertyChanged(nameof(RouteSelected));
                if (RouteSelected != 0)
                    SelectionCheck();
            }
        }

        //selected index
        public int ListDataIndex
        {
            get => _listDataIndex;
            set
            {
                _listDataIndex = value;
                if (ListDataIndex > 0)
                    UpdateDataGrid();
            }
        }

        //IsChecked
        public bool StationTicked
        {
            get => _stationTicked;
            set
            {
                _stationTicked = value;
                OnPropertyChanged(nameof(StationTicked));
                if (StationTicked)
                    OnSelectionChange(1);
            }
        }

        //IsChecked
        public bool TrainTicked
        {
            get => _trainTicked;
            set
            {
                _trainTicked = value;
                OnPropertyChanged(nameof(TrainTicked));
                if (TrainTicked)
                    OnSelectionChange(2);
            }
        }

        public int RouteSelected1
        {
            get => _routeSelected1;
            set
            {
                _routeSelected1 = value;
                OnPropertyChanged(nameof(RouteSelected1));
                if (RouteSelected1 > 0)
                    LoadData1();
            }
        }

        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }

        public bool RouteEnable
        {
            get => _routeEnable;
            set
            {
                _routeEnable = value;
                OnPropertyChanged(nameof(RouteEnable));
            }
        }

        public bool Checked1
        {
            get => _checked1;
            set
            {
                _checked1 = value;
                OnPropertyChanged(nameof(Checked1));
            }
        }

        public bool Checked2
        {
            get => _checked2;
            set
            {
                _checked2 = value;
                OnPropertyChanged(nameof(Checked2));
            }
        }

        public static int Errors { get; set; }

        #endregion

        #region Model

        //not equel 0
        public short TrainId { get; set; }

        //not equel 0
        public short StationId { get; set; }

        [Required]
        public string ATime1
        {
            get { return GetValue(() => ATime1); }
            set { SetValue(() => ATime1, value); }
        }

        [Required]
        public string DTime1
        {
            get { return GetValue(() => DTime1); }
            set { SetValue(() => DTime1, value); }
        }

        [Required]
        public bool Direction
        {
            get { return GetValue(() => Direction); }
            set { SetValue(() => Direction, value); }
        }

        #endregion

        public TimeTableVm()
        {
            LoadData();
            AddCommand = new CommandBase(action: AddRecord, canExecute: CheckValid);
            UpdateCommand = new CommandBase(action: UpdateData, canExecute: CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
            SelectDirectionCommand = new CommandBase(action: OnDirectionChange, canExecute: AlwaysTrue);
        }

        #region Command

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DataGridSelectionChangeCommand { get; }
        public ICommand SelectDirectionCommand { get; }

        #endregion

        #region Actions

        private void AddRecord()
        {
            var data = new StopAtDto
            {
                TID = TrainId,
                SID = StationId,
                Atime = ATime1.ToTime(),
                Dtime = DTime1.ToTime(),
                Direction = Direction
            };

            var response = WebConnect.PostData("TimeTables", data);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                DialogDisplayHelper.DisplayMessageBox("Action Completed", "Informative");
                if(data.TID==ListDataIndex||data.SID==ListDataIndex)
                {
                   TimeTable.Add(
                        new Stops
                        {
                            StationName = Stations.First(s => s.SID == StationId).Name,
                            TrainName = Trains.First(t=>t.TID==TrainId).Name,
                            ArriveTime = ATime1.RemovedDate(),
                            DepartureTime = DTime1.RemovedDate(),
                            Direction = data.Direction,
                            TrainId = data.TID,
                            StationId = data.SID
                        });
                }
                UpdateViewsProperties();
                return;
            }

            DialogDisplayHelper.DisplayMessageBox("Record Already Exist", "Informative");
        }

        public void UpdateData()
        {
            var data = new StopAtDto
            {
                TID = TrainId,
                SID = StationId,
                Atime = ATime1.ToTime(),
                Dtime = DTime1.ToTime(),
                Direction = Direction
            };
            HttpResponseMessage response = WebConnect.UpdateDate($"TimeTables?sid={StationId}&tid={TrainId}", data);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                DialogDisplayHelper.DisplayMessageBox("Update Completed", "Informative");
                var index = TimeTable.First(t =>
                    t.TrainId == data.TID && t.StationId == data.SID && t.Direction == data.Direction);
                index.ArriveTime = ATime1.RemovedDate();
                index.DepartureTime = DTime1.RemovedDate();
                ObservableCollection<Stops> temp = new ObservableCollection<Stops>(TimeTable);
                TimeTable.Clear();
                TimeTable = temp;
                OnPropertyChanged(nameof(TimeTable));
                return;
            }

            DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
        }

        private void Reset()
        {
            UpdateViewsProperties();
            RouteSelected = 0;
            ListData?.Clear();
            Trains?.Clear();
            Stations?.Clear();
            TimeTable?.Clear();
            StationTicked = false;
            TrainTicked = false;
            RouteEnable = true;
            TrainId = StationId = 0;
            OnPropertyChanged(nameof(TrainId));
            OnPropertyChanged(nameof(StationId));
        }

        private void OnDataGridSelectionChange(object data)
        {
            if (data == null)
                return;
            RouteEnable = false;
            RouteSelected1 = RouteSelected;

            var timetable = (Stops) data;
            TrainId = timetable.TrainId;
            StationId = timetable.StationId;
            ATime1 = timetable.ArriveTime;
            DTime1 = timetable.DepartureTime;

            Checked1 = timetable.Direction;
            Checked2 = timetable.Direction != true;
            IsEnable = false;
        }

        private void OnDirectionChange(object obj)
        {
            Direction = obj as string == "1" ? true : false;
        }

        private bool CheckValid()
        {
            if (Errors == 0 && RouteSelected1 != 0 && StationId != 0 && TrainId != 0&&(Checked1||Checked2)&&!string.IsNullOrWhiteSpace(ATime1)&&!string.IsNullOrWhiteSpace(DTime1))
                return true;
            return false;
        }

        #endregion

        #region Private

        private async void UpdateDataGrid()
        {
            var flag = (byte) (StationTicked ? 1 : (TrainTicked ? 2 : 0));
            if (flag == 0) return;
            short id = 0;
            id = flag == 1 ? (short) ListData[ListDataIndex].SID : (short) ListData[ListDataIndex].TID;
            try
            {
                TimeTable?.Clear();
                TimeTable = await Stops.GetTimeTableBySidOrTid(flag, id);
                OnPropertyChanged(nameof(TimeTable));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                DialogDisplayHelper.DisplayMessageBox("No Record Found With Selection", "Informative",
                    boxIcon: MessageBoxImage.Stop);
                ListDataIndex = 0;
                OnPropertyChanged(nameof(ListDataIndex));
            }
        }

        private async void OnSelectionChange(byte index)
        {
            if (RouteSelected == 0)
                return;

            var routeId = RouteList[RouteSelected].RID;
            ListData = new ObservableCollection<dynamic>();

            if (index == 1)
            {
                try
                {
                    var data = await Station.GetStationByRouteId(routeId);
                    foreach (Station station in data)
                    {
                        ListData.Add(station);
                    }
                }
                catch (Exception)
                {
                    DialogDisplayHelper.DisplayMessageBox("No Results Found", "Informative");
                    RouteSelected = 0;
                    return;
                }
            }

            else if (index == 2)
            {
                try
                {
                    var data = await Train.GetTrainByRouteId(routeId);
                    foreach (var station in data)
                    {
                        ListData.Add(station);
                    }
                }
                catch (Exception)
                {
                    DialogDisplayHelper.DisplayMessageBox("No Results Found", "Informative");
                    RouteSelected = 0;
                    return;
                }
            }

            OnPropertyChanged(nameof(ListData));
            ListDataIndex = 0;
            OnPropertyChanged(nameof(ListDataIndex));
        }

        private void UpdateViewsProperties()
        {
            ClearValidation();
            RouteSelected1 = 0;
            TrainId = StationId = 0;
            OnPropertyChanged(nameof(TrainId));
            OnPropertyChanged(nameof(StationId));
            ATime1 = DTime1 = "";
            Checked1 = Checked2 = false;
        }

        private async void LoadData()
        {
            RouteList = await Route.GetRouteList();
            OnPropertyChanged(nameof(RouteList));
        }

        private async void LoadData1()
        {
            IsEnable = true;
            IsEnable = true;

            var rid = RouteList[RouteSelected1].RID;

            try
            {
                Stations = await Station.GetStationByRouteId(rid);
            }
            catch (HttpRequestException)
            {
                DialogDisplayHelper.DisplayMessageBox("No Station Found Please Add Stations First", "Informative");
                RouteSelected1 = 0;
                return;
            }

            try
            {
                Trains = await Train.GetTrainByRouteId(rid);
            }
            catch (HttpRequestException)
            {
                DialogDisplayHelper.DisplayMessageBox("No Train Found Please Add Train First", "Informative");
                RouteSelected1 = 0;
            }
        }

        private void SelectionCheck()
        {
            if (StationTicked)
                OnSelectionChange(1);
            if (TrainTicked)
                OnSelectionChange(2);
        }

        #endregion
    }
}