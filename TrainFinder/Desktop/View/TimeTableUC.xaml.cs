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
            if (e.Action == ValidationErrorEventAction.Added) StationVM.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) StationVM.Errors -= 1;
        }
    }
}