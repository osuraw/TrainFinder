using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop
{
    public partial class ControlUc
    {
        public ControlUc()
        {
            InitializeComponent();
        }

        //Move to control if possible
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControlVm.Direction = e.AddedItems[0].ToString();           
        }

        private void Direction_Change(object sender, SelectionChangedEventArgs e)
        {
            ControlVm.Action = e.AddedItems[0].ToString();
        }
    }
}
