using System;
using System.Windows;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            var dd = new object();
            InitializeComponent();
            //Console.WriteLine(System.Windows.SystemParameters.WorkArea.Width);
            //Console.WriteLine(System.Windows.SystemParameters.WorkArea.Height);
            //Thread time=new Thread(()=>this.time(out dd));
            //time.Start();
            //lbl_time2.Content = dd.ToString();
            //lbl_name.Content = logininfor.user.name;
            //lbl_logtime.Content = logininfor.logtime;
        }

        public void time(out object a)
        {
            a = DateTime.Now.TimeOfDay.ToString();
        }
        private void BtnHome_OnClick(object sender, RoutedEventArgs e)
        {
            this.RouteUC.Visibility = Visibility.Hidden;
            this.StationUc.Visibility = Visibility.Hidden;
            this.TrainUc.Visibility = Visibility.Hidden;
            this.TableUc.Visibility = Visibility.Hidden;
            this.LocationUc.Visibility = Visibility.Hidden;

        }
        private void BtnTrain_OnClick(object sender, RoutedEventArgs e)
        {
            this.TrainUc.Visibility = Visibility.Visible;
            this.TrainUc.BringIntoView();
        }


        private void BtnStation_OnClick(object sender, RoutedEventArgs e)
        {
            this.StationUc.Visibility = Visibility.Visible;
            this.StationUc.BringIntoView();
        }

        private void BtnRoute_OnClick(object sender, RoutedEventArgs e)
        {
            this.RouteUC.Visibility = Visibility.Visible;
        }

        private void BtnTimeTable_Click(object sender, RoutedEventArgs e)
        {
            this.TableUc.Visibility = Visibility.Visible;
        }

        private void BtnPinLocations_Click(object sender, RoutedEventArgs e)
        {
            this.LocationUc.Visibility = Visibility.Visible;
        }
    }
}