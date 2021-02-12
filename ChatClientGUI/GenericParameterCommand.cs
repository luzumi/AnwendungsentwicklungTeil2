using System;
using System.Windows.Input;

namespace ChatClientGUI
{
    class GenericParameterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> _executeAction;
        private readonly Func<bool> _canExecuteAction;

        public bool CanExecute(object parameter)
        {
            if (_canExecuteAction == null) return true;
            return _canExecuteAction.Invoke();
        }

        public GenericParameterCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            _executeAction = execute;
            if (canExecute != null)
                _canExecuteAction = canExecute;
        }
        public void Execute(object parameter) => _executeAction?.Invoke(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
