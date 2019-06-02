using System.Security;
using System.Windows;
using Desktop.ViewModels;
using Fasetto.Word;

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
    }
}
