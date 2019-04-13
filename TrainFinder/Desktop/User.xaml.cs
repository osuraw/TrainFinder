using System.Data.Entity;
using System.Windows;
using Desktop.Model;
using Desktop.webconnect;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private user _user;
        public Window1()
        {
            InitializeComponent();
            _user = new user();
            this.DataContext = _user;
        }

        private void Btn_update_OnClick(object sender, RoutedEventArgs e)
        {
            //bool flag = Webconnect.ParssData("/User/Update",_user);
            //MessageBox.Show(flag ? "Successfully updated" : "Update failed", "Information", MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Btn_back_OnClick(object sender, RoutedEventArgs e)
        {
            var obj = new Login();
            obj.Show();
            Close();
        }

        private void Btn_clear_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            txt_name.Clear();
            txt_uname.Clear();
            ptxt_password.Clear();
            ptxt_cpassword.Clear();
        }
    }
}