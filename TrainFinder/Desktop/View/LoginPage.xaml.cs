using System.Security;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for logpage.xaml
    /// </summary>
    public partial class LoginPage :BasePage<LoginPageVm>,IHavePassword
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        public SecureString SecureString => LoginPassword.SecurePassword;
        public SecureString SecureString1 =>null;
    }
}
