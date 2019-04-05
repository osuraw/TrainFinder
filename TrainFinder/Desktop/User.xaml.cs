using System.Windows;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Btn_update_OnClick(object sender, RoutedEventArgs e)
        {
            var obj = new User(1, txt_name.Text, txt_uname.Text, ptxt_password.Password, ptxt_cpassword.Password);
            var flag = obj.Update();
            MessageBox.Show(flag ? "Successfully updated" : "Update failed", "Information", MessageBoxButton.OK,
                MessageBoxImage.Information);
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