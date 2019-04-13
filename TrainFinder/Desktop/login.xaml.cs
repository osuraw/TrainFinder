using System;
using System.Windows;
using Desktop.Model;

namespace Desktop
{

    public partial class Login : Window
    {
        private user _user;
        public Login()
        {
            InitializeComponent();
            _user = new user();
            this.DataContext = _user;
        }
        private void Btn_login_OnClick(object sender, RoutedEventArgs e)
        {
            _user.Password = txt_password.Password;
            var httpResponseMessage = webconnect.Webconnect.ParssData("/api/User/Login", _user);
            if (Boolean.Parse(httpResponseMessage.Content.ReadAsStringAsync().Result))
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