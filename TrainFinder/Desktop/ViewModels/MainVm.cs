using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Desktop.Helpers;
using Desktop.Model;

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
            ViewContent = new ControlVm();
            NavigationCommand = new CommandBase(action: Navigation, canExecute: CheckValid);
            Name = LogInFor.User.Name;
            Date = DateTime.Today.ToString("D");
            LogTime = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss");
            new DispatcherTimer(TimeSpan.FromSeconds(1),DispatcherPriority.Normal,
                delegate 
                {
                    Time = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss");
                    OnPropertyChanged(nameof(Time));
                },Dispatcher.CurrentDispatcher);
        }

        
        private void Navigation(object window)
        {
            for (double i = 1; i > 0.0; i -= 0.1)
            {
                Opacity = i;
                Thread.Sleep(50);
            }

            if ("Logout".Equals(window.ToString()))
            {
                LoginWindow window1 =new LoginWindow();
                window1.Show();
                Application.Current.MainWindow.Close();
            }
            ViewContent = ViewFactory.GetView(window as string);
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