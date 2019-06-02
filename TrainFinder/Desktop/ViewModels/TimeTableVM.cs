using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
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

        #endregion

        #region DataLists

        public ObservableCollection<dynamic> ListData { get; set; }

        public ObservableCollection<Route> RouteList { get; set; }

        public ObservableCollection<Station> Stations { get; set; }

        public ObservableCollection<Train> Trains { get; set; }

        public ObservableCollection<Stopat> TimeTable { get; set; }

        #endregion

        #region ComtrolProperties

        public int RouteSelected
        {
            get => _routeSelected;
            set
            {
                _routeSelected = value;
                if (StationTicked)
                    OnSelectionChange(1);
                if (TrainTicked)
                    OnSelectionChange(2);
            }
        }

        public int ListDataIndex
        {
            get => _listDataIndex;
            set
            {
                _listDataIndex = value;
                UpdateDataGrid();
            }
        }

        public bool StationTicked
        {
            get => _stationTicked;
            set
            {
                _stationTicked = value;
                if (StationTicked)
                    OnSelectionChange(1);
            }
        }
        public bool TrainTicked
        {
            get => _trainTicked;
            set
            {
                _trainTicked = value;
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
                LoadData();
            }
        }

        private void LoadData()
        {
            var rid = RouteList[RouteSelected1].RID;
            Stations = Station.GetStationByRouteId(rid);
            Trains = Train.GetTrainByRouteId(rid);
            StationEnable = true;
            TrainEnable = true;
        }

        public bool StationEnable { get; set; }
        public bool TrainEnable { get; set; }

        public static int Errors { get; set; }
        #endregion

        #region Command

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DataGridSelectionChangeCommand { get; }

        #endregion

        #region Model

        public short TrainId { get; set; }
        public short StationId { get; set; }
        public string ATime1 { get; set; }
        public string DTime1 { get; set; }
        public string ATime2 { get; set; }
        public string DTime2 { get; set; }

        #endregion

        public TimeTableVm()
        {
            RouteList = Route.GetRouteList();
            AddCommand = new CommandBase(AddRecord, CheckValid);
            UpdateCommand = new CommandBase(UpdateData, CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
        }

        #region Actions

        private bool AddRecord()
        {
            Stopat[] data =
            {
                new Stopat
                {
                    TID = TrainId,
                    SID = StationId,
                    Atime = Convert.ToSingle(ATime1),
                    Dtime = Convert.ToSingle(DTime1),
                    Direction = true
                },
                new Stopat
                {
                    TID = TrainId,
                    SID = StationId,
                    Atime = Convert.ToSingle(ATime2),
                    Dtime = Convert.ToSingle(DTime2),
                    Direction = false
                }
            };
            return Stopat.AddRecordToTimeTable(data);
        }

        public bool UpdateData()
        {
            Stopat[] data =
            {
                new Stopat
                {

                    Atime = Convert.ToSingle(ATime1),
                    Dtime = Convert.ToSingle(DTime1),
                    Direction = true
                },
                new Stopat
                {

                    Atime = Convert.ToSingle(ATime2),
                    Dtime = Convert.ToSingle(DTime2),
                    Direction = false
                }
            };
            return Stopat.UpdateRecords(StationId, TrainId, data);
        }

        #endregion

        #region Overides

        //protected override void Reset()
        //{
        //    Clear();
        //    ListData?.Clear();
        //    Trains?.Clear();
        //    Stations?.Clear();
        //    TimeTable?.Clear();
        //}

        //protected override bool CheckValid()
        //{
        //    if (Errors == 0)
        //        return true;
        //    return false;
        //}
        protected void Reset()
        {
            Clear();
            ListData?.Clear();
            Trains?.Clear();
            Stations?.Clear();
            TimeTable?.Clear();
        }

        protected bool CheckValid()
        {
            if (Errors == 0)
                return true;
            return false;
        }

        #endregion

        #region Private

        private void UpdateDataGrid()
        {
            var flag = (byte)(StationTicked ? 1 : (TrainTicked ? 2 : 0));
            if (flag != 0)
            {
                short id = flag == 1 ? ((Station)ListData[ListDataIndex]).SID : ((Train)ListData[ListDataIndex]).TID;
                TimeTable = new ObservableCollection<Stopat>(Stopat.GetTimeTableBySidOrTid(flag, id));
            }
        }

        private void OnSelectionChange(byte index)
        {
            if (RouteSelected == 0)
                return;
            var routeId = RouteList[RouteSelected].RID;
            ListData = new ObservableCollection<dynamic>();

            if (index == 1)
            {
                ObservableCollection<Station> data = Station.GetStationByRouteId(routeId);
                foreach (Station station in data)
                {
                    ListData.Add(station);
                }
            }

            if (index == 2)
            {
                ObservableCollection<Train> data = Train.GetTrainByRouteId(routeId);
                foreach (Train station in data)
                {
                    ListData.Add(station);
                }
            }

            ListDataIndex = 0;
        }

        private void OnDataGridSelectionChange(object data)
        {
            var timetable = (Stopat)data;
            TrainId = timetable.TID;
            StationId = timetable.SID;
            ATime1 = timetable.Atime.ToString("F");
            DTime1 = timetable.Dtime.ToString("F");

        }

        private void Clear()
        {
            TrainId = StationId = 0;
        }

        #endregion
    }
}