using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for UserUc.xaml
    /// </summary>
    public partial class UserUc : IHavePassword
    {
        public UserUc()
        {
            InitializeComponent();
        }

        public SecureString SecureString =>Password.SecurePassword;
        public SecureString SecureString1 => ConfirmPassword.SecurePassword;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Password.Password=ConfirmPassword.Password=null;
        }
    }
}
