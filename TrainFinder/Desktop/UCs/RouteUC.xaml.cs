using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
//using Desktop.entity;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Route.xaml
    /// </summary>
    public partial class Route : UserControl
    {
        public Route()
        {
            InitializeComponent();
            this.DataContext=new RouteVM();
              
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            RouteVM.AddRoute();
                //? MessageBox.Show("New Route Add Successfully", "Information", MessageBoxButton.OK,MessageBoxImage.Information);
        }
    }
}