using System.Windows;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private User user;

        public Login()
        {
            InitializeComponent();
        }


        private void Btn_login_OnClick(object sender, RoutedEventArgs e)
        {
            var flag = new User().Login(txt_user.Text, txt_password.Text);
            if (flag)
            {
                new Main().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Login Failed please check username and password", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}