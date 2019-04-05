using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Desktop.ViewModels;

namespace Desktop.command
{
    class RouteCommand : ICommand
    {
        private RouteVM _routeVm;

      
        public RouteCommand(RouteVM routeVm)
        {
            _routeVm = routeVm;
        }

        public void Execute(object parameter)
        {
            //_personalDetails.SaveCharges();
        }

        public bool CanExecute(object parameter)
        {
            //Debug.WriteLine("++++++" + _routeVm.CanUpdate);
            return _routeVm.CanUpdate;
           // return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
