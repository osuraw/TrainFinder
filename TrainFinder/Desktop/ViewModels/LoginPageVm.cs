using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Desktop.ViewModels;

namespace Desktop
{
    public class LoginPageVm : BaseViewModel2
    {
        #region PrivetFields

        #endregion

        #region Properties

        public string UserName { get; set; }
        public SecureString Password { get; set; }


        public bool LoginIsRunning { get; set; } = false;

        #endregion

        #region Command

        public ICommand LoginCommand { get; set; }

        #endregion


        public LoginPageVm()
        {
            LoginCommand = new CommandBase(action: async (parameter) => await Login(parameter), canExecute: () => true);
        }


        public async Task Login(object parameter)
        {
            //await RunCommand(() => this.LoginIsRunning, async () =>
            //{
            //await Task.Delay(2000);
            var userName = UserName;
            var password = (parameter as IHavePassword).SecureString.UnsSecure();

            var response = WebConnect.PostData("user/login", new { Uname = userName, password });
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync();
                var page = (parameter as LoginPage);
                var win = Application.Current.MainWindow;
                new Main().Show();
                win.Close();
            }

            //});
        }
    }
}