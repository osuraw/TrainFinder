using System;
using System.Windows;
using Desktop.Model;

//using Desktop.View;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Main.xaml
    /// </summary>
    public partial class Main
    {
        public Main()
        {
            var dd = new object();
            InitializeComponent();
            var ssss = logininfor.UserId;
        }

        public void time(out object a)
        {
            a = DateTime.Now.TimeOfDay.ToString();
        }
        private void BtnHome_OnClick(object sender, RoutedEventArgs e)
        {
         
            //this.UcGrid.Content = new HomeUC();
        }
        private void BtnTrain_OnClick(object sender, RoutedEventArgs e)
        {
            this.UcGrid.Content=new TrainUC();
        }


        private void BtnStation_OnClick(object sender, RoutedEventArgs e)
        {
            this.UcGrid.Content=new StationUC();
        }

        private void BtnRoute_OnClick(object sender, RoutedEventArgs e)
        {
            this.UcGrid.Content = new RouteUC();
        }

        private void BtnTimeTable_Click(object sender, RoutedEventArgs e)
        {
            this.UcGrid.Content = new TimeTableUC();
        }

        private void BtnPinLocations_Click(object sender, RoutedEventArgs e)
        {
            this.UcGrid.Content =new PinLocationUC();
        }
    }
}