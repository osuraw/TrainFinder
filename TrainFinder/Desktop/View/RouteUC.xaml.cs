﻿using System.Linq;
using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Route.xaml
    /// </summary>
    /// 
    public partial class RouteUC : UserControl
    {
        private byte index;
        private RouteVM _routeVm;

        public RouteUC()
        {
            InitializeComponent();
            _routeVm = new RouteVM();
            this.DataContext = _routeVm;

        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) RouteVM.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) RouteVM.Errors -= 1;
        }
        private void CmbRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DgvStation.ItemsSource = "";
            DgvTrain.ItemsSource = "";
            if (CmbRoutes.SelectedIndex > 0)
            {
                index = (byte) CmbRoutes.SelectedIndex;
                _routeVm.LoadTableContent(index);
                if (_routeVm.Stations != null)
                {

                    DgvStation.ItemsSource = _routeVm.Stations.ToArray();
                }

                if (_routeVm.Trains!=null)
                {
                    DgvTrain.ItemsSource = _routeVm.Trains.ToArray();
                }
                //check user-ability
                BtnAdd.IsEnabled = false;
            }
            else
            {
                //_routeVm.Reset();
            }
        }
    }
}