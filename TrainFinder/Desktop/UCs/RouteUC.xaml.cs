using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
//using Desktop.entity;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Route.xaml
    /// </summary>
    /// 
    public partial class Route : UserControl
    {
        private byte index;
        private RouteVM _routeVm;

        public Route()
        {
            InitializeComponent();
            _routeVm = new RouteVM();
            this.DataContext = _routeVm;
            Debug.WriteLine(_routeVm.RoutesList.Count);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_routeVm.AddRoute() ? "New Route Added Successfully" : "Adding Failed", "Information",MessageBoxButton.OK, MessageBoxImage.Information);
            BtnClear_OnClick(this, e);
        }

        private void CmbRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dgv_Station.ItemsSource = "";
            Dgv_Train.ItemsSource = "";
            if (CmbRoutes.SelectedIndex > 0)
            {
                index = (byte) CmbRoutes.SelectedIndex;
                index--;
                _routeVm.LoadTableContent(index);
                Dgv_Station.ItemsSource = _routeVm.Stations.ToArray();
                Dgv_Train.ItemsSource = _routeVm.Trains.ToArray();
                //check user-ability
                BtnAdd.IsEnabled = false;
            }
            else
            {
                _routeVm.Reset();
            }
        }

        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            Dgv_Station.ItemsSource = "";
            Dgv_Train.ItemsSource = "";
            _routeVm.Reset();
            CmbRoutes.SelectedIndex = 0;
            BtnAdd.IsEnabled = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CmbRoutes.SelectedIndex != 0)
            {
                _routeVm.DeleteRoute(index);
                CmbRoutes.SelectedIndex = 0;
                BtnAdd.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("No route Selected", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (CmbRoutes.SelectedIndex != 0)
            {
                MessageBox.Show(_routeVm.Update(index) ? "Route Update Successfully" : "Update Failed", "Information",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No route Selected", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            CmbRoutes.SelectedIndex = 0;
        }
    }
}