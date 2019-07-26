using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Desktop.ViewModels
{
    class ControlVm:BaseViewModelMain
    {
        #region DataList

        public ObservableCollection<Log> ActiveTrains { get; set; }
        public ObservableCollection<InactiveTrains> InactiveTrain { get; set; }

        #endregion

        #region Data

        public static string Action { get; set; }
        public static string Direction { get; set; }

        public Log LogData
        {
            get { return GetValue(() => LogData); }
            set { SetValue(() => LogData, value); }
        }

        #endregion

        #region Command

        public ICommand ActionCommand { get; }

        public ICommand TrainSelectedCommand { get; }

        #endregion

        public ControlVm()
        {
            GetData();
            ActionCommand = new CommandBase(ActivateTrain, (() => true));
            TrainSelectedCommand = new CommandBase(TrainSelected, (() => true));
        }

        #region command Actions

        private void ActivateTrain(object obj)
        {
            if (obj == null || Action.Equals("SelectAction") || Direction.Equals("SelectDirection"))
                return;
            var data = obj as InactiveTrains;
            var response = WebConnect.UpdateDate("Train/AddTranToWatch", new { TrainId = data.TID, Status = Action, Direction = Direction.Equals("UpWay") });
            if (response.StatusCode == HttpStatusCode.OK)
            {
                GetData();
                DialogDisplayHelper.DisplayMessageBox("Activation Completed", "Informative");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                DialogDisplayHelper.DisplayMessageBox("Configuration Error Occur Check System Configurations", "System Error Detection", boxIcon: MessageBoxImage.Warning);
            }
            Action = "SelectAction";
            Direction = "SelectDirection";
        }

        private void TrainSelected(object obj)
        {
            LogData = obj as Log;
        }

        #endregion

        private async void GetData()
        {
            ActiveTrains = await Log.GetActiveTrains();
            OnPropertyChanged(nameof(ActiveTrains));
            InactiveTrain = await InactiveTrains.GetInActiveTrains();
            OnPropertyChanged(nameof(InactiveTrain));
        }
    }
}