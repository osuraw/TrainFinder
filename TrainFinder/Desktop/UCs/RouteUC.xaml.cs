using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Desktop.Model;
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
            _routeVm=new RouteVM();
            this.DataContext = _routeVm;
              Debug.WriteLine(_routeVm.RoutesList.Count);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_routeVm.AddRoute()? "New Route Added Successfully": "Adding Failed", "Information", MessageBoxButton.OK,MessageBoxImage.Information);
            BtnClear_OnClick(this,e);
            
        }
        private void CmbRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dgv_Station.ItemsSource = "";
            Dgv_Train.ItemsSource = "";
            if (CmbRoutes.SelectedIndex!=0)
            {
                index = (byte)CmbRoutes.SelectedIndex;
                index--;
                _routeVm.LoadTableContent(index);
                //txtName.Text = _routeVm.RoutesList[index].Name;
                //txtdistance.Text = _routeVm.RoutesList[index].Distance.ToString();
                //txtdescription.Text = _routeVm.RoutesList[index].Distance.ToString();
                Dgv_Station.ItemsSource= _routeVm.Stations.ToArray();
                Dgv_Train.ItemsSource= _routeVm.Trains.ToArray();
                BtnAdd.IsEnabled = false;

            }

        }

        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            Dgv_Station.ItemsSource = "";
            Dgv_Train.ItemsSource = "";
            txtName.Text = "";
            txtdescription.Text = "";
            txtdistance.Text = "";
            CmbRoutes.SelectedIndex = 0;
            BtnAdd.IsEnabled = true;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CmbRoutes.SelectedIndex != 0)
            {
                _routeVm.DeleteRoute(_routeVm.RoutesList[index].RID);
                txtName.Text = _routeVm.RoutesList[index].Name;
                txtdistance.Text = _routeVm.RoutesList[index].Distance.ToString();
                txtdescription.Text = _routeVm.RoutesList[index].Distance.ToString();
                Dgv_Station.ItemsSource = _routeVm.Stations.ToArray();
                Dgv_Train.ItemsSource = _routeVm.Trains.ToArray();
                BtnAdd.IsEnabled = false;

            }
        }
    }
}