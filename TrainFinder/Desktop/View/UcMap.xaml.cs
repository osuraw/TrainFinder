using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Desktop.Helpers;
using Microsoft.Maps.MapControl.WPF;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for UCMaps.xaml
    /// </summary>
    public partial class UcMap
    {
        public UcMap()
        {
            InitializeComponent();
            new MapEventArgs().Handled = false;
        }

        #region LocationListDependancyProp

        public static readonly DependencyProperty LocationListProperty = DependencyProperty.Register(
            "LocationList", typeof(List<Location>), typeof(UcMap), new PropertyMetadata(default(List<Location>), SetUpPins));

        public List<Location> LocationList
        {
            get => (List<Location>) GetValue(LocationListProperty);
            set => SetValue(LocationListProperty, value);
        }

        //For multiple adds
        private static void SetUpPins(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = d as UcMap;
            if(owner==null)
                throw new Exception("ucmaps setuppin");
            foreach (var location in owner.LocationList)
                owner.DisplayLocation(location);
        }

        #endregion

        #region LocatioDependancyProp

        private Location _location;

        public static readonly DependencyProperty LocationPinProperty = DependencyProperty.Register(
            "LocationPin", typeof(string), typeof(UcMap), new PropertyMetadata(default(string),OnLocationChange));

        public string LocationPin
        {
            get => (string) GetValue(LocationPinProperty);
            set => SetCurrentValue(LocationPinProperty, value);
        }
        // call when LocationPin change
        private static void OnLocationChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var maps = d as UcMap;
            maps?.Clear();
            maps._location = e.NewValue.ToLocation();
            maps.DisplayLocation(maps._location);
        }

        #endregion
        
        #region methods
        //single add
        private void DisplayLocation(Location location)
        {
            if(_location==null)
                return;
            var pin = new Pushpin {Location = location};
            Map.Children.Add(pin);
        }
        
        //clear pins
        private void Clear()
        {
            Map.Children?.Clear();
        }

        #endregion

        //MouseDouble Click
        private void Map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var mo = e.GetPosition(this);
            _location = Map.ViewportPointToLocation(mo);
            LocationPin = _location.LocationToString();//extension method
        }
    }
}