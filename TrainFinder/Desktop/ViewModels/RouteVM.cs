using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Desktop.command;
using Desktop.Model;

namespace Desktop.ViewModels
{
    public class RouteVM
    {
        #region Propety_constrtors

        private static route _route;
        private bool _canupdate;
        private RouteCommand _command;
        

        public route Route
        {
            get => _route;
            private set => _route = value;
        }

        public RouteVM()
        {
            Route = new route(this){Name = "",Distance=null};
            UpdateCommand = new RouteCommand(this);
           
        }

        #endregion

        #region Icommand

        public ICommand UpdateCommand
        {
            get;
            private set;
        }


        public bool CanUpdate
        {
            get => Route.IsValid;
            set => _canupdate = value;
        }

        #endregion

        #region send to server

        public static bool AddRoute()
        {
            webconnect.Webconnect.PostData("/api/Route", _route);
            return true;
        }



        #endregion
    }
}
