using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Desktop.Model;

namespace Desktop.ViewModels
{
    class PinLocationVm
    {
        #region DataContaners

        public ObservableCollection<Route> Routes { get; set; }
        public ObservableCollection<Pinlocation> PinLocations { get; set; }

        #endregion

        #region Model

        public short RouteId { get; set; }
        public short LocationId { get; set; }
        public string Location { get; set; }
        public bool Type { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

        #endregion

        #region Command

        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand DataGridSelectionChangeCommand { get; private set; }
        public ICommand LocationCommand { get; private set; }


        #endregion

        public PinLocationVm()
        {
            PinLocations=new ObservableCollection<Pinlocation>();
            Routes = Route.GetRouteList();
        }


        #region Control

        //public short RouteIdSelectCmb1
        //{
        //    get => _routeIdSelectCmb1;
        //    set
        //    {
        //        _routeIdSelectCmb1 = value;
        //        OnRouteIdSelectCmb1Change();

        //    }
        //}

        public static int Errors { get; set; }

        #endregion

        //#region Methods

        //public StationVM()
        //{
        //    RoutesList = Route.GetRouteList();
        //    AddCommand = new CommandBase(AddStation, CheckValid);
        //    UpdateCommand = new CommandBase(Update, CheckValid);
        //    ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
        //    DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
        //    LocationCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
        //}

        //#region Action

        //public bool AddStation()
        //{
        //    var station = GetViewData();
        //    station.SID = 0;
        //    var response = WebConnect.PostData("Stations/AddStation", station);
        //    if (response.StatusCode == HttpStatusCode.Created)
        //    {
        //        var ree = response.Content.ReadAsStringAsync();
        //        station.SID = Int16.Parse(ree.Result);
        //        StationId = Int16.Parse(ree.Result);
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Update()
        //{
        //    var station = GetViewData();
        //    var response = WebConnect.UpdateDate("Stations/" + station.SID, station);
        //    if (response.StatusCode == HttpStatusCode.Created)
        //    {
        //        ClearViewProperties();
        //        return true;
        //    }

        //    return false;
        //}

        //#endregion

        //#region OverideBaseVM

        //protected override bool CheckValid()
        //{
        //    if (Errors == 0)
        //        return true;
        //    else
        //        return false;
        //}

        //protected override void Reset()
        //{
        //    ClearViewProperties();
        //    if (Station != null)
        //        Stations.Clear();
        //}

        //#endregion

        //#region PrivateHelpers

        //private Station GetViewData()
        //{
        //    if (CheckValid())
        //        return new Station()
        //        {
        //            RID = RouteId,
        //            Name = Name,
        //            Distance = float.Parse(Distance),
        //            Address = Address,
        //            Telephone = Telephone,
        //            SID = StationId
        //            //Location=Location
        //        };
        //    return null;
        //}

        //private void OnRouteIdSelectCmb1Change()
        //{
        //    Stations?.Clear();
        //    if (RouteIdSelectCmb1 != 0)
        //    {
        //        Stations = Station.GetStationByRouteId(RoutesList[RouteIdSelectCmb1].RID);
        //        Stations.RemoveAt(0);
        //    }
        //}

        //private void OnDataGridSelectionChange(object station)
        //{
        //    if (station != null)
        //    {
        //        Station data = (Station)station;
        //        RouteId = data.RID;
        //        Name = data.Name;
        //        Distance = data.Distance.ToString("##.##");
        //        Telephone = data.Telephone;
        //        Address = data.Address;
        //        Location = data.Locations;
        //        Description = data.Description;
        //        return;
        //    }
        //    ClearViewProperties();
        //}

        //private void ClearViewProperties()
        //{
        //    RouteIdSelectCmb1 = RouteId = 0;
        //    Name = Distance = Telephone = Address = Location = Description = "";
        //}

        //#endregion
        //#endregion
    }
}
