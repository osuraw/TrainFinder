using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Desktop.ViewModels;
using Fasetto.Word;

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
            LoginCommand =new CommandBase(action: async (parameter)=>await Login(parameter),canExecute:()=>true);
        }

        public async Task Login(object parameter)
        {
            await RunCommand(() => this.LoginIsRunning, async ()=>
            {
             await Task.Delay(5000);
            var username = UserName;
            var password=(parameter as IHavePassword).SecureString.UnsSecure();
            });
        }

       
    }
}
