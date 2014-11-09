using System;
using System.Windows.Input;

namespace IF.Lastfm.Syro.Helpers
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _executeAction;

        public DelegateCommand(Action executeAction)
        {
            _executeAction = executeAction;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }

        #endregion
    }
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeAction;

        public DelegateCommand(Action<T> executeAction)
        {
            _executeAction = executeAction;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void Execute(object parameter)
        {
            _executeAction((T)parameter);
        }

        #endregion
    }
}