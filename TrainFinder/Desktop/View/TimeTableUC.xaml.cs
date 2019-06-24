using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop
{
    
    public partial class TimeTableUc 
    {
        public TimeTableUc()
        {
            InitializeComponent();
            this.DataContext = new TimeTableVm();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) TimeTableVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) TimeTableVm.Errors -= 1;
        }
    }
}