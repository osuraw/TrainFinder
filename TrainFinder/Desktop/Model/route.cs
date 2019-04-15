using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Desktop.ViewModels;

namespace Desktop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class route:IDataErrorInfo,INotifyPropertyChanged
    {
        private RouteVM _routeVm;

        #region Entity
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public route()
        {
        }

        public route(RouteVM routeVm)
        {
            this._routeVm = routeVm;
        }

        public short RID { get; set; }
        public int Sstation { get; set; }
        public int Estation { get; set; }
        public float Distance { get; set; }
        public string Description { get; set; } 
        public string Name { get; set; }
         #endregion

        #region model

        #region dataerrorinfo

        /// <summary>
        /// errors store dictionary
        /// </summary>
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();


        public string this[string PropertyName]
        {
            get
            {
                return GetValidationError(PropertyName);
            }
        }

        public string Error
        { get=>null; }

        private static readonly string[] validationPropeties =
        {"Name","Distance"};

        public bool IsValid
        {
            get
            {
                foreach (string property in validationPropeties)
                    if (GetValidationError(property) != null)
                        return false;
                return true;
            }
        }

        private string GetValidationError(string PropertyName)
        {

            string error = null;
            
            switch (PropertyName)
            {
               case "Name":error= Validations.NullEmptyStringValidation(Name) ?"Please Ente Route Name": Validations.HasNumber(Name) ? "Numbers are not allowed" : null;SetErrorMessage("txtName", error); break;
               case "Distance": error = Validations.NullEmptyStringValidation(Distance.ToString()) ? "Please enter Distance" :Validations.DigitCheck(Distance.ToString()) ? "Numbers Only":null; SetErrorMessage("txtdistance", error); break;
               default:break;
            }
            Debug.WriteLine("error message "+error);
            return error;
        }

        private void SetErrorMessage(string key, string message)
        {
            if (ErrorCollection.ContainsKey(key) && message != null)
            {
                ErrorCollection[key] = message;
            }
            else if (message != null)
            {
                ErrorCollection.Add(key, message);
            }
            OnPropertyChanged("ErrorCollection");
        }

        #endregion

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        #endregion

        #region methods Returns

        #endregion

    }
}
