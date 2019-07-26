using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Desktop.Model;
using Microsoft.Maps.MapControl.WPF;

namespace Desktop.ViewModels
{
    class PinLocationVm : BaseViewModelMain
    {
        #region DataContaners

        public ObservableCollection<Route> Routes { get; set; }
        public ObservableCollection<PinLocation> PinLocations { get; set; }
        public ObservableCollection<Location> Locations { get; set; }

        #endregion

        #region Model

        public short RouteId { get; set; }
        public short LocationId { get; set; } = 0;
        [Required]
        public string Location { get; set; }
        public string Location1 { get; set; }
        public byte Type { get; set; } = 0;
        [Required]
        public string Message { get; set; }
        public string Description { get; set; }

        #endregion

        #region Control

        private short _selectRoute = 0;

        public short SelectRouteId
        {
            get => _selectRoute;
            set
            {
                _selectRoute = value;
                if (SelectRouteId > 0)
                    OnSelectRouteIdChange();
            }
        }

        public static int Errors { get; set; }

        public bool Checked1 { get; set; } = false;
        public bool Checked2 { get; set; } = false;

        #endregion

        #region Command

        public ICommand AddCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand DataGridSelectionChangeCommand { get; private set; }
        public ICommand LocationCommand { get; private set; }
        public ICommand SelectTypeCommand { get; private set; }

        #endregion

        public PinLocationVm()
        {
            LoadData();
            AddCommand = new CommandBase(action: AddStation, canExecute: CheckValid);
            UpdateCommand = new CommandBase(action: Update, canExecute: CheckValid);
            DeleteCommand = new CommandBase(action: Delete, canExecute: CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            SelectTypeCommand = new CommandBase(action: SetType, canExecute: AlwaysTrue);
            DataGridSelectionChangeCommand = new CommandBase(action: OnDataGridSelectionChange, canExecute: AlwaysTrue);
            LocationCommand = new CommandBase(action: SetLocation, canExecute: AlwaysTrue);
        }

        private void SetType(object obj)
        {
            Type = Convert.ToByte(obj.ToString());
        }

        private async void LoadData()
        {
            Routes = await Route.GetRouteList();
            OnPropertyChanged(nameof(Routes));
        }

        private void UpdateViewsProperties()
        {
            ClearValidation();
            RouteId = 0;
            Message = Description = "";
            LocationId = 0;
            Type = 0;
            Checked1 = false;
            OnPropertyChanged(nameof(Checked1));
            Checked2 = false;
            OnPropertyChanged(nameof(Checked2));
            Location = null;
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(RouteId));
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(Description));
        }

        private async void OnSelectRouteIdChange()
        {
            PinLocations?.Clear();
            try
            {
                PinLocations = await PinLocation.GetPinLocationList(Routes[SelectRouteId].RID);
                if (PinLocations.Count == 0)
                    throw new Exception();
                OnPropertyChanged(nameof(PinLocations));
                Locations = PinLocation.GetLocationList();
                OnPropertyChanged(nameof(Locations));
            }
            catch (Exception)
            {
                DialogDisplayHelper.DisplayMessageBox("No Pined Location Found","Informative");
                SelectRouteId = 0;
                OnPropertyChanged(nameof(SelectRouteId));
            }
        }

        private PinLocation GetViewData()
        {
            if (CheckValid())
                return new PinLocation
                {
                    RouteId = RouteId,
                    Message = Message,
                    Description = Description,
                    Type = Type,
                    PinId = LocationId,
                    Location = Location
                };
            return null;
        }


        #region ActionCommands

        public void AddStation()
        {
            var pinLocation = GetViewData();
            if (pinLocation.PinId != 0)
            {
                DialogDisplayHelper.DisplayMessageBox("All Ready Exist", "Informative");
                return;
            }

            var response = WebConnect.PostData("PinLocation/Add", pinLocation);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
                return;
            }

            UpdateViewsProperties();
            var ree = response.Content.ReadAsStringAsync();
            LocationId = pinLocation.PinId = short.Parse(ree.Result);

            DialogDisplayHelper.DisplayMessageBox("Action Completed", "Informative");
            if (pinLocation.RouteId == SelectRouteId)
            {
                PinLocations.Add(pinLocation);
                OnPropertyChanged(nameof(PinLocations));
            }

           
        }

        public void Update()
        {
            var pinLocation = GetViewData();
            var response = WebConnect.UpdateDate("PinLocation/Update?lid=" + pinLocation.PinId, pinLocation);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                DialogDisplayHelper.DisplayMessageBox("Update Completed", "Informative");
                var index = PinLocations.First(s => s.PinId == pinLocation.PinId);
                index.Location = pinLocation.Location;
                index.Message = pinLocation.Message;
                index.Type = pinLocation.Type;
                index.Description = pinLocation.Description;
                ObservableCollection<PinLocation> temp = new ObservableCollection<PinLocation>(PinLocations);
                PinLocations.Clear();
                PinLocations = temp;
                OnPropertyChanged(nameof(PinLocations));
                UpdateViewsProperties();
                return;
            }

            DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
        }

        private void Delete()
        {
            var response = WebConnect.DeleteData("PinLocation/Delete/" + LocationId);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                DialogDisplayHelper.DisplayMessageBox("Delete Completed", "Informative");
                //var index = PinLocations.IndexOf(LocationId);
                //PinLocations.Remove();
                OnPropertyChanged(nameof(PinLocations));
                UpdateViewsProperties();
                return;
            }

            DialogDisplayHelper.DisplayMessageBox("Action Failed", "Informative");
        }

        private void SetLocation(object location)
        {
            Location = location.ToString();
            OnPropertyChanged(nameof(Location));
        }

        private void OnDataGridSelectionChange(object pinLocations)
        {
            UpdateViewsProperties();
            if (pinLocations == null) return;
            var data = (PinLocation) pinLocations;
            RouteId = data.RouteId;
            LocationId = data.PinId;
            Location = data.Location;
            //Location1 = data.Location;
            OnPropertyChanged(nameof(Location));
            //OnPropertyChanged(nameof(Location1));
            Message = data.Message;
            OnPropertyChanged(nameof(Message));
            Description = data.Description;
            OnPropertyChanged(nameof(Description));
            Type = data.Type;
            if (Type == 1)
            {
                Checked1 = true;
                OnPropertyChanged(nameof(Checked1));
            }
            else if (Type == 2)
            {
                Checked2 = true;
                OnPropertyChanged(nameof(Checked2));
            }
        }

        private void Reset()
        {
            UpdateViewsProperties();
            PinLocations?.Clear();
            SelectRouteId = 0;
            OnPropertyChanged(nameof(SelectRouteId));
        }

        private bool CheckValid()
        {
            if (Errors == 0 && RouteId != 0 && Type != 0
                && !string.IsNullOrWhiteSpace(Location)
                && !string.IsNullOrWhiteSpace(Message))
                return true;
            return false;
        }

        #endregion
    }
}