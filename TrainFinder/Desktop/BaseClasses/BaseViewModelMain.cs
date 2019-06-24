using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Desktop
{
    public class BaseViewModelMain : INotifyPropertyChanged, IDataErrorInfo
    {
        #region ClassMethods

        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue<T>(propertyName, value);
        }

        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            OnPropertyChanged(propertyName);
        }

        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }

        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            if (!_values.TryGetValue(propertyName, out object value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        private string GetPropertyName(LambdaExpression expression)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        private object GetValue(string propertyName)
        {
            if (!_values.TryGetValue(propertyName, out object value))
            {
                var propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
                if (propertyDescriptor == null)
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }

                value = propertyDescriptor.GetValue(this);
                _values.Add(propertyName, value);
            }

            return value;
        }

        protected bool AlwaysTrue() => true;

        #endregion

        #region INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfor

        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        private Dictionary<string, bool> ValidationResults { get; set; } = new Dictionary<string, bool>();

        public string this[string propertyName] => OnValidation(propertyName);

        protected virtual string OnValidation(string propertyName)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property not found");
                

            if (ValidationResults.Any(x => x.Key == propertyName))
            {
                var value = GetValue(propertyName);
                var results = new List<ValidationResult>(1);
                var result = Validator.TryValidateProperty(
                    value,
                    new ValidationContext(this, null, null)
                    {
                        MemberName = propertyName
                    },
                    results);
                if (!result)
                {
                    var validationResult = results.First();
                    error = validationResult.ErrorMessage;
                    ValidationResults[propertyName] = true;
                }
                else
                {
                    ValidationResults[propertyName] = false;
                }
            }
            else
            {
                ValidationResults.Add(propertyName, true);
            }

            OnPropertyChanged(nameof(IsValid));
            return error;
        }

        public string Error => null;

        public bool IsValid
        {
            get { return (!ValidationResults.Any(x => x.Value) && (ValidationResults.Count > 0)); }
        }

        public void ClearValidation()
        {
            ValidationResults.Clear();
        }

        #endregion
    }
}