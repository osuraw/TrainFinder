using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Model;
using Newtonsoft.Json;

namespace Desktop.ViewModels
{
    class ReportVm:BaseViewModelMain
    {
        #region properties

        public List<Train> Trains { get; set; }

        public short TrainId { get; set; }
        public string Parameter1 {
            get => GetValue(() => Parameter1);
            set { SetValue(() => Parameter1, DataValidation(value, Parameter2) ? value : ""); }
        }

        public string Parameter2
        {
            get => GetValue(() => Parameter2);
            set { SetValue(() => Parameter2, DataValidation(Parameter1, value) ? value : ""); }
        }

        public static int Errors { get; set; }

        public DataTable Table { get; set; }

        public bool DateValid { get; set; }

        #endregion

        #region Commands

        public ICommand GenerateCommand { get; }
        public ICommand ResetCommand { get; }

        #endregion

        public ReportVm()
        {
            GenerateCommand = new CommandBase(action: Generate, canExecute: CheckValid);
            ResetCommand = new CommandBase(action: Reset, canExecute: AlwaysTrue);
            GetData();
        }

        #region Methods

        private async void Generate()
        {
            var tempData = await WebConnect.GetData($"Search/Reports?trainId={TrainId}&parameter1={Parameter1}&parameter2={Parameter2}");
            Table = JsonConvert.DeserializeObject<DataTable>(tempData);         
        }

        private async void GetData()
        {
            Trains = await Train.GetTrains();
            OnPropertyChanged(nameof(Trains));
        }

        protected bool CheckValid()
        {
            if (Errors == 0 && TrainId != 0&&DateValid)
                return true;
            return false;
        }

        protected void Reset()
        {
            ClearValidation();
            Parameter1 = Parameter2 = "";
            TrainId = 0;
            OnPropertyChanged(nameof(TrainId));
        }

        private bool DataValidation(string para1,string para2)
        {
            if (DateTime.TryParse(para1, out var p1))
            {
                if (p1 > DateTime.Now)
                {
                    DialogDisplayHelper.DisplayMessageBox("Selected From Date Not Valid", "Error", boxIcon: MessageBoxImage.Warning);
                    return DateValid=false;
                }
                if (DateTime.TryParse(para2, out var p2))
                    if (p2 > DateTime.Now)
                    {
                        DialogDisplayHelper.DisplayMessageBox("Selected To Date Not Valid", "Error",
                            boxIcon: MessageBoxImage.Warning);
                        Parameter2 = "";
                        return DateValid = false;
                    }
                    else if (p1 > p2)
                    {
                        DialogDisplayHelper.DisplayMessageBox("Date Range Not Valid", "Error",
                            boxIcon: MessageBoxImage.Warning);
                        Parameter2 = "";
                        return DateValid = false;
                    }
                return DateValid=true;
            }
            DialogDisplayHelper.DisplayMessageBox("Set From Date First", "Error", boxIcon: MessageBoxImage.Warning);
            return false;
        }

        public async Task<DataTable> GetTableTemp()
        {
            var tempData = await WebConnect.GetData($"Search/Reports?trainId={TrainId}&parameter1={Parameter1}&parameter2={Parameter2}");
            return JsonConvert.DeserializeObject<DataTable>(tempData);
        }

        #endregion
    }
}
