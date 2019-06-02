using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Desktop.Model;
using Desktop.ViewModels;

namespace Desktop
{
    /// <summary>
    ///     Interaction logic for StationUC.xaml
    /// </summary>
    public partial class StationUC 
    {
        public StationUC()
        {
            InitializeComponent();
            this.DataContext = new StationVm();
        }
      
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) StationVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) StationVm.Errors -= 1;
        }

        
    }
}