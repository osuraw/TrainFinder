using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop
{
    
    public partial class TimeTableUC : UserControl
    {
        public TimeTableUC()
        {
            InitializeComponent();
            this.DataContext = new TimeTableVm();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) StationVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) StationVm.Errors -= 1;
        }
    }
}