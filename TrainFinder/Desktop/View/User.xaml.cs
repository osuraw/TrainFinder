using System.Net;
using System.Windows;
using Desktop.Model;

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
            _user = new user(){Name = "ddd"};
            this.DataContext = _user;
        }

        private void Btn_update_OnClick(object sender, RoutedEventArgs e)
        {
            var flag = WebConnect.PostData("user/create", _user);
            if (flag.StatusCode==HttpStatusCode.Created)
            {
                MessageBox.Show("Successfully updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if(flag.StatusCode==HttpStatusCode.Conflict)
            {
                MessageBox.Show("Conflicting User Name Please Use Different User Name", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txt_uname.Text = "";
                
            }

        }

        private void Btn_back_OnClick(object sender, RoutedEventArgs e)
        {
            //var obj = new Login();
            //obj.Show();
            //Close();
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