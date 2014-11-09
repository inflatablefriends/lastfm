using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IF.Lastfm.Syro.Helpers
{
    public class AsyncDelegateCommand : ICommand
    {
        private readonly Func<Task> _executeAction;

        public AsyncDelegateCommand(Func<Task> executeAction)
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
            _executeAction.Invoke();
        }

        #endregion
    }

    public class AsyncDelegateCommand<T> : ICommand
    {
        private readonly Func<T, Task> _executeAction;

        public AsyncDelegateCommand(Func<T, Task> executeAction)
        {
            _executeAction = executeAction;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return parameter is T;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public async void Execute(object parameter)
        {
            await _executeAction((T) parameter);
        }

        #endregion
    }
}