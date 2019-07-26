using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Desktop
{
    public enum ApplicationPage
    {
        None = 0,
        LoginPage = 1,
    }

    public class LoginWindowVm: BaseViewModel2
    {
        #region PrivetFields

        private readonly Window _loginWindow;
        private int _outerMarginSize=10;
        private int _windowRadius = 10;

        #endregion

        #region Properties

        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.LoginPage;

        public int MinWidth { get; set; } = 400;
        public int MinHeight { get; set; } = 400;

        public int TitleHeight { get; set; } = 40;
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        public int ResizeBorder { get; set; } = 6;
        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder+OuterMarginSize);

        public Thickness InnerContentPadding => new Thickness(ResizeBorder);

        public int OuterMarginSize
        {
            get => _loginWindow.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            set => _outerMarginSize = value;
        }
        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        public int WindowRadius
        {
            get => _loginWindow.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            set => _windowRadius = value;
        }
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        #endregion

        #region Command

        public ICommand MenuCommand  { get; set; }
        public ICommand Minimize { get; set; }
        public ICommand Maximize { get; set; }
        public ICommand Close { get; set; }

        #endregion

        public LoginWindowVm(Window loginWindow)
        {
            _loginWindow = loginWindow;
            this._loginWindow.StateChanged += OnStateChange;

            MenuCommand = new CommandBase(() => SystemCommands.ShowSystemMenu(_loginWindow, GetMousePosition()),()=>true);
            Minimize = new CommandBase(() => SystemCommands.MinimizeWindow(_loginWindow), () => true);
            Maximize = new CommandBase(() => SystemCommands.MaximizeWindow(_loginWindow), () => true);
            Close = new CommandBase(() => SystemCommands.CloseWindow(_loginWindow), () => true);

            Task.Run(action:WebConnect.CheckConnection);

            //Thread.CurrentThread.Name = "Main";

            var reSizer =new WindowResize(_loginWindow);
        }

        #region privateMethods

        private void OnStateChange(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ResizeBorder));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
        }

        private Point GetMousePosition()
        {
            var position = Mouse.GetPosition(_loginWindow);
            position.X += _loginWindow.Left; position.Y += _loginWindow.Top;

            return position;
        }

        #endregion
    }
}
