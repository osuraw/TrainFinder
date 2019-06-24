using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop
{
    public partial class PinLocationUc
    {
        public PinLocationUc()
        {
            InitializeComponent();
            this.DataContext = new PinLocationVm();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) PinLocationVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) PinLocationVm.Errors -= 1;
        }
    }
}