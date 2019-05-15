using System.Windows;
using Desktop.ViewModels;

namespace Desktop
{

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext=new LoginWindowVm(this);
        }

    }
}