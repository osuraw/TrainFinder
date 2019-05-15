using System.Windows.Controls;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for PinLocationUC.xaml
    /// </summary>
    public partial class PinLocationUC : UserControl
    {
        public PinLocationUC()
        {
            InitializeComponent();
            this.DataContext = new PinLocationUC();
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) PinLocationVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) PinLocationVm.Errors -= 1;
        }
    }
}