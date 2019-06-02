using System.Threading;
using System.Windows.Input;

namespace Desktop.ViewModels
{
    public class MainVm : BaseViewModelMain
    {
        private BaseViewModelMain _viewContent;

        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string LogTime { get; set; }

        public double Opacity { get; set; } = 1;

        public BaseViewModelMain ViewContent
        {
            get => _viewContent;
            set
            {
                _viewContent = value;
                OnPropertyChanged(nameof(ViewContent));
            }
        }
        
        public ICommand NavigationCommand { get; set; }

        public MainVm()
        {
            NavigationCommand = new CommandBase(action: Navigation, canExecute: CheckValid);
        }

        
        private void Navigation(object window)
        {
            for (double i = 1; i > 0.0; i -= 0.1)
            {
                Opacity = i;
                Thread.Sleep(50);
            }
            switch (window as string)
            {
                case "Home": break;
                case "Route":ViewContent=new RouteVm();break;
                case "Train": ViewContent = new TrainVM(); break;
                case "Station":ViewContent=new StationVm(); break;
                case "PinLocation": break;
                case "TimeTable":ViewContent=new TimeTableVm(); break;
                case "User": break;
                case "Logout":break;
                default:break;
            }
            for (double i = 0; i < 1.1; i += 0.1)
            {
                Opacity = i;
                Thread.Sleep(50);
            }
        }

        protected bool CheckValid()
        {
            return true;
        }
    }
}