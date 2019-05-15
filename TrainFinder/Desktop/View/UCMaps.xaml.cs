using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for UCMaps.xaml
    /// </summary>
    public partial class UCMaps : UserControl
    {
        private List<Location> LocationList;

        public UCMaps()
        {
            InitializeComponent();
            new MapEventArgs().Handled = false;
        }

        public Location Location { get; private set; }

        public void SetPusgPins(List<Location> list)
        {
            LocationList = list;
            foreach (var location in LocationList) DisplayLocation(location);
        }

        private void DisplayLocation(Location location)
        {
            var pin = new Pushpin();
            pin.Location = Location;
            Map.Children.Add(pin);
        }

        private void Map_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var mo = e.GetPosition(this);
            Location = Map.ViewportPointToLocation(mo);
            DisplayLocation(Location);
        }

        public void Clear()
        {
        }
    }
}