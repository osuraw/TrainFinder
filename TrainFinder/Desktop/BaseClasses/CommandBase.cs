using System;
using System.Windows.Input;
using Desktop.ViewModels;

namespace Desktop
{
    public class CommandBase : ICommand
    {
        #region privateFeilds

        private readonly Func<bool> _func;
        private readonly Func<int, bool> _funcParameter1;
        private readonly Action _action;
        private readonly Action<object> _actionParamete1;
        private readonly Func<bool> _canExecute;

        #endregion

        #region Constructors

        public CommandBase(Action action, Func<bool> canExecute)
        {
            this._canExecute = canExecute;
            this._action = action;
        }
        public CommandBase(Action<object> action, Func<bool> canExecute)
        {
            this._canExecute = canExecute;
            this._actionParamete1 = action;
        }

        public CommandBase(Func<bool> func, Func<bool> canExecute)
        {
            this._canExecute = canExecute;
            this._func = func;
        }

        public CommandBase(Func<int, bool> funcParameter1, Func<bool> canExecute)
        {
            this._canExecute = canExecute;
            this._funcParameter1 = funcParameter1;
        }

        #endregion

        #region ICommand

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _func?.Invoke();
            _action?.Invoke();

            _funcParameter1?.Invoke((int) parameter);
            _actionParamete1?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion
    }
}